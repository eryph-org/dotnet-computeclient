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
    /// <summary> The Projects service client. </summary>
    public partial class ProjectsClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal ProjectsRestClient RestClient { get; }

        /// <summary> Initializes a new instance of ProjectsClient for mocking. </summary>
        protected ProjectsClient()
        {
        }

        /// <summary> Initializes a new instance of ProjectsClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal ProjectsClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new ProjectsRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Creates a new project. </summary>
        /// <param name="body"> The <see cref="NewProjectRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates a project. </remarks>
        public virtual async Task<Response<Models.Operation>> CreateAsync(NewProjectRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Create");
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

        /// <summary> Creates a new project. </summary>
        /// <param name="body"> The <see cref="NewProjectRequest"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Creates a project. </remarks>
        public virtual Response<Models.Operation> Create(NewProjectRequest body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Create");
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

        /// <summary> Deletes a project. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Delete");
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

        /// <summary> Deletes a project. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Delete(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Delete");
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

        /// <summary> Get a projects. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Project>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Get");
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

        /// <summary> Get a projects. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Project> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Get");
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

        /// <summary> Updates a project. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateProjectBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<Models.Operation>> UpdateAsync(string id, UpdateProjectBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Update");
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

        /// <summary> Updates a project. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateProjectBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<Models.Operation> Update(string id, UpdateProjectBody body, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ProjectsClient.Update");
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

        /// <summary> List all projects. </summary>
        /// <param name="count"> The <see cref="bool"/>? to use. </param>
        /// <param name="projectId"> The <see cref="Guid"/>? to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual AsyncPageable<Project> ListAsync(bool? count = null, Guid? projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, projectId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, projectId);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, Project.DeserializeProject, _clientDiagnostics, _pipeline, "ProjectsClient.List", "value", "nextLink", cancellationToken);
        }

        /// <summary> List all projects. </summary>
        /// <param name="count"> The <see cref="bool"/>? to use. </param>
        /// <param name="projectId"> The <see cref="Guid"/>? to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Pageable<Project> List(bool? count = null, Guid? projectId = null, CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => RestClient.CreateListRequest(count, projectId);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => RestClient.CreateListNextPageRequest(nextLink, count, projectId);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, Project.DeserializeProject, _clientDiagnostics, _pipeline, "ProjectsClient.List", "value", "nextLink", cancellationToken);
        }
    }
}
