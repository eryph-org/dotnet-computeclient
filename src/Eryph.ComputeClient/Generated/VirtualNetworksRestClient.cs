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
    internal partial class VirtualNetworksRestClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> Initializes a new instance of VirtualNetworksRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        public VirtualNetworksRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            ClientDiagnostics = clientDiagnostics ?? throw new ArgumentNullException(nameof(clientDiagnostics));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _endpoint = endpoint ?? new Uri("https://localhost:8000/compute");
        }

        internal HttpMessage CreateGetRequest(string id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/virtualnetworks/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Get a virtual network. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public async Task<Response<VirtualNetwork>> GetAsync(string id, CancellationToken cancellationToken = default)
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
                        VirtualNetwork value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = VirtualNetwork.DeserializeVirtualNetwork(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Get a virtual network. </summary>
        /// <param name="id"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public Response<VirtualNetwork> Get(string id, CancellationToken cancellationToken = default)
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
                        VirtualNetwork value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = VirtualNetwork.DeserializeVirtualNetwork(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetConfigRequest(string projectId)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/virtualnetworks/config", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> Get the virtual network configuration of a project. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> is null. </exception>
        public async Task<Response<VirtualNetworkConfiguration>> GetConfigAsync(string projectId, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            using var message = CreateGetConfigRequest(projectId);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        VirtualNetworkConfiguration value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = VirtualNetworkConfiguration.DeserializeVirtualNetworkConfiguration(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> Get the virtual network configuration of a project. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> is null. </exception>
        public Response<VirtualNetworkConfiguration> GetConfig(string projectId, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            using var message = CreateGetConfigRequest(projectId);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        VirtualNetworkConfiguration value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = VirtualNetworkConfiguration.DeserializeVirtualNetworkConfiguration(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateUpdateConfigRequest(string projectId, UpdateProjectNetworksRequestBody body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Put;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/v1/projects/", false);
            uri.AppendPath(projectId, true);
            uri.AppendPath("/virtualnetworks/config", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            request.Headers.Add("Content-Type", "application/json");
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(body);
            request.Content = content;
            return message;
        }

        /// <summary> Update the virtual network configuration of a project. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateProjectNetworksRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="body"/> is null. </exception>
        public async Task<Response<Models.Operation>> UpdateConfigAsync(string projectId, UpdateProjectNetworksRequestBody body, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var message = CreateUpdateConfigRequest(projectId, body);
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

        /// <summary> Update the virtual network configuration of a project. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="body"> The <see cref="UpdateProjectNetworksRequestBody"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="projectId"/> or <paramref name="body"/> is null. </exception>
        public Response<Models.Operation> UpdateConfig(string projectId, UpdateProjectNetworksRequestBody body, CancellationToken cancellationToken = default)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            using var message = CreateUpdateConfigRequest(projectId, body);
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
            uri.AppendPath("/v1/virtualnetworks", false);
            if (projectId != null)
            {
                uri.AppendQuery("projectId", projectId, true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json, application/problem+json");
            return message;
        }

        /// <summary> List all virtual networks. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<VirtualNetworkList>> ListAsync(string projectId = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest(projectId);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        VirtualNetworkList value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = VirtualNetworkList.DeserializeVirtualNetworkList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }

        /// <summary> List all virtual networks. </summary>
        /// <param name="projectId"> The <see cref="string"/> to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<VirtualNetworkList> List(string projectId = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateListRequest(projectId);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        VirtualNetworkList value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = VirtualNetworkList.DeserializeVirtualNetworkList(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw new RequestFailedException(message.Response);
            }
        }
    }
}
