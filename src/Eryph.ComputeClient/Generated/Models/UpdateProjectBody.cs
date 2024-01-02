// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The UpdateProjectBody. </summary>
    public partial class UpdateProjectBody
    {
        /// <summary> Initializes a new instance of <see cref="UpdateProjectBody"/>. </summary>
        public UpdateProjectBody()
        {
        }

        /// <summary> Initializes a new instance of <see cref="UpdateProjectBody"/>. </summary>
        /// <param name="correlationId"></param>
        /// <param name="name"></param>
        internal UpdateProjectBody(Guid? correlationId, string name)
        {
            CorrelationId = correlationId;
            Name = name;
        }

        /// <summary> Gets or sets the correlation id. </summary>
        public Guid? CorrelationId { get; set; }
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; }
    }
}
