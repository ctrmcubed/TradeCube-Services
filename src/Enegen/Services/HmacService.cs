using System.Security.Cryptography;
using System.Text;

namespace Enegen.Services;

public class HmacService
{
    public string GenerateHash(string signature, string privateSharedKey)
    {
        var privateSharedKeyBytes = Encoding.UTF8.GetBytes(privateSharedKey);
        var signatureBytes = Encoding.UTF8.GetBytes(signature);

        using var hash = new HMACSHA256(privateSharedKeyBytes);
        
        return Convert.ToBase64String(hash.ComputeHash(signatureBytes));
    }
}