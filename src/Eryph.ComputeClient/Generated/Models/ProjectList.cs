// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Eryph.ComputeClient;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ProjectList. </summary>
    internal partial class ProjectList
    {
        /// <summary> Initializes a new instance of <see cref="ProjectList"/>. </summary>
        internal ProjectList()
        {
            Value = new ChangeTrackingList<Project>();
        }

        /// <summary> Initializes a new instance of <see cref="ProjectList"/>. </summary>
        /// <param name="count"></param>
        /// <param name="nextLink"></param>
        /// <param name="value"></param>
        internal ProjectList(string count, string nextLink, IReadOnlyList<Project> value)
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
        public IReadOnlyList<Project> Value { get; }
    }
}
