# 🚀 gRPC İletişim Türleri ve Kullanım Senaryoları

Bu doküman, gRPC iletişim türleri ve kullanım senaryolarını detaylandırır. 👇 Aşağıdaki tabloda çeşitli gRPC türlerinin nasıl çalıştığı ve hangi durumlarda kullanılabileceği gösterilmiştir.

---

<table>
  <tr>
    <th align="center" width="350px">💡 Tür</th>
    <th align="center" width="150px">🔄 İstek Sayısı</th>
    <th align="center" width="150px">💬 Yanıt Sayısı</th>
    <th align="center" width="350px">🔗 İletişim Yönü</th>
    <th align="center" width="300px">📚 Kullanım Senaryosu</th>
  </tr>
  <tr>
    <td align="center"><strong style="color: #3b82f6;">Unary RPC</strong></td>
    <td align="center">1</td>
    <td align="center">1</td>
    <td align="center">Tek yönlü (Client → Server)</td>
    <td align="center">Tek yanıt gerektiren işlemler</td>
  </tr>
  <tr>
    <td align="center"><strong style="color: #ec4899;">Server Streaming RPC</strong></td>
    <td align="center">1</td>
    <td align="center">Çoklu</td>
    <td align="center">Tek yönlü (Server → Client)</td>
    <td align="center">Sürekli veri yayını</td>
  </tr>
  <tr>
    <td align="center"><strong style="color: #10b981;">Client Streaming RPC</strong></td>
    <td align="center">Çoklu</td>
    <td align="center">1</td>
    <td align="center">Tek yönlü (Client → Server)</td>
    <td align="center">Toplu veri gönderimi</td>
  </tr>
  <tr>
    <td align="center"><strong style="color: #f97316;">Bidirectional Streaming RPC</strong></td>
    <td align="center">Çoklu</td>
    <td align="center">Çoklu</td>
    <td align="center">Çift yönlü (Client ↔ Server)</td>
    <td align="center">Gerçek zamanlı iletişim</td>
  </tr>
</table>

---

## 🎨 Ekstra Bilgilendirme
1. **Unary RPC** genellikle basit ve hızlı işlemler için kullanılır. Örneğin, kimlik doğrulama isteği gönderme ve yanıt alma.
2. **Server Streaming RPC** belirli bir süre boyunca verilerin sürekli gönderildiği senaryolarda idealdir.
3. **Client Streaming RPC** verilerin toplu olarak gönderilmesi gerektiğinde kullanılır.
4. **Bidirectional Streaming RPC** ise gerçek zamanlı veri alışverişi gereken uygulamalarda tercih edilir. Örneğin, anlık mesajlaşma uygulamaları.

## 🌟 Kullanım Alanları
- 💻 **Microservices Architecture:** gRPC, mikroservisler arasında hızlı ve güvenilir iletişim sağlar.
- 🔗 **Real-time Communication:** Gerçek zamanlı veri akışı için idealdir.
- 📡 **Efficient Data Transfer:** Büyük boyutlu verilerin düşük gecikme ile aktarımını sağlar.

> 💬 **Not:** Daha fazla bilgi için [gRPC Dokümantasyonu](https://grpc.io/docs/) sayfasını ziyaret edebilirsiniz.
