using System.Runtime.CompilerServices;
using Eryph.IdentityModel.Clients;

namespace Eryph.ComputeClient
{
    public partial class EryphComputeClientOptions
    {
        public ClientCredentials ClientCredentials { get; }

        public EryphComputeClientOptions(ClientCredentials clientCredentials, ServiceVersion version = LatestVersion)
            : this(version)
        {
            ClientCredentials = clientCredentials;
        }
    }
}