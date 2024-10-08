// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Eryph.ComputeClient.Models
{
    /// <summary> The StopCatletRequestBody. </summary>
    public partial class StopCatletRequestBody
    {
        /// <summary> Initializes a new instance of <see cref="StopCatletRequestBody"/>. </summary>
        /// <param name="mode"></param>
        public StopCatletRequestBody(CatletStopMode mode)
        {
            Mode = mode;
        }

        /// <summary> Gets the mode. </summary>
        public CatletStopMode Mode { get; }
    }
}
