// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient
{
    /// <summary> The VNetworks service client. </summary>
    public partial class VNetworksClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal VNetworksRestClient RestClient { get; }

        /// <summary> Initializes a new instance of VNetworksClient for mocking. </summary>
        protected VNetworksClient()
        {
        }

        /// <summary> Initializes a new instance of VNetworksClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal VNetworksClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new VNetworksRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Get a virtual network. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<VirtualNetwork>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.Get");
            scope.Start();
            try
            {
                return await RestClient.GetAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get a virtual network. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<VirtualNetwork> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.Get");
            scope.Start();
            try
            {
                return RestClient.Get(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get project virtual networks configuration. </summary>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration for all networks in a project. </remarks>
        public virtual async Task<Response<VirtualNetworkConfiguration>> GetConfigAsync(string project, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.GetConfig");
            scope.Start();
            try
            {
                return await RestClient.GetConfigAsync(project, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get project virtual networks configuration. </summary>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration for all networks in a project. </remarks>
        public virtual Response<VirtualNetworkConfiguration> GetConfig(string project, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.GetConfig");
            scope.Start();
            try
            {
                return RestClient.GetConfig(project, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Creates or updates virtual networks of project. </summary>
        /// <param name="body"> The UpdateProjectNetworksRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates or updates virtual networks. </remarks>
        public virtual async Task<Response<Models.Operation>> CreateAsync(UpdateProjectNetworksRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.Create");
            scope.Start();
            try
            {
                return await RestClient.CreateAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Creates or updates virtual networks of project. </summary>
        /// <param name="body"> The UpdateProjectNetworksRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates or updates virtual networks. </remarks>
        public virtual Response<Models.Operation> Create(UpdateProjectNetworksRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VNetworksClient.Create");
            scope.Start();
            try
            {
                return RestClient.Create(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get list of virtual networks. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<VirtualNetwork> ListAsync(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, project);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, project);
            return PageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, VirtualNetwork.DeserializeVirtualNetwork, _clientDiagnostics, _pipeline, "VNetworksClient.List", "value", "nextLink", cancellationToken);
        }

        /// <summary> Get list of virtual networks. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<VirtualNetwork> List(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, project);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, project);
            return PageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, VirtualNetwork.DeserializeVirtualNetwork, _clientDiagnostics, _pipeline, "VNetworksClient.List", "value", "nextLink", cancellationToken);
        }

        /// <summary> Get list of virtual networks in a project. </summary>
        /// <param name="project"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="project"/> is null. </exception>
        /// <remarks> Get list of virtual networks in project. </remarks>
        public virtual AsyncPageable<VirtualNetwork> ListProjectAsync(string project, bool? count = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(project, nameof(project));

            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListProjectRequest(project, count);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListProjectNextPageRequest(nextLink, project, count);
            return PageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, VirtualNetwork.DeserializeVirtualNetwork, _clientDiagnostics, _pipeline, "VNetworksClient.ListProject", "value", "nextLink", cancellationToken);
        }

        /// <summary> Get list of virtual networks in a project. </summary>
        /// <param name="project"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="project"/> is null. </exception>
        /// <remarks> Get list of virtual networks in project. </remarks>
        public virtual Pageable<VirtualNetwork> ListProject(string project, bool? count = null, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(project, nameof(project));

            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListProjectRequest(project, count);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListProjectNextPageRequest(nextLink, project, count);
            return PageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, VirtualNetwork.DeserializeVirtualNetwork, _clientDiagnostics, _pipeline, "VNetworksClient.ListProject", "value", "nextLink", cancellationToken);
        }
    }
}
