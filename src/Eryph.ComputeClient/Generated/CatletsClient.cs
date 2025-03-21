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

        /// <summary> Create a new catlet. </summary>
        /// <param name="body"> The <see cref="NewCatletRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Create a catlet. </remarks>
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

        /// <summary> Create a new catlet. </summary>
        /// <param name="body"> The <see cref="NewCatletRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Create a catlet. </remarks>
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

        /// <summary> Delete a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Deletes a catlet. </remarks>
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

        /// <summary> Delete a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Deletes a catlet. </remarks>
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
        /// <param name="id"> The <see cref="string"/> to use. </param>
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
        /// <param name="id"> The <see cref="string"/> to use. </param>
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

        /// <summary> Update a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateCatletRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> UpdateAsync(string id, UpdateCatletRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Update");
            scope.Start();
            try
            {
                return await RestClient.UpdateAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Update a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateCatletRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Update(string id, UpdateCatletRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Update");
            scope.Start();
            try
            {
                return RestClient.Update(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Expand catlet config. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="ExpandCatletConfigRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Expand the config for an existing catlet. </remarks>
        public virtual async Task<Response<Models.Operation>> ExpandConfigAsync(string id, ExpandCatletConfigRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ExpandConfig");
            scope.Start();
            try
            {
                return await RestClient.ExpandConfigAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Expand catlet config. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="ExpandCatletConfigRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Expand the config for an existing catlet. </remarks>
        public virtual Response<Models.Operation> ExpandConfig(string id, ExpandCatletConfigRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ExpandConfig");
            scope.Start();
            try
            {
                return RestClient.ExpandConfig(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Expand new catlet config. </summary>
        /// <param name="body"> The <see cref="ExpandNewCatletConfigRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Expand the config for a new catlet. </remarks>
        public virtual async Task<Response<Models.Operation>> ExpandNewConfigAsync(ExpandNewCatletConfigRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ExpandNewConfig");
            scope.Start();
            try
            {
                return await RestClient.ExpandNewConfigAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Expand new catlet config. </summary>
        /// <param name="body"> The <see cref="ExpandNewCatletConfigRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Expand the config for a new catlet. </remarks>
        public virtual Response<Models.Operation> ExpandNewConfig(ExpandNewCatletConfigRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ExpandNewConfig");
            scope.Start();
            try
            {
                return RestClient.ExpandNewConfig(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Get a catlet configuration. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
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

        /// <summary> Get a catlet configuration. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
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

        /// <summary> Populate catlet config variables. </summary>
        /// <param name="body"> The <see cref="PopulateCatletConfigVariablesRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Populates the variables in a config for a new catlet based on the parent. </remarks>
        public virtual async Task<Response<Models.Operation>> PopulateConfigVariablesAsync(PopulateCatletConfigVariablesRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.PopulateConfigVariables");
            scope.Start();
            try
            {
                return await RestClient.PopulateConfigVariablesAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Populate catlet config variables. </summary>
        /// <param name="body"> The <see cref="PopulateCatletConfigVariablesRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Populates the variables in a config for a new catlet based on the parent. </remarks>
        public virtual Response<Models.Operation> PopulateConfigVariables(PopulateCatletConfigVariablesRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.PopulateConfigVariables");
            scope.Start();
            try
            {
                return RestClient.PopulateConfigVariables(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Start a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
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

        /// <summary> Start a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
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

        /// <summary> Stop a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="StopCatletRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> StopAsync(string id, StopCatletRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Stop");
            scope.Start();
            try
            {
                return await RestClient.StopAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Stop a catlet. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="StopCatletRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Stop(string id, StopCatletRequestBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.Stop");
            scope.Start();
            try
            {
                return RestClient.Stop(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Validate catlet config. </summary>
        /// <param name="body"> The <see cref="ValidateConfigRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Performs a quick validation of the catlet configuration. </remarks>
        public virtual async Task<Response<CatletConfigValidationResult>> ValidateConfigAsync(ValidateConfigRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ValidateConfig");
            scope.Start();
            try
            {
                return await RestClient.ValidateConfigAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Validate catlet config. </summary>
        /// <param name="body"> The <see cref="ValidateConfigRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Performs a quick validation of the catlet configuration. </remarks>
        public virtual Response<CatletConfigValidationResult> ValidateConfig(ValidateConfigRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("CatletsClient.ValidateConfig");
            scope.Start();
            try
            {
                return RestClient.ValidateConfig(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> List all catlets. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<Catlet> ListAsync(string projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(projectId);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, null, Catlet.DeserializeCatlet, _clientDiagnostics, _pipeline, "CatletsClient.List", "value", null, cancellationToken);
        }

        /// <summary> List all catlets. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<Catlet> List(string projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(projectId);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, null, Catlet.DeserializeCatlet, _clientDiagnostics, _pipeline, "CatletsClient.List", "value", null, cancellationToken);
        }
    }
}
