using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Other service configurations...

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        // Other service configurations...
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Other app configurations...

        // Use CORS
        app.UseCors("AllowAll");

        // Other app configurations...
    }
}



// Configure the HTTP request pipeline.


