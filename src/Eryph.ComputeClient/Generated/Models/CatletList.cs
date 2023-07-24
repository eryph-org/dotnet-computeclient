// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The CatletList. </summary>
    internal partial class CatletList
    {
        /// <summary> Initializes a new instance of CatletList. </summary>
        internal CatletList()
        {
            Value = new ChangeTrackingList<Catlet>();
        }

        /// <summary> Initializes a new instance of CatletList. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal CatletList(string count, string nextLink, IReadOnlyList<Catlet> value)
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
        public IReadOnlyList<Catlet> Value { get; }
    }
}