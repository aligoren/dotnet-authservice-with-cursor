using AuthenticationService.Configuration;
using AuthenticationService.Endpoints;
using AuthenticationService.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var multiTenantConfig = builder.Configuration.Get<MultiTenantConfiguration>()
    ?? throw new InvalidOperationException("Configuration is missing");

// Konfigürasyon servislerini kaydet
builder.Services.AddSingleton(multiTenantConfig);
builder.Services.AddSingleton(multiTenantConfig.Jwt);
builder.Services.AddSingleton(multiTenantConfig.OAuth2);

// Swagger
builder.Services.AddSwaggerConfiguration();

// Servisler
builder.Services.AddApplicationServices(builder.Configuration);

// JWT Kimlik Doğrulama
builder.Services.AddJwtAuthentication(multiTenantConfig.Jwt);

// Yetkilendirme
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoint'leri kaydet
app.MapAuthEndpoints();
app.MapTestEndpoints();

app.Run();
