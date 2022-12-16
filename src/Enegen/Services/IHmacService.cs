namespace Enegen.Services;

public interface IHmacService
{
    string CreateSignature(string uri, string body, string appId, long timeStamp, string nonce);
    string GenerateHash(string signature, string privateSharedKey);
}