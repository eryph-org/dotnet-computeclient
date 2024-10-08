// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient
{
    internal partial class ProjectMembersRestClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> Initializes a new instance of ProjectMembersRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        public ProjectMembersRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            ClientDiagnostics = clientDiagnostics ?? throw new ArgumentNullException(nameof(clientDiagnostics));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _endpoint = endpoint ?? new Uri("https://localhost:8000/compute");
        }

        internal HttpMessage CreateAddRequest(string projectId, NewProjectMemberBody body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/members", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            request.Headers.Add("Content-Type", "application/json");
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(body);
            request.Content = content;
            return message;
        }

        /// <summary> Add a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="NewProjectMemberBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="body"/> is null. </exception>
        public async Task<Response<Models.Operation>> AddAsync(string projectId, NewProjectMemberBody body, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var message = CreateAddRequest(projectId, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        Models.Operation value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Add a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="NewProjectMemberBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="body"/> is null. </exception>
        public Response<Models.Operation> Add(string projectId, NewProjectMemberBody body, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var message = CreateAddRequest(projectId, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        Models.Operation value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateListRequest(string projectId)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/members", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> List all project members. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> is null. </exception>
        public async Task<Response<ProjectMemberRoleList>> ListAsync(string projectId, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            using var message = CreateListRequest(projectId);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ProjectMemberRoleList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = ProjectMemberRoleList.DeserializeProjectMemberRoleList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> List all project members. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> is null. </exception>
        public Response<ProjectMemberRoleList> List(string projectId, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            using var message = CreateListRequest(projectId);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ProjectMemberRoleList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = ProjectMemberRoleList.DeserializeProjectMemberRoleList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateRemoveRequest(string projectId, string id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/members/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Remove a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="id"/> is null. </exception>
        /// <remarks> Removes a project member assignment. </remarks>
        public async Task<Response<Models.Operation>> RemoveAsync(string projectId, string id, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateRemoveRequest(projectId, id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        Models.Operation value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Remove a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="id"/> is null. </exception>
        /// <remarks> Removes a project member assignment. </remarks>
        public Response<Models.Operation> Remove(string projectId, string id, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateRemoveRequest(projectId, id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        Models.Operation value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetRequest(string projectId, string id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/members/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Get a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="id"/> is null. </exception>
        public async Task<Response<ProjectMemberRole>> GetAsync(string projectId, string id, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(projectId, id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ProjectMemberRole value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = ProjectMemberRole.DeserializeProjectMemberRole(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Get a project member. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="id"/> is null. </exception>
        public Response<ProjectMemberRole> Get(string projectId, string id, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(projectId, id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ProjectMemberRole value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = ProjectMemberRole.DeserializeProjectMemberRole(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }
    }
}
