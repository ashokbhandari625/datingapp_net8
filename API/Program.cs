using System.Data.Common;
using System.Text;
using API.Data;
using API.Extensions;
using API.Inerfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options=> {
        var tokenkey = builder.Configuration["TokenKey"]?? throw new Exception("Token Key not found");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(tokenkey) ) ,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>() ; 
// Configure the HTTP request pipeline.

app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()) ; 
app.UseAuthentication() ;
app.UseAuthorization() ;    

app.MapControllers();
using var scope=app.Services.CreateScope();

var services = scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context) ; 
}
catch( Exception ex ){
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration"); 
}
app.Run();
