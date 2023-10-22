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
    /// <summary> The Catlets service client. </summary>
    public partial class CatletsClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal CatletsRestClient RestClient { get; }

        /// <summary> Initializes a new instance of CatletsClient for mocking. </summary>
        protected CatletsClient()
        {
        }

        /// <summary> Initializes a new instance of CatletsClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal CatletsClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new CatletsRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Creates a new catlet. </summary>
        /// <param name="body"> The NewCatletRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates a catlet. </remarks>
        public virtual async Task<Response<Models.Operation>> CreateAsync(NewCatletRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Create");
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

        /// <summary> Creates a new catlet. </summary>
        /// <param name="body"> The NewCatletRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates a catlet. </remarks>
        public virtual Response<Models.Operation> Create(NewCatletRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Create");
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

        /// <summary> Deletes a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Delete");
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

        /// <summary> Deletes a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Delete(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Delete");
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

        /// <summary> Get a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Catlet>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Get");
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

        /// <summary> Get a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Catlet> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Get");
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

        /// <summary> Get catlet configuration. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration of a catlet. </remarks>
        public virtual async Task<Response<CatletConfiguration>> GetConfigAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.GetConfig");
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

        /// <summary> Get catlet configuration. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Get the configuration of a catlet. </remarks>
        public virtual Response<CatletConfiguration> GetConfig(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.GetConfig");
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

        /// <summary> Starts a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> StartAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Start");
            scope.Start();
            try
            {
                return await RestClient.StartAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Starts a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Start(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Start");
            scope.Start();
            try
            {
                return RestClient.Start(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Stops a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> StopAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Stop");
            scope.Start();
            try
            {
                return await RestClient.StopAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Stops a catlet. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Stop(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Stop");
            scope.Start();
            try
            {
                return RestClient.Stop(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Updates a catlet. </summary>
        /// <param name="body"> The UpdateCatletRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> UpdateAsync(UpdateCatletRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Update");
            scope.Start();
            try
            {
                return await RestClient.UpdateAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Updates a catlet. </summary>
        /// <param name="body"> The UpdateCatletRequest to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Update(UpdateCatletRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Update");
            scope.Start();
            try
            {
                return RestClient.Update(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> List all catlets. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<Catlet> ListAsync(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, project);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, project);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, Catlet.DeserializeCatlet, _clientDiagnostics, _pipeline, "CatletsClient.List", "value", "nextLink", cancellationToken);
        }

        /// <summary> List all catlets. </summary>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<Catlet> List(bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, project);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, project);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, Catlet.DeserializeCatlet, _clientDiagnostics, _pipeline, "CatletsClient.List", "value", "nextLink", cancellationToken);
        }
    }
}
