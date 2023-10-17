using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Azure.Core;
using Azure.Core.Pipeline;
using Eryph.ClientRuntime.Authentication;

namespace Eryph.ComputeClient
{
    public class ComputeClientsFactory
    {
        private readonly EryphComputeClientOptions _options;
        private readonly Uri _endpoint;

        public ComputeClientsFactory(EryphComputeClientOptions options, Uri endpoint)
        {
            _options = options;
            _endpoint = endpoint;
        }

        public Client CreateOperationsClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);

        public ProjectsClient CreateProjectsClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);

        public VNetworksClient CreateVNetworksClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);


        public CatletsClient CreateCatletsClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);


        public VirtualDisksClient CreateVirtualDisksClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);


    }
}
