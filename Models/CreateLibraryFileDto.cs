using System.ComponentModel.DataAnnotations;

namespace Minio_Demo.Models;

public class CreateLibraryFileDto
{
    [Required]
    [MinLength(3)]
    public string NameAr { get; set; }

    public string NameEn { get; set; }

    [Required]
    public IFormFile File { get; set; }
}