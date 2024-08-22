using MusicStore.Api.Endpoints;

namespace MusicStore.Api.Configurations
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseMiddlewareExtensions(this IApplicationBuilder app, string corsConfiguration)
		{
			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseCors(corsConfiguration);

			return app;
		}

		public static void UseMiddlewareEndpoints(this IEndpointRouteBuilder app)
		{
			app.MapReportEndpoints();
			app.MapHomeEndpoints();
			app.MapControllers();
		}
	}
}
