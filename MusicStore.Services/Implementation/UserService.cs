using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Persistence;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace MusicStore.Services.Implementation;

public class UserService : IUserService
{
	private readonly UserManager<MusicStoreUserIdentity> _userManager;
	private readonly SignInManager<MusicStoreUserIdentity> _signInManager;
	private readonly ILogger<UserService> _logger;
	private readonly ICustomerRepository _customerRepository;
	private readonly IOptions<AppSettings> _appSettings;
	private readonly IEmailService _emailService;

	public UserService(
		UserManager<MusicStoreUserIdentity> userManager, 
		SignInManager<MusicStoreUserIdentity> signInManager, 
		ILogger<UserService> logger, 
		ICustomerRepository customerRepository, 
		IOptions<AppSettings> appSettings,
		IEmailService emailService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_logger = logger;
		_customerRepository = customerRepository;
		_appSettings = appSettings;
		_emailService = emailService;
	}
	public async Task<BaseResponseGeneric<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
	{
		var response = new BaseResponseGeneric<RegisterResponseDto>();

		try
		{
			var user = new MusicStoreUserIdentity
			{
				UserName = registerRequestDto.Email,
				Email = registerRequestDto.Email,				
				FirstName = registerRequestDto.FirstName,
				LastName = registerRequestDto.LastName,
				Age = registerRequestDto.Age,
				DocumentType = (DocumentTypeEnum)registerRequestDto.DocumentType,
				DocumentNumber = registerRequestDto.DocumentNumber,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
			if (result.Succeeded)
			{
				user = await _userManager.FindByEmailAsync(user.Email);
				if (user is not null)
				{
					await _userManager.AddToRoleAsync(user, Constants.RoleUser);

					var customer = new Customer()
					{
						Email = registerRequestDto.Email,
						FullName = $"{registerRequestDto.FirstName} {registerRequestDto.LastName}",
					};

					await _customerRepository.AddAsync(customer);

					// TODO: Enviar correo de confirmación

					response.Success = true;

					var tokenResponse = await ConstruirToken(user); // returnig token
					response.Data = new RegisterResponseDto
					{
						UserId = user.Email,						
						Token = tokenResponse.Token,
						ExpirationDate = tokenResponse.ExpirationDate
					};
				}
			}
			else
			{				
				response.ErrorMessage = String.Join(" ", result.Errors.Select(x => x.Description).ToArray());
				_logger.LogWarning(response.ErrorMessage);
			}
		}
		catch (Exception ex)
		{			
			response.ErrorMessage = "Ocurrió un error al registrar el usuario.";
			_logger.LogError(ex, "{ErrorMessage} {Mensaje}", response.ErrorMessage, ex.Message);			
		}

		return response;
	}

	public async Task<BaseResponseGeneric<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
	{
		var response = new BaseResponseGeneric<LoginResponseDto>();

		try
		{
			var resultado = await _signInManager.PasswordSignInAsync(loginRequestDto.Username, loginRequestDto.Password, isPersistent: false, lockoutOnFailure: false);
			if (resultado.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
				response.Success = true;
				response.Data = await ConstruirToken(user);
			}
			else
			{
				response.ErrorMessage = "Usuario o contraseña incorrectos.";
			}
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Ocurrió un error al iniciar sesión.";
			_logger.LogError(ex, "{ErrorMessage} {Mensaje}", response.ErrorMessage, ex.Message);
		}

		return response;
	}

	private async Task<LoginResponseDto> ConstruirToken(MusicStoreUserIdentity user) 
	{
		var claims = new List<Claim>
		{
			//new Claim(ClaimTypes.NameIdentifier, user.Id),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
			//new Claim(ClaimTypes.GivenName, user.FirstName),
			//new Claim(ClaimTypes.Surname, user.LastName),
			//new Claim(ClaimTypes.Role, Constants.RoleUser)
		};

		var roles = await _userManager.GetRolesAsync(user);
		foreach (var rol in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, rol));
		}

		// firmando el JWT
		var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Jwt.JWTKey));
		var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
		var expiracion = DateTime.UtcNow.AddSeconds(_appSettings.Value.Jwt.LifetimeInSeconds);

		var securityToken = new JwtSecurityToken(
			issuer: null,
			audience: null,
			claims: claims,
			expires: expiracion,
			signingCredentials: credenciales
		);

		return new LoginResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
			ExpirationDate = expiracion
		};
	}

	public async Task<BaseResponse> RequestTokenToResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
	{
		var response = new BaseResponse();
		
		try
		{
			var userIdentity = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
			if (userIdentity is null)
			{
				throw new SecurityException("Usuario no existe");
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);

			// Enviar un email con el token para resetear la contraseña
			await _emailService.SendEmailAsync(resetPasswordRequestDto.Email, "Reestablecer clave", 
				@$"
					<p> Estimado {userIdentity.FirstName} {userIdentity.LastName}: </p>
					<p> Para reestablecer su clave, por favor copie el siguiente código</p>
					<p> <strong>{token}</strong> </p>
					<hr/>
					Atte. <br/>
					Music Store @ 2024
				"	
			);

			response.Success = true;
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Ocurrió un error al solicitar el token para reestablecer la contraseña.";
			_logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
		}

		return response;
	}

	public async Task<BaseResponse> ResetPasswordAsync(NewPasswordRequestDto requestDto)
	{
		var response = new BaseResponse();

		try
		{
			var userIdentity = await _userManager.FindByEmailAsync(requestDto.Email);
			if (userIdentity is null)
			{
				throw new SecurityException("Usuario no existe");
			}

			var result = await _userManager.ResetPasswordAsync(userIdentity, requestDto.Token, requestDto.ConfirmNewPassword);
			response.Success = result.Succeeded;

			if (!result.Succeeded)
			{
				response.ErrorMessage = String.Join(" ", result.Errors.Select(x => x.Description).ToArray());				
			}
			else
			{
				// Enviar un email de confirmación de clave cambiada
				await _emailService.SendEmailAsync(requestDto.Email, "Confirmación de cambio de clave",
					@$"
						<p> Estimado {userIdentity.FirstName} {userIdentity.LastName}: </p>
						<p> Se ha reestablecido su clave correctamente. </p>
						<hr/>
						Atte. <br/>
						Music Store @ 2024
					");
			}
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Ocurrió un error al resetear la clave";
			_logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
		}

		return response;
	}

	public async Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequestDto requestDto)
	{
		var response = new BaseResponse();

		try
		{
			var userIdentity = await _userManager.FindByEmailAsync(email);
			if (userIdentity is null)
			{
				throw new SecurityException("Usuario no existe");
			}

			var result = await _userManager.ChangePasswordAsync(userIdentity, requestDto.OldPassword, requestDto.NewPassword);
			response.Success = result.Succeeded;

			if (!result.Succeeded)
			{
				response.ErrorMessage = String.Join(" ", result.Errors.Select(x => x.Description).ToArray());
			}
			else
			{
				_logger.LogInformation("Se cambió la clave correctamente para {email}", userIdentity.Email);

				// Enviar un email de confirmación de clave cambiada
				await _emailService.SendEmailAsync(email, "Confirmación de cambio de clave",
					@$"
						<p> Estimado {userIdentity.FirstName} {userIdentity.LastName}: </p>
						<p> Se ha cambiado su clave correctamente. </p>
						<hr/>
						Atte. <br/>
						Music Store @ 2024
					");
			}
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Error al cambiar la clave";
			_logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
		}

		return response;
	}		
}
