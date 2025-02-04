// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ProjectNetworksConfigValidationResult. </summary>
    public partial class ProjectNetworksConfigValidationResult
    {
        /// <summary> Initializes a new instance of <see cref="ProjectNetworksConfigValidationResult"/>. </summary>
        internal ProjectNetworksConfigValidationResult()
        {
            Errors = new ChangeTrackingList<ValidationIssue>();
        }

        /// <summary> Initializes a new instance of <see cref="ProjectNetworksConfigValidationResult"/>. </summary>
        /// <param name="isValid"> Indicates whether the network configuration is valid. </param>
        /// <param name="errors"> Contains a list of the issues when the configuration is invalid. </param>
        internal ProjectNetworksConfigValidationResult(bool? isValid, IReadOnlyList<ValidationIssue> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }

        /// <summary> Indicates whether the network configuration is valid. </summary>
        public bool? IsValid { get; }
        /// <summary> Contains a list of the issues when the configuration is invalid. </summary>
        public IReadOnlyList<ValidationIssue> Errors { get; }
    }
}
