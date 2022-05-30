

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
                            .Build();



builder.Host.ConfigureAppConfiguration(app => app.AddJsonFile("configuration.json"));
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration, (o) =>
{
    o.GenerateDocsForGatewayItSelf = true;
});
builder.Services.AddMvc();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddOcelot(configuration).AddConsul().AddConfigStoredInConsul();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "ApiGateway V1");
        c.RoutePrefix =string.Empty;
    }
    );
   

   


}

app.UseRouting();


app.UseHttpsRedirection();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    }
);
app.UseSwaggerForOcelotUI((opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
    

}));
app.UseOcelot().Wait();



app.Run();
