using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;
using RestaurantGLB_Webserver.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("NapasClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        string pfxPwd = builder.Configuration["NAPAS:pfxPwd"];
        string pfxPath = builder.Configuration["NAPAS:pfxPath"];

        var handler = new HttpClientHandler();
        var cert = new X509Certificate2(pfxPath, pfxPwd,
            X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.PersistKeySet |
            X509KeyStorageFlags.Exportable);
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        handler.ClientCertificates.Add(cert);
        return handler;
    });
builder.Services.AddDbContext<UnionPOSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
