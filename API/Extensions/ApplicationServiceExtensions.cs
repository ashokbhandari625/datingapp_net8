using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Inerfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //  services.AddCors();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
 



            // services.AddCors(options =>
            // {
            //     options.AddPolicy("AllowSpecificOrigins", policy =>
            //     {
            //         policy.WithOrigins("https://localhost:4200/").AllowAnyHeader().AllowAnyMethod();
            //     });
            //     options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            // });
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddDbContext<DataContext>(opt =>
            {

                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>(); // created ones per client http request 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoService, PhotoService>() ; 
            services.AddScoped<LogUserActivity>() ; 
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); 
            return services;
        }

    }
}