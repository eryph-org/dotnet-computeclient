// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The NewProjectRequest. </summary>
    public partial class NewProjectRequest
    {
        /// <summary> Initializes a new instance of NewProjectRequest. </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> is null. </exception>
        public NewProjectRequest(string name)
        {
            Argument.AssertNotNull(name, nameof(name));

            Name = name;
        }

        /// <summary> Gets or sets the correlation id. </summary>
        public Guid? CorrelationId { get; set; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
    }
}
