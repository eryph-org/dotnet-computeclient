// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The PopulateCatletConfigVariablesRequest. </summary>
    public partial class PopulateCatletConfigVariablesRequest
    {
        /// <summary> Initializes a new instance of <see cref="PopulateCatletConfigVariablesRequest"/>. </summary>
        /// <param name="configuration"> Anything. </param>
        public PopulateCatletConfigVariablesRequest(JsonElement configuration)
        {
            Configuration = configuration;
        }

        /// <summary> Initializes a new instance of <see cref="PopulateCatletConfigVariablesRequest"/>. </summary>
        /// <param name="correlationId"></param>
        /// <param name="configuration"> Anything. </param>
        internal PopulateCatletConfigVariablesRequest(Guid? correlationId, JsonElement configuration)
        {
            CorrelationId = correlationId;
            Configuration = configuration;
        }

        /// <summary> Gets or sets the correlation id. </summary>
        public Guid? CorrelationId { get; set; }
    }
}
