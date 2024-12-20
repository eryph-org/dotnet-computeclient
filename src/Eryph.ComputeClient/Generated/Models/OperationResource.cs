// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The OperationResource. </summary>
    public partial class OperationResource
    {
        /// <summary> Initializes a new instance of <see cref="OperationResource"/>. </summary>
        /// <param name="id"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceType"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> or <paramref name="resourceId"/> is null. </exception>
        internal OperationResource(string id, string resourceId, ResourceType resourceType)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(resourceId, nameof(resourceId));

            Id = id;
            ResourceId = resourceId;
            ResourceType = resourceType;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the resource id. </summary>
        public string ResourceId { get; }
        /// <summary> Gets the resource type. </summary>
        public ResourceType ResourceType { get; }
    }
}
