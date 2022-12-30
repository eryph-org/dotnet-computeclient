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
    internal partial class RestClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> Initializes a new instance of RestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        public RestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            ClientDiagnostics = clientDiagnostics ?? throw new ArgumentNullException(nameof(clientDiagnostics));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _endpoint = endpoint ?? new Uri("https://localhost:50864/compute");
        }

        internal HttpMessage CreateGetRequest(string id, DateTimeOffset? logTimeStamp, string expand)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/operations/", false);
            uri.AppendPath(id, true);
            if (logTimeStamp != null)
            {
                uri.AppendQuery("logTimeStamp", logTimeStamp.Value, "O", true);
            }
            if (expand != null)
            {
                uri.AppendQuery("expand", expand, true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, text/json");
            return message;
        }

        /// <summary> Get a operation. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public async Task<Response<Models.Operation>> GetAsync(string id, DateTimeOffset? logTimeStamp = null, string expand = null, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(id, logTimeStamp, expand);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        Models.Operation value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await ClientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Get a operation. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public Response<Models.Operation> Get(string id, DateTimeOffset? logTimeStamp = null, string expand = null, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(id, logTimeStamp, expand);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        Models.Operation value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = Models.Operation.DeserializeOperation(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw ClientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateListRequest(DateTimeOffset? logTimeStamp, string expand, bool? count, string project)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/operations", false);
            if (logTimeStamp != null)
            {
                uri.AppendQuery("logTimeStamp", logTimeStamp.Value, "O", true);
            }
            if (expand != null)
            {
                uri.AppendQuery("expand", expand, true);
            }
            if (count != null)
            {
                uri.AppendQuery("count", count.Value, true);
            }
            if (project != null)
            {
                uri.AppendQuery("project", project, true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, text/json");
            return message;
        }

        /// <summary> List all Operations. </summary>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<OperationList>> ListAsync(DateTimeOffset? logTimeStamp = null, string expand = null, bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest(logTimeStamp, expand, count, project);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        OperationList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = OperationList.DeserializeOperationList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await ClientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> List all Operations. </summary>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<OperationList> List(DateTimeOffset? logTimeStamp = null, string expand = null, bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest(logTimeStamp, expand, count, project);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        OperationList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = OperationList.DeserializeOperationList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw ClientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateListNextPageRequest(string nextLink, DateTimeOffset? logTimeStamp, string expand, bool? count, string project)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendRawNextLink(nextLink, false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, text/json");
            return message;
        }

        /// <summary> List all Operations. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/> is null. </exception>
        public async Task<Response<OperationList>> ListNextPageAsync(string nextLink, DateTimeOffset? logTimeStamp = null, string expand = null, bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            if (nextLink == null)
            {
                throw new ArgumentNullException(nameof(nextLink));
            }

            using var message = CreateListNextPageRequest(nextLink, logTimeStamp, expand, count, project);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        OperationList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = OperationList.DeserializeOperationList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await ClientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> List all Operations. </summary>
        /// <param name="nextLink"> The URL to the next page of results. </param>
        /// <param name="logTimeStamp"> The DateTimeISO8601 to use. </param>
        /// <param name="expand"> The String to use. </param>
        /// <param name="count"> The Boolean to use. </param>
        /// <param name="project"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="nextLink"/> is null. </exception>
        public Response<OperationList> ListNextPage(string nextLink, DateTimeOffset? logTimeStamp = null, string expand = null, bool? count = null, string project = null, CancellationToken cancellationToken = default)
        {
            if (nextLink == null)
            {
                throw new ArgumentNullException(nameof(nextLink));
            }

            using var message = CreateListNextPageRequest(nextLink, logTimeStamp, expand, count, project);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        OperationList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = OperationList.DeserializeOperationList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw ClientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
