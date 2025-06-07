using System.Security.Claims;
using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Data;

var builder = WebApplication.CreateBuilder(args);

//Add Key Vault as configuration source
var vaultUri = new Uri(builder.Configuration["KeyVault:VaultUri"]!);
builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());

//Read secrets from configuration (Key Vault)
var connString = builder.Configuration.GetConnectionString("DefaultConnection")!;
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;
var jwtExpiresMin = builder.Configuration["Jwt:ExpiresInMinutes"]!;

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            policy
              .WithOrigins("https://ashy-cliff-0942cde03.6.azurestaticapps.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
        }
    });
});

//Entity Framework Core
builder.Services.AddDbContext<DataContext>(opts =>
    opts.UseSqlServer(connString));

//OpenAPI/Swagger (ingen ändring här)
builder.Services.AddOpenApi();

//Authentication / JWT
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.RequireHttpsMetadata = true;
      options.SaveToken = true;
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtIssuer,
          ValidAudience = jwtAudience,
          IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
          RoleClaimType = ClaimTypes.Role

      };
      options.Events = new JwtBearerEvents
      {
          OnMessageReceived = ctx =>
          {
              Console.WriteLine("Auth-header: " + ctx.Request.Headers["Authorization"]);
              return Task.CompletedTask;
          },
          OnAuthenticationFailed = ctx =>
          {
              Console.WriteLine("JWT-validering FEL: " + ctx.Exception.Message);
              return Task.CompletedTask;
          }
      };
  });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

//Swagger-gen type helt chat gptad eftersom den surade och inte tog in token ordentligt 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventService API", Version = "v1" });

    
    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,   
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "enter: {token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme          
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, bearerScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        
        { bearerScheme, Array.Empty<string>() }
    });
});


var app = builder.Build();

//Middleware-pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventService API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.MapOpenApi();
app.Run();
