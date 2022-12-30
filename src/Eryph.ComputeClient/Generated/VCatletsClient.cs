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
    /// <summary> The VCatlets service client. </summary>
    public partial class VCatletsClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal VCatletsRestClient RestClient { get; }

        /// <summary> Initializes a new instance of VCatletsClient for mocking. </summary>
        protected VCatletsClient()
        {
        }

        /// <summary> Initializes a new instance of VCatletsClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal VCatletsClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new VCatletsRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Get a virtual catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<VirtualCatlet>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VCatletsClient.Get");
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

        /// <summary> Get a virtual catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<VirtualCatlet> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VCatletsClient.Get");
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

        /// <summary> Get virtual catlet configuration. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration of a virtual catlet. </remarks>
        public virtual async Task<Response<VirtualCatletConfiguration>> GetConfigAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VCatletsClient.GetConfig");
            scope.Start();
            try
            {
                return await RestClient.GetConfigAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get virtual catlet configuration. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration of a virtual catlet. </remarks>
        public virtual Response<VirtualCatletConfiguration> GetConfig(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("VCatletsClient.GetConfig");
            scope.Start();
            try
            {
                return RestClient.GetConfig(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get list of virtual catlets. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<VirtualCatlet> ListAsync(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            async Task<Page<VirtualCatlet>> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("VCatletsClient.List");
                scope.Start();
                try
                {
                    var response = await RestClient.ListAsync(count, project, cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            async Task<Page<VirtualCatlet>> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("VCatletsClient.List");
                scope.Start();
                try
                {
                    var response = await RestClient.ListNextPageAsync(nextLink, count, project, cancellationToken).ConfigureAwait(false);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateAsyncEnumerable(FirstPageFunc, NextPageFunc);
        }

        /// <summary> Get list of virtual catlets. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<VirtualCatlet> List(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            Page<VirtualCatlet> FirstPageFunc(int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("VCatletsClient.List");
                scope.Start();
                try
                {
                    var response = RestClient.List(count, project, cancellationToken);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            Page<VirtualCatlet> NextPageFunc(string nextLink, int? pageSizeHint)
            {
                using var scope = _clientDiagnostics.CreateScope("VCatletsClient.List");
                scope.Start();
                try
                {
                    var response = RestClient.ListNextPage(nextLink, count, project, cancellationToken);
                    return Page.FromValues(response.Value.Value, response.Value.NextLink, response.GetRawResponse());
                }
                catch (Exception e)
                {
                    scope.Failed(e);
                    throw;
                }
            }
            return PageableHelpers.CreateEnumerable(FirstPageFunc, NextPageFunc);
        }
    }
}
