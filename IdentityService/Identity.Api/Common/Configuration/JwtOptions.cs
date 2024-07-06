namespace Identity.Api.Common.Configuration
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public IEnumerable<string> ValidIssuers { get; set; } = null!;
        public IEnumerable<string> ValidAudiences { get; set; } = null!;
    }
}