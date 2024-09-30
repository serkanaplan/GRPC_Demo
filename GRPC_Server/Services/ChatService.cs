using Grpc.Core;

namespace GRPC_Server.Services;

public class ChatService : Chat.ChatBase
{
    public override async Task GetMessages(ChatRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
    {
        // Örnek mesajları stream ile gönderiyoruz.
        List<ChatMessage> messages =
        [
            new ChatMessage { UserId = request.UserId, Message = "Hello!", Timestamp = DateTime.UtcNow.ToString() },
            new ChatMessage { UserId = request.UserId, Message = "How are you?", Timestamp = DateTime.UtcNow.ToString() },
            new ChatMessage { UserId = request.UserId, Message = "I'm fine, thanks!", Timestamp = DateTime.UtcNow.ToString() },
            new ChatMessage { UserId = request.UserId, Message = "Bye!", Timestamp = DateTime.UtcNow.ToString() },
            new ChatMessage { UserId = request.UserId, Message = "Goodbye!", Timestamp = DateTime.UtcNow.ToString() }
        ];

        foreach (var message in messages)
        {
            await responseStream.WriteAsync(message);
            await Task.Delay(1000); // 1 saniye bekleme (simülasyon amaçlı)
        }
    }
}
