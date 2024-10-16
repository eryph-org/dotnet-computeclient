// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The OperationList. </summary>
    internal partial class OperationList
    {
        /// <summary> Initializes a new instance of <see cref="OperationList"/>. </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        internal OperationList(IEnumerable<Operation> value)
        {
            Argument.AssertNotNull(value, nameof(value));

            Value = value.ToList();
        }

        /// <summary> Initializes a new instance of <see cref="OperationList"/>. </summary>
        /// <param name="value"></param>
        internal OperationList(IReadOnlyList<Operation> value)
        {
            Value = value;
        }

        /// <summary> Gets the value. </summary>
        public IReadOnlyList<Operation> Value { get; }
    }
}
