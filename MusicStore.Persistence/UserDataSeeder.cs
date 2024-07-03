using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Entities;

namespace MusicStore.Persistence;

public static class UserDataSeeder
{
	public static async Task Seed(IServiceProvider serviceProvider)
	{
		// User repository
		var userManager = serviceProvider.GetRequiredService<UserManager<MusicStoreUserIdentity>>();
		// Role Repository
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		// Creating roles
		var adminRole = new IdentityRole(Constants.RoleAdmin);
		var customerRole = new IdentityRole(Constants.RoleUser);

		if (!await roleManager.RoleExistsAsync(Constants.RoleAdmin))
			await roleManager.CreateAsync(adminRole);

		if (!await roleManager.RoleExistsAsync(Constants.RoleUser))
			await roleManager.CreateAsync(customerRole);

		// Admin User
		var adminUser = new MusicStoreUserIdentity()
		{
			FirstName = "System",
			LastName = "Administrator",
			UserName = "admin@gmail.com",
			Email = "admin@gmial.com",
			PhoneNumber = "123456789",
			Age = 30,
			DocumentType = DocumentTypeEnum.Dni,
			DocumentNumber = "12345678",
			EmailConfirmed = true
		};

		if (await userManager.FindByEmailAsync("admin@gmial.com") is null)
		{
			var result = await userManager.CreateAsync(adminUser, "Admin1234*");
			if (result.Succeeded)
			{
				// Obtener el resgitro del usuario
				adminUser = await userManager.FindByEmailAsync(adminUser.Email);
				
				if (adminUser is not null)
				{
					// Asignar el rol de administrador al usuario
					await userManager.AddToRoleAsync(adminUser, Constants.RoleAdmin);
				}
			}
		}
	}
	
}
