namespace Equias.Messages
{
    public class RequestTokenRequest
    {
        public string Username { get; }
        public string Password { get; }


        public RequestTokenRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
