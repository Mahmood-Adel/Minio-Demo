using Microsoft.Extensions.Options;
using Minio;
using Minio_Demo.Services.Abstractions;
using Minio_Demo.Settings;
using Minio_Demo.Wrappers;
using Minio.DataModel.Args;

namespace Minio_Demo.Services.Implementations;

public class MinioFileManager: IFileManager
{
   private readonly IMinioClient _client;
    private readonly string _bucketName;

    public MinioFileManager(IOptions<MinioSettings> settings)
    {
        var minio = settings.Value;
        _bucketName = minio.BucketName;
        _client = new MinioClient()
            .WithEndpoint(minio.Url)
            .WithCredentials(minio.AccessKey, minio.SecretKey)
            .Build();
    }

    public async Task<FileManagerResult> PutAsync(Stream stream, string dir, string name, string contentType = null)
    {
        try
        {
            var args = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject($"{dir}/{name}")
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _client.PutObjectAsync(args);
            return new FileManagerResult {Succeeded = true};
        }
        catch (Exception e)
        {
            return new FileManagerResult {Succeeded = false, ErrorMessage = e.Message};
        }
    }

    public async Task<FileManagerResult> DeleteAsync(string path)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(path);

            await _client.RemoveObjectAsync(args);
            return new FileManagerResult {Succeeded = true};
        }
        catch (Exception e)
        {
            return new FileManagerResult {Succeeded = false, ErrorMessage = e.Message};
        }
    }

    public async Task<Stream> GetAsync(string path)
    {
        var taskCompletionSource = new TaskCompletionSource<Stream>();

        var args = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(path)
            .WithCallbackStream(stream =>
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                taskCompletionSource.SetResult(ms);
            });

        await _client.GetObjectAsync(args);
        return await taskCompletionSource.Task;
    }
    
    public async Task<string> GetTempUrl(string path, int expiry)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(path)
            .WithExpiry(expiry);
        
        return await _client.PresignedGetObjectAsync(args);
    }
}