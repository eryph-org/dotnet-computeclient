// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ExpandNewCatletConfigRequest. </summary>
    public partial class ExpandNewCatletConfigRequest
    {
        /// <summary> Initializes a new instance of <see cref="ExpandNewCatletConfigRequest"/>. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="configuration"/> is null. </exception>
        public ExpandNewCatletConfigRequest(object configuration)
        {
            Argument.AssertNotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        /// <summary> Initializes a new instance of <see cref="ExpandNewCatletConfigRequest"/>. </summary>
        /// <param name="correlationId"></param>
        /// <param name="configuration"> Anything. </param>
        internal ExpandNewCatletConfigRequest(Guid? correlationId, object configuration)
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
