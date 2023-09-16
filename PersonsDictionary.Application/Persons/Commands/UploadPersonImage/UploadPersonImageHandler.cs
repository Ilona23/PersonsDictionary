using MediatR;
using Application.Constants;
using Application.Services;
using Domain.Abstractions;
using Domain.Exceptions;
using System.Net;

namespace Application.Persons.Commands.UpdatePersonImage;

public class UploadPersonImageHandler : IRequestHandler<UploadPersonImageCommand, Unit>
{
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public UploadPersonImageHandler(IPersonRepository repository, IResourceManagerService resourceManagerService)
    {
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<Unit> Handle(UploadPersonImageCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetAsync(request.Id);

        if (person is null)
        {
            throw new BadRequestException($"Unable to upload image, person not found by Id: {request.Id}", HttpStatusCode.NotFound);
        }

        if (request.File == null || request.File.Length == 0)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.NoFileIsSelected);
            throw new BadRequestException(message, HttpStatusCode.BadRequest);
        }

        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(request.File.FileName);
        if (!allowedExtensions.Contains(extension.ToLower()))
        {
            throw new ArgumentException(_resourceManagerService.GetString(ValidationMessages.InvalidFileType));
        }

        var maxFileSizeInBytes = 2097152;
        if (request.File.Length > maxFileSizeInBytes)
        {
            throw new ArgumentException(_resourceManagerService.GetString(ValidationMessages.FileSizeIsTooLarge));
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string fileName = $"{person.FirstName}_{person.LastName}_{person.Id}.jpg";
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

        // Delete existing file if it exists
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using FileStream stream = new FileStream(path, FileMode.Create);
        await request.File.CopyToAsync(stream);

        return new Unit();
    }
}