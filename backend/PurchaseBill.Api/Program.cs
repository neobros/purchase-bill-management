using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PurchaseBill.Api.Data;
using PurchaseBill.Api.Helpers;
using PurchaseBill.Api.Interfaces;
using PurchaseBill.Api.Middleware;
using PurchaseBill.Api.Models;
using PurchaseBill.Api.Repositories;
using PurchaseBill.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configuration binding ----------
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApi"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt configuration section is missing.");
var externalApiSettings = builder.Configuration.GetSection("ExternalApi").Get<ExternalApiSettings>()
    ?? throw new InvalidOperationException("ExternalApi configuration section is missing.");

// ---------- EF Core / SQL Server ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ---------- HttpClient for the external POS API ----------
// The external API sits behind Azure App Service's load balancer and returns a
// Set-Cookie: ARRAffinity cookie to pin the client to one backend instance. That
// instance appears to return an empty/default GetLoginData response on a second
// call for the same device — disabling cookie handling means every request is
// routed independently instead of getting stuck on one (misbehaving) instance.
builder.Services.AddHttpClient<IExternalPosApiClient, ExternalPosApiClient>(client =>
{
    client.BaseAddress = new Uri(externalApiSettings.BaseUrl);
}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    UseCookies = false,
});

// ---------- Dependency injection: repositories & services ----------
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPurchaseBillRepository, PurchaseBillRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPurchaseBillService, PurchaseBillService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// ---------- Global exception handling (.NET 8 IExceptionHandler) ----------
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ---------- Authentication / Authorization ----------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });
builder.Services.AddAuthorization();

// ---------- MVC / Swagger ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Purchase Bill API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token returned by /api/auth/login."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ---------- CORS (Angular dev server) ----------
const string AngularCorsPolicy = "AngularClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---------- HTTP pipeline ----------
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AngularCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
