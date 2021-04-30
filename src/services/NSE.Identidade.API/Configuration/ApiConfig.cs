using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;
using NFE.WepAPI.Core.Identidade;
using NFE.WepAPI.Core.Usuario;

namespace NSE.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //localhost/jwks
            //app.UseJwksDiscovery("/minha-chave");

            //localhost/jwks
            app.UseJwksDiscovery();

            return app;
        }
    }
}
