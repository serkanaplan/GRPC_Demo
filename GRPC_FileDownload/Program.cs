using Grpc.Core;
using Grpc.Net.Client;
using GRPC_FileDownload;

var client = new FileService.FileServiceClient(GrpcChannel.ForAddress("http://localhost:5000"));

string downloadPath = "C:\\Users\\serkan\\Desktop\\GRPC_Demo\\GRPC_FileDownload\\Files";

var fileInfo = new GRPC_FileDownload.FileInfo
{
    FileExtension = ".mp4",
    Filename = "deneme"
};

FileStream fs = null;

var request = client.FileDownload(fileInfo);

int count = 0;
decimal chunkSize = 0;
while (await request.ResponseStream.MoveNext())
{
    if (count++ == 0)
    {
        fs = new FileStream($"{downloadPath}\\{request.ResponseStream.Current.Info.Filename}{request.ResponseStream.Current.Info.FileExtension}", FileMode.CreateNew);
        fs.SetLength(request.ResponseStream.Current.FileSize);
    }
    var buffer = request.ResponseStream.Current.Buffer.ToByteArray();
    await fs.WriteAsync(buffer, 0, request.ResponseStream.Current.ReadedByte);
    Console.WriteLine($"{Math.Round((chunkSize += request.ResponseStream.Current.ReadedByte) * 100 / request.ResponseStream.Current.FileSize)} %");
}

Console.WriteLine("Download completed.");
await fs.DisposeAsync();
