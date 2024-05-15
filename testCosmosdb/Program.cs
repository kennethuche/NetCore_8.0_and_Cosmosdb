using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Azure.Cosmos;
using Serilog;
using Serilog.Events;
using System.Collections;
using System.Net;
using testCosmosdb.Data.Abstract;
using testCosmosdb.Data.Inteface;
using testCosmosdb.Mapper;
using testCosmosdb.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var configuration = builder.Configuration;




// Add CosmosClient and configure logging
builder.Services.AddSingleton((provider) =>
{
    var endpointUri = configuration["CosmosDbSettings:EndpointUri"];
    var primaryKey = configuration["CosmosDbSettings:PrimaryKey"];
    var databaseName = configuration["CosmosDbSettings:DatabaseName"];

    var cosmosClientOptions = new CosmosClientOptions
    {
        ApplicationName = databaseName,
        ConnectionMode = ConnectionMode.Gateway,

        ServerCertificateCustomValidationCallback = (request, certificate, chain) =>
        {
            // Always return true to ignore certificate validation errors
            return true; //not for production
        }
    };

  


    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);


    return cosmosClient;
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
// In production, modify this with the actual domains
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();





Log.Information("Starting the application...");



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Enable Serilog exception logging
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    });
});
app.UseHttpsRedirection();

app.UseCors("default");
app.UseAuthorization();

app.MapControllers();

app.Run();
Log.Information("Application started successfully.");

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid(); // Ensure mapping configuration is valid
mapper.ConfigurationProvider.CompileMappings();