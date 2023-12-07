using TechTest.Application;
using TechTest.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechTest.API.Middleware;
using Microsoft.OpenApi.Models;
using TechTest.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Adding keys
string jwt = builder.Configuration.GetSection("JWT:Key").Value;

// DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddMultitenancy();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "User",
        };
    });

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TenantMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("getall", "{__tenant__}/{controller=Products}/{action=GetAll}");
    endpoints.MapControllerRoute("getbyid", "{__tenant__}/{controller=Products}/{action=GetById}");
    endpoints.MapControllerRoute("create", "{__tenant__}/{controller=Products}/{action=Create}");
    endpoints.MapControllerRoute("update", "{__tenant__}/{controller=Products}/{action=Update}");
    endpoints.MapControllerRoute("delete", "{__tenant__}/{controller=Products}/{action=Delete}");
});
app.Run();
