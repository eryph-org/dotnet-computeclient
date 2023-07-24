// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualNetworkList. </summary>
    internal partial class VirtualNetworkList
    {
        /// <summary> Initializes a new instance of VirtualNetworkList. </summary>
        internal VirtualNetworkList()
        {
            Value = new ChangeTrackingList<VirtualNetwork>();
        }

        /// <summary> Initializes a new instance of VirtualNetworkList. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal VirtualNetworkList(string count, string nextLink, IReadOnlyList<VirtualNetwork> value)
        {
            Count = count;
            NextLink = nextLink;
            Value = value;
        }

        /// <summary> Gets the count. </summary>
        public string Count { get; }
        /// <summary> Gets the next link. </summary>
        public string NextLink { get; }
        /// <summary> Gets the value. </summary>
        public IReadOnlyList<VirtualNetwork> Value { get; }
    }
}