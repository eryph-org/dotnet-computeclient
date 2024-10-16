// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualNetworkList. </summary>
    internal partial class VirtualNetworkList
    {
        /// <summary> Initializes a new instance of <see cref="VirtualNetworkList"/>. </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        internal VirtualNetworkList(IEnumerable<VirtualNetwork> value)
        {
            Argument.AssertNotNull(value, nameof(value));

            Value = value.ToList();
        }

        /// <summary> Initializes a new instance of <see cref="VirtualNetworkList"/>. </summary>
        /// <param name="value"></param>
        internal VirtualNetworkList(IReadOnlyList<VirtualNetwork> value)
        {
            Value = value;
        }

        /// <summary> Gets the value. </summary>
        public IReadOnlyList<VirtualNetwork> Value { get; }
    }
}
