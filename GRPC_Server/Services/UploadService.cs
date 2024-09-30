using Grpc.Core;

namespace GRPC_Server.Services;

public class UploadService : Upload.UploadBase
{
    public override async Task<UploadStatus> UploadFiles(IAsyncStreamReader<FileChunk> requestStream, ServerCallContext context)
    {
        List<FileChunk> fileChunks = [];
        await foreach (var chunk in requestStream.ReadAllAsync())  fileChunks.Add(chunk);
        
        Console.WriteLine($"Received {fileChunks.Count} chunks.");
        // Örnek: Gelen tüm dosya chunk'ları alındıktan sonra başarılı yanıt döndürüyoruz.
        return new UploadStatus
        {
            Success = true,
            Message = $"Uploaded {fileChunks.Count} chunks successfully."
        };
    }
}
