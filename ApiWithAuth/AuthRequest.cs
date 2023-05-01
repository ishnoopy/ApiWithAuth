namespace ApiWithAuth
{
    public class AuthRequest
    {
        // DOCU: null! - for now it will be null but it won't be null in runtime.
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
