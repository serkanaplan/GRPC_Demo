
using GRPC_Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();
app.UseStaticFiles();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<UserService>();
app.MapGrpcService<ChatService>();
app.MapGrpcService<UploadService>();
app.MapGrpcService<ChatsService>();
app.MapGrpcService<ProductService>();
app.MapGrpcService<FileTransportService>();
app.MapGet("/", () => "Hello World!");

app.Run();
