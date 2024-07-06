namespace Identity.Api.Common.Configuration
{
    public class ClientsOptions
    {
        public const string Clients = "Clients";

        public IEnumerable<Client> ClientsArr { get; set; } = null!;

        public class Client
        {
            public string ClientId { get; set; } = null!;
            public string ClientSecret { get; set; } = null!;
            public string DisplayName { get; set; } = null!;
            public Uri[] RedirectUris { get; set; } = null!;
            public string[] Permissions { get; set; } = null!;
        }
    }
}