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
    internal partial class GenesRestClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> Initializes a new instance of GenesRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        public GenesRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            ClientDiagnostics = clientDiagnostics ?? throw new ArgumentNullException(nameof(clientDiagnostics));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _endpoint = endpoint ?? new Uri("https://localhost:8000/compute");
        }

        internal HttpMessage CreateCleanupRequest()
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/genes", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Remove all unused genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Remove all unused genes from the local gene pool. </remarks>
        public async Task<Response<Models.Operation>> CleanupAsync(CancellationToken cancellationToken = default)
        {
            using var message = CreateCleanupRequest();
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

        /// <summary> Remove all unused genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <remarks> Remove all unused genes from the local gene pool. </remarks>
        public Response<Models.Operation> Cleanup(CancellationToken cancellationToken = default)
        {
            using var message = CreateCleanupRequest();
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

        internal HttpMessage CreateListRequest()
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/genes", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> List all genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<GeneList>> ListAsync(CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest();
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        GeneList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = GeneList.DeserializeGeneList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> List all genes. </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<GeneList> List(CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest();
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        GeneList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = GeneList.DeserializeGeneList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateDeleteRequest(string id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/genes/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Remove a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        /// <remarks> Remove a gene from the local gene pool. </remarks>
        public async Task<Response<Models.Operation>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateDeleteRequest(id);
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

        /// <summary> Remove a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        /// <remarks> Remove a gene from the local gene pool. </remarks>
        public Response<Models.Operation> Delete(string id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateDeleteRequest(id);
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

        internal HttpMessage CreateGetRequest(string id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/genes/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Get a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public async Task<Response<GeneWithUsage>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        GeneWithUsage value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = GeneWithUsage.DeserializeGeneWithUsage(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Get a gene. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public Response<GeneWithUsage> Get(string id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            using var message = CreateGetRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        GeneWithUsage value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = GeneWithUsage.DeserializeGeneWithUsage(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }
    }
}
