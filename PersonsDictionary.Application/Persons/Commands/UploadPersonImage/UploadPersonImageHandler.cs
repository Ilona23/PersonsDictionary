using MediatR;
using Application.Constants;
using Application.Services;
using Domain.Abstractions;
using Domain.Exceptions;
using System.Net;

namespace Application.Persons.Commands.UpdatePersonImage
{
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
            if (request.File == null || request.File.Length == 0)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.NoFileIsSelected);
                throw new BadRequestException(message, HttpStatusCode.BadRequest);
            }

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(request.File.FileName);

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new InvalidOperationException(_resourceManagerService.GetString(ValidationMessages.InvalidFileType));
            }

            var maxFileSizeInBytes = 2097152; // 2 MB
            if (request.File.Length > maxFileSizeInBytes)
            {
                throw new InvalidOperationException(_resourceManagerService.GetString(ValidationMessages.FileSizeIsTooLarge));
            }

            var person = await _repository.GetPersonByIdAsync(request.Id, cancellationToken);

            if (person is null)
            {
                throw new BadRequestException($"Unable to upload image, person not found by Id: {request.Id}", HttpStatusCode.NotFound);
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string fileName = $"{person.FirstName}_{person.LastName}_{person.Id}.jpg";
            string path = Path.Combine(filePath, fileName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using FileStream stream = new(path, FileMode.Create);
            await request.File.CopyToAsync(stream, cancellationToken);

            return new Unit();
        }
    }
}
