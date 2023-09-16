using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Persons.Commands.UpdatePersonImage;

public class UploadPersonImageCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }
    public IFormFile File { get; set; }
}