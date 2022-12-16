using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Enegen.Services;

public class HmacService : IHmacService
{
    public string CreateSignature(string uri, string body, string appId, long timeStamp, string nonce)
    {
        var encodedUri = WebUtility.UrlEncode(uri.ToLower());
        var bodyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));

        return $"{appId}{HttpMethod.Post}{encodedUri}{timeStamp}{nonce}{bodyBase64}";
    }
    
    public string GenerateHash(string signature, string privateSharedKey)
    {
        var privateSharedKeyBytes = Encoding.UTF8.GetBytes(privateSharedKey);
        var signatureBytes = Encoding.UTF8.GetBytes(signature);

        using var hash = new HMACSHA256(privateSharedKeyBytes);
        
        return Convert.ToBase64String(hash.ComputeHash(signatureBytes));
    }
}