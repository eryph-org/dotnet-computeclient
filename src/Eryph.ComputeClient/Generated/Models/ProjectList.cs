// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ProjectList. </summary>
    internal partial class ProjectList
    {
        /// <summary> Initializes a new instance of <see cref="ProjectList"/>. </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        internal ProjectList(IEnumerable<Project> value)
        {
            Argument.AssertNotNull(value, nameof(value));

            Value = value.ToList();
        }

        /// <summary> Initializes a new instance of <see cref="ProjectList"/>. </summary>
        /// <param name="value"></param>
        internal ProjectList(IReadOnlyList<Project> value)
        {
            Value = value;
        }

        /// <summary> Gets the value. </summary>
        public IReadOnlyList<Project> Value { get; }
    }
}
