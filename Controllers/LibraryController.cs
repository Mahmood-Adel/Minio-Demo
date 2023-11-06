using Microsoft.AspNetCore.Mvc;
using Minio_Demo.Constants;
using Minio_Demo.Data;
using Minio_Demo.Models;
using Minio_Demo.Persistence;
using Minio_Demo.Services.Abstractions;

namespace Minio_Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController: Controller
{
    private readonly IFileManager _fileManager;
    private readonly AppDbContext _dbContext;

    public LibraryController(IFileManager fileManager, AppDbContext dbContext)
    {
        _fileManager = fileManager;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateLibraryFileDto fileDto)
    {
       var item = new LibraryFile
       {
           NameAr = fileDto.NameAr.Trim(),
           NameEn = fileDto.NameEn?.Trim() ?? fileDto.NameAr.Trim(),
       };
        _dbContext.LibraryFiles.Add(item);
        
        //upload file to minio
        var fileName = Guid.NewGuid().ToString();
        var contentType = fileDto.File.ContentType;
        await using var ms = fileDto.File.OpenReadStream();
        ms.Seek(0, SeekOrigin.Begin);


        var result = await _fileManager.PutAsync(ms, Directories.Library, fileName, contentType);
        if (!result.Succeeded)
        {
            return BadRequest(result.ErrorMessage);
        }

        item.FileName = fileName;
        item.ContentType = contentType;
        item.FileSize = fileDto.File.Length;
        await _dbContext.SaveChangesAsync();
        return Ok(item);
    }
    
    [HttpGet("download/{id:guid}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var libraryFile = _dbContext.LibraryFiles
            .Where(x => x.Id.Equals(id))
            .Select(x => new
            {
                x.FileName,
                x.ContentType,
            })
            .FirstOrDefault();

        if (libraryFile == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(libraryFile.FileName))
        {
            return BadRequest("the library item does not have a file");
        }

        var stream = await _fileManager.GetAsync($"{Directories.Library}/{libraryFile.FileName}");
        return new FileStreamResult(stream, libraryFile.ContentType);
    }

    [HttpGet("temp-url/{id:guid}")]
    public async Task<IActionResult> GetTempUrl(Guid id)
    {
        var libraryFile = _dbContext.LibraryFiles
            .Where(x => x.Id.Equals(id))
            .Select(x => new
            {
                x.FileName,
            })
            .FirstOrDefault();
        
        if (libraryFile == null)
        {
            return NotFound();
        }
        
        if (string.IsNullOrEmpty(libraryFile.FileName))
        {
            return BadRequest("the library item does not have a file");
        }

        var url = await _fileManager.GetTempUrl($"{Directories.Library}/{libraryFile.FileName}", 60);

        return Ok(url);
    }
}