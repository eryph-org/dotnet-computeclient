// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The OperationList. </summary>
    internal partial class OperationList
    {
        /// <summary> Initializes a new instance of <see cref="OperationList"/>. </summary>
        internal OperationList()
        {
            Value = new ChangeTrackingList<Operation>();
        }

        /// <summary> Initializes a new instance of <see cref="OperationList"/>. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal OperationList(string count, string nextLink, IReadOnlyList<Operation> value)
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
        public IReadOnlyList<Operation> Value { get; }
    }
}
