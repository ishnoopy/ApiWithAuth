using ApiWithAuth;
using ApiWithAuth.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore.Extensions;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DOCU: Retrieve configuration from appsettings.json, this is where you've set your issuer and audience values for best practice.
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// DOCU: Adding this to the dependency container allows us to inject this service to our controllers so we can perform CRUD operations.
builder.Services.AddEntityFrameworkMySQL().AddDbContext<JwtDemoContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// DOCU: To allow attaching of access token to requests for SwaggerUI
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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

// DOCU: Add a JWT Token authentication scheme.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtIssuer"],
            ValidAudience = configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtKey"])
            ),
        };
    });

// DOCU: Register the TokenService class so that it can be injected to other class via dependency injection (Adding it to the recipient class' constructor), more like what we did in the IConfiguration in TokenService.cs.
builder.Services.AddScoped<TokenService, TokenService>();

/* DOCU: You can add policy for more complex authorization such as multiple roles for a particular api endpoint.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
         policy.RequireRole("admin"));

    options.AddPolicy("StudentPolicy", policy =>
     policy.RequireRole("student"));
});
*/
var app = builder.Build();

// DOCU: Configure the HTTP request pipeline. This is for using SwaggerUI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//DOCU: Add useAuthentication method.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
