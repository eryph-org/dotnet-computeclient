// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The GeneList. </summary>
    internal partial class GeneList
    {
        /// <summary> Initializes a new instance of <see cref="GeneList"/>. </summary>
        internal GeneList()
        {
            Value = new ChangeTrackingList<Gene>();
        }

        /// <summary> Initializes a new instance of <see cref="GeneList"/>. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal GeneList(string count, string nextLink, IReadOnlyList<Gene> value)
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
        public IReadOnlyList<Gene> Value { get; }
    }
}
