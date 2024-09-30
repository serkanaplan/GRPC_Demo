using Grpc.Net.Client;
using GRPC_Client;
using Grpc.Core;
using Google.Protobuf;

var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Serkan Kaplan" });
Console.WriteLine("Greeting: " + reply.Message);



// Unary RPC
var client1 = new User.UserClient(GrpcChannel.ForAddress("http://localhost:5000"));

var response = await client1.GetUserAsync(new UserRequest { UserId = 1 });
Console.WriteLine($"Name: {response.Name}, Email: {response.Email}");



// Server streaming RPC
var client2 = new Chat.ChatClient(GrpcChannel.ForAddress("http://localhost:5000"));

using var call = client2.GetMessages(new ChatRequest { UserId = "user1" });

await foreach (var message in call.ResponseStream.ReadAllAsync())
    Console.WriteLine($"[{message.Timestamp}] {message.UserId}: {message.Message}");

//  while (await call.ResponseStream.MoveNext())
//     Console.WriteLine($"[{call.ResponseStream.Current.Timestamp}] {call.ResponseStream.Current.UserId}: {call.ResponseStream.Current.Message}");



// Client streaming RPC
var client3 = new Upload.UploadClient(GrpcChannel.ForAddress("http://localhost:5000"));

using var call2 = client3.UploadFiles();
foreach (var chunk in GetFileChunks("example.txt")) await call2.RequestStream.WriteAsync(chunk);

await call2.RequestStream.CompleteAsync();

var response2 = await call2;
Console.WriteLine($"Upload Status: {response2.Success}, Message: {response2.Message}");

// Local dosya chunk'ları oluşturmak için bir örnek metot
IEnumerable<FileChunk> GetFileChunks(string fileName)
{
    var fileContent = File.ReadAllBytes(fileName);
    int chunkSize = 1024; // 1KB
    for (int i = 0; i < fileContent.Length; i += chunkSize)
    {
        yield return new FileChunk
        {
            FileName = fileName,
            Content = ByteString.CopyFrom(fileContent.Skip(i).Take(chunkSize).ToArray())
        };
    }
}




// Bidirectional streaming RPC
var channel4 = GrpcChannel.ForAddress("http://localhost:5000");
var client4 = new Chats.ChatsClient(channel4);

using var call3 = client4.Chats();

// Client'tan mesajları gönderme
var send = Task.Run(async () =>
{
    string[] messages = ["Hello!", "What's up?", "Bye!"];
    foreach (var msg in messages) await call3.RequestStream.WriteAsync(new ChatsMessage { UserId = "user1", Message = msg });
    await call3.RequestStream.CompleteAsync();
});

// Server'dan gelen mesajları okuma
var read = Task.Run(async () =>
{
    await foreach (var response in call3.ResponseStream.ReadAllAsync()) Console.WriteLine($"Received: {response.Message}");
});

await Task.WhenAll(send, read);




// CRUD Operations For Product
var client5 = new Product.ProductClient(GrpcChannel.ForAddress("http://localhost:5000"));

// Create a product
var createResponse = await client5.CreateProductAsync(new CreateProductRequest
{
    Name = "Test Product",
    Price = 9.99
});
Console.WriteLine($"Created product: ID={createResponse.Id}, Name={createResponse.Name}, Price={createResponse.Price}");

// Get the product
var getResponse = await client5.GetProductAsync(new GetProductRequest { Id = createResponse.Id });
Console.WriteLine($"Retrieved product: ID={getResponse.Id}, Name={getResponse.Name}, Price={getResponse.Price}");

// Update the product
var updateResponse = await client5.UpdateProductAsync(new UpdateProductRequest
{
    Id = createResponse.Id,
    Name = "Updated Test Product",
    Price = 19.99
});
Console.WriteLine($"Updated product: ID={updateResponse.Id}, Name={updateResponse.Name}, Price={updateResponse.Price}");

// List all products
var listResponse = await client5.ListProductsAsync(new ListProductsRequest());
Console.WriteLine("All products:");
foreach (var product in listResponse.Products)
{
    Console.WriteLine($"ID={product.Id}, Name={product.Name}, Price={product.Price}");
}

// Delete the product
var deleteResponse = await client5.DeleteProductAsync(new DeleteProductRequest { Id = createResponse.Id });
Console.WriteLine($"Product deleted: {deleteResponse.Success}");