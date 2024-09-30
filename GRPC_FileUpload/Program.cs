using Google.Protobuf;
using Grpc.Net.Client;
using GRPC_FileUpload;

var client = new FileService.FileServiceClient(GrpcChannel.ForAddress("http://localhost:5000"));

string file = "deneme.mp4";
using FileStream fs = new(file, FileMode.Open);

var content = new BytesContent
{
    FileSize = fs.Length,
    ReadedByte = 0,
    Info = new GRPC_FileUpload.FileInfo { Filename = Path.GetFileNameWithoutExtension(fs.Name), FileExtension = Path.GetExtension(fs.Name) },

};

var upload = client.FileUpload();
byte[] buffer = new byte[2048];

while ((content.ReadedByte =await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
{
   content.Buffer = ByteString.CopyFrom(buffer);
   await upload.RequestStream.WriteAsync(content);
}

await upload.RequestStream.CompleteAsync();
fs.Close();