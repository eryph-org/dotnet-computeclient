// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The UpdateCatletRequestBody. </summary>
    public partial class UpdateCatletRequestBody
    {
        /// <summary> Initializes a new instance of <see cref="UpdateCatletRequestBody"/>. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="configuration"/> is null. </exception>
        public UpdateCatletRequestBody(object configuration)
        {
            Argument.AssertNotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        /// <summary> Initializes a new instance of <see cref="UpdateCatletRequestBody"/>. </summary>
        /// <param name="correlationId"></param>
        /// <param name="configuration"> Anything. </param>
        internal UpdateCatletRequestBody(Guid? correlationId, object configuration)
        {
            CorrelationId = correlationId;
            Configuration = configuration;
        }

        /// <summary> Gets or sets the correlation id. </summary>
        public Guid? CorrelationId { get; set; }
        /// <summary> Anything. </summary>
        public object Configuration { get; }
    }
}
