// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualDiskList. </summary>
    internal partial class VirtualDiskList
    {
        /// <summary> Initializes a new instance of <see cref="VirtualDiskList"/>. </summary>
        internal VirtualDiskList()
        {
            Value = new ChangeTrackingList<VirtualDisk>();
        }

        /// <summary> Initializes a new instance of <see cref="VirtualDiskList"/>. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal VirtualDiskList(string count, string nextLink, IReadOnlyList<VirtualDisk> value)
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
        public IReadOnlyList<VirtualDisk> Value { get; }
    }
}
