using CivicPlusChallenge;
using CivicPlusChallenge.Configuration;
using CivicPlusChallenge.Services;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EventsApiConfig>(options =>
{
    builder.Configuration.GetSection(nameof(EventsApiConfig)).Bind(options);
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddSwaggerGen();

builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddHttpClient("AuthenticatedHttpClient").AddHttpMessageHandler<AuthenticationHandler>();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins ?? [])
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

app.UseCors();

app.MapAllEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "CivicPlus Challenge API v1");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();