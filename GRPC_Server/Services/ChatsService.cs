using Grpc.Core;

namespace GRPC_Server.Services;
public class ChatsService : Chats.ChatsBase
{
    public override async Task Chats(IAsyncStreamReader<ChatsMessage> requestStream, IServerStreamWriter<ChatsResponse> responseStream, ServerCallContext context)
    {
        await foreach (var chatsMessage in requestStream.ReadAllAsync())
        {
            // Client'tan gelen mesajı işleyip yanıt olarak client'a gönderiyoruz.
            Console.WriteLine($"{chatsMessage.UserId}: {chatsMessage.Message}");
            await responseStream.WriteAsync(new ChatsResponse
            {
                UserId = chatsMessage.UserId,
                Message = $"Received: {chatsMessage.Message}"
            });
        }
    }
}
