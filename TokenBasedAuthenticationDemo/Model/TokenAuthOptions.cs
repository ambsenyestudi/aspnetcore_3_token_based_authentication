namespace TokenBasedAuthenticationDemo.Model
{
    public class TokenAuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int MinutesOfLife { get; set; }
    }
}
