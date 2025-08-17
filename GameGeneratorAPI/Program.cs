using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Lägg till CORS-tjänsten i containern.
// Detta skapar en policy som tillåter anrop från din React-app.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // URL:en för din React-app
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Lägg till kontroller (controllers) i containern.
// Detta är nödvändigt för att API:et ska kunna hantera inkommande HTTP-anrop.
builder.Services.AddControllers();

// Lägg till swagger/openapi (endast för utveckling)
// Detta ger dig en interaktiv API-dokumentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfigurera HTTP-pipeline för utvecklingsmiljön.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Använd CORS-policyn.
// Denna rad måste komma före `app.UseAuthorization();` och andra middleware som kan påverka CORS.
app.UseCors(MyAllowSpecificOrigins);

// Använder HTTPS-omdirigering.
app.UseHttpsRedirection();

// Konfigurera API-routerna.
app.MapControllers();

// Kör applikationen.
app.Run();