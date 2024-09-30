using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GRPC_Server.Services;

public class FileTransportService(IWebHostEnvironment env) : FileService.FileServiceBase
{
    readonly IWebHostEnvironment _env = env; //wwwroot dizinine erişmemizi sağlar

    public override async Task<Empty> FileUpload(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
    {
        // Streamin yapılacağı dizinin belirlenmesi
        string path = Path.Combine(_env.WebRootPath, "files");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        FileStream fs = null;
        try
        {
            int count = 0;
            decimal chunkSize = 0;//yüzde kaç yüklendiğini göstermek için bir progressbar oluşturuyoruz
            while (await requestStream.MoveNext())
            {
                if (count++ == 0)
                {
                    fs = new FileStream($"{path}/{requestStream.Current.Info.Filename}{requestStream.Current.Info.FileExtension}", FileMode.Create);
                    fs.SetLength(requestStream.Current.FileSize);
                }

                var buffer = requestStream.Current.Buffer.ToByteArray();
                await fs.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine($"{Math.Round((chunkSize += requestStream.Current.ReadedByte) * 100 / requestStream.Current.FileSize)} %");
            }
 

        }
        catch (Exception)
        {
        }
        await fs.DisposeAsync();
        fs.Close();
        return new Empty();
    }


    public override async Task FileDownload(FileInfo request, IServerStreamWriter<BytesContent> responseStream, ServerCallContext context)
    {
        string path = Path.Combine(_env.WebRootPath, "files");

        using FileStream fs = new($"{path}/{request.Filename}{request.FileExtension}", FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[2048];
        BytesContent content = new()
        {
           FileSize = fs.Length,
           Info = new FileInfo{ Filename = Path.GetFileNameWithoutExtension(fs.Name), FileExtension = Path.GetExtension(fs.Name) },
           ReadedByte = 0 
        };
        while ((content.ReadedByte = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            content.Buffer = ByteString.CopyFrom(buffer);
            responseStream.WriteAsync(content);
        }
        fs.Close(); 
    }
}