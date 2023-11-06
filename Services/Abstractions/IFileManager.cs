using Minio_Demo.Wrappers;

namespace Minio_Demo.Services.Abstractions;

public interface IFileManager
{
    public Task<FileManagerResult> PutAsync(Stream stream, string dir, string name, string contentType = null);

    public Task<FileManagerResult> DeleteAsync(string path);

    public Task<Stream> GetAsync(string path);
    public Task<string> GetTempUrl(string path, int expiry);
}