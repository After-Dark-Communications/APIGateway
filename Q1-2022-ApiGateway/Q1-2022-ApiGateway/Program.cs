

using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;




var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json", optional: true, reloadOnChange: true)
                            .Build();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(configuration);
builder.Services.AddOcelot(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("https://localhost:49372/swagger/index.html", "ApiGateway");
        c.SwaggerEndpoint("https://localhost:49370/swagger/index.html", "test");
    });

    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });

   


}
app.UseRouting();


app.UseHttpsRedirection();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    }
);

app.UseOcelot().Wait();

app.Run();
