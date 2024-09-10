// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Autorest.CSharp.Core;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient
{
    /// <summary> The Genes service client. </summary>
    public partial class GenesClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal GenesRestClient RestClient { get; }

        /// <summary> Initializes a new instance of GenesClient for mocking. </summary>
        protected GenesClient()
        {
        }

        /// <summary> Initializes a new instance of GenesClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal GenesClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new GenesRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Removes unused genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Removes unused genes from the local gene pool. </remarks>
        public virtual async Task<Response<Models.Operation>> CleanupAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Cleanup");
            scope.Start();
            try
            {
                return await RestClient.CleanupAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Removes unused genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Removes unused genes from the local gene pool. </remarks>
        public virtual Response<Models.Operation> Cleanup(CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Cleanup");
            scope.Start();
            try
            {
                return RestClient.Cleanup(cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Removes a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Removes a gene from the local gene pool. </remarks>
        public virtual async Task<Response<Models.Operation>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Delete");
            scope.Start();
            try
            {
                return await RestClient.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Removes a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Removes a gene from the local gene pool. </remarks>
        public virtual Response<Models.Operation> Delete(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Delete");
            scope.Start();
            try
            {
                return RestClient.Delete(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<GeneWithUsage>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Get");
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

        /// <summary> Get a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<GeneWithUsage> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("GenesClient.Get");
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

        /// <summary> List all genes. </summary>
        /// <param name="count"> The <see cref="bool"/>? to use. </param>
        /// <param name="projectId"> The <see cref="Guid"/>? to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<Gene> ListAsync(bool? count = null, Guid? projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, projectId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, projectId);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, Gene.DeserializeGene, _clientDiagnostics, _pipeline, "GenesClient.List", "value", "nextLink", cancellationToken);
        }

        /// <summary> List all genes. </summary>
        /// <param name="count"> The <see cref="bool"/>? to use. </param>
        /// <param name="projectId"> The <see cref="Guid"/>? to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<Gene> List(bool? count = null, Guid? projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, projectId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, projectId);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, Gene.DeserializeGene, _clientDiagnostics, _pipeline, "GenesClient.List", "value", "nextLink", cancellationToken);
        }
    }
}