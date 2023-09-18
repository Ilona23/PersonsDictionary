using Serilog;
using System.Reflection;
using Web.Middlewares;
using Application.Mapping;
using Application.Models;
using Application.PipelineBehaviors;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence;
using System.Resources;
using System.Text.Json.Serialization;
using Application.Abstractions.Messaging;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);
var presentationAssembly = typeof(AssemblyReference).Assembly;

ConfigureServices(builder.Services, presentationAssembly);
ConfigureDatabase(builder.Configuration, builder.Services);
ConfigureLogging(builder.Configuration);

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

void ConfigureServices(IServiceCollection services, Assembly presentationAssembly)
{
    services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ValidationActionFilter));
    })
    .AddApplicationPart(presentationAssembly)
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddHttpContextAccessor();
    services.AddMediatR(typeof(AssemblyReference));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddValidatorsFromAssembly(presentationAssembly);
    services.AddScoped<ValidationActionFilter>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IDtoToEntityMapper, DtoToEntityMapper>();
    services.AddScoped<IPersonRepository, PersonRepository>();
    services.AddScoped<IMapper<Person, PersonModel>, PersonMapper>();
    services.AddScoped<IMapper<Person, PersonDetailedModel>, PersonDetailedMapper>();
    services.AddScoped<IMapper<PersonRelation, RelatedPersonsModel>, RelatedPersonsMapper>();
    services.AddSingleton<IResourceManagerService>(provider =>
    {
        var assembly = presentationAssembly;
        var resourceManager = new ResourceManager("Application.Resources.SharedResource", assembly);
        return new ResourceManagerService(resourceManager);
    });

    services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
}

void ConfigureDatabase(IConfiguration configuration, IServiceCollection services)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
}

void ConfigureLogging(IConfiguration configuration)
{
    var logFilePath = configuration["Logging:File:Path"] ?? "log.txt";
    Log.Logger = new LoggerConfiguration()
        .ReadFrom
        .Configuration(configuration)
        .WriteTo.File(logFilePath)
        .CreateLogger();
}
