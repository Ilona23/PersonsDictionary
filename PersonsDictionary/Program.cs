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
using System.Reflection;
using Application.Abstractions.Messaging;
using Web.Middlewares;
using System.Text.Json.Serialization;
using Domain.Mapping;

var builder = WebApplication.CreateBuilder(args);
var presentationAssembly = typeof(AssemblyReference).Assembly;

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidationActionFilter));
})
.AddApplicationPart(presentationAssembly)
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
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // to run every time it's requested
builder.Services.AddValidatorsFromAssembly(presentationAssembly);
builder.Services.AddScoped<ValidationActionFilter>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDTOToEntityMapper, DTOToEntityMapper>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddSingleton<IResourceManagerService>(provider =>
{
    var assembly = presentationAssembly;
    var resourceManager = new ResourceManager("Application.Resources.SharedResource", assembly);
    return new ResourceManagerService(resourceManager);
});

builder.Services.AddFluentValidation(x =>
{
    x.ImplicitlyValidateChildProperties = true;
    x.RegisterValidatorsFromAssemblies(new Assembly[]
    {
        typeof(ICommand<>).Assembly
    });
});

var app = builder.Build();

var dbInitializer = new DbInitializer();
await new DbInitializer().Seed(app.Services, CancellationToken.None);

app.UseErrorHandlingMiddleware();
app.UseMiddleware<AcceptLanguageMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
