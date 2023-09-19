using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Application;
using Application.Services;
using Persistence;
using System.Resources;
using Domain.Abstractions;
using FluentValidation;
using Application.PipelineBehaviors;
using FluentValidation.AspNetCore;
using Application.Abstractions.Messaging;
using Web.Middlewares;
using System.Text.Json.Serialization;
using Serilog;
using Domain.Entities;
using Application.Mapping;
using Application.Models;

var builder = WebApplication.CreateBuilder(args);
var applicationAssembly = typeof(AssemblyReference).Assembly;

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidationActionFilter));
})
.AddApplicationPart(applicationAssembly)
.AddDataAnnotationsLocalization()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(AssemblyReference));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddScoped<ValidationActionFilter>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDtoToEntityMapper, DtoToEntityMapper>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IMapper<Person, PersonModel>, PersonMapper>();
builder.Services.AddScoped<IMapper<Person, PersonDetailedModel>, PersonDetailedMapper>();
builder.Services.AddScoped<IMapper<PersonRelation, RelatedPersonsModel>, RelatedPersonsMapper>();
builder.Services.AddSingleton<IResourceManagerService>(provider =>
{
    var assembly = applicationAssembly;
    var resourceManager = new ResourceManager("Application.Resources.SharedResource", assembly);
    return new ResourceManagerService(resourceManager);
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

var app = builder.Build();

var dbInitializer = new DbInitializer();
await dbInitializer.Seed(app.Services, CancellationToken.None);

app.UseMiddleware<AcceptLanguageMiddleware>();
app.UseMiddleware<ErrorLoggingMiddleware>();

var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json")
   .Build();

var logFilePath = configuration["Logging:File:Path"] ?? "log.txt";
Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(configuration)
    .WriteTo.File(logFilePath)
    .CreateLogger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
