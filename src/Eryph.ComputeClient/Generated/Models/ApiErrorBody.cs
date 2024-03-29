// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ApiErrorBody. </summary>
    internal partial class ApiErrorBody
    {
        /// <summary> Initializes a new instance of <see cref="ApiErrorBody"/>. </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="code"/> or <paramref name="message"/> is null. </exception>
        internal ApiErrorBody(string code, string message)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Code = code;
            Message = message;
            AdditionalProperties = new ChangeTrackingDictionary<string, object>();
        }

        /// <summary> Initializes a new instance of <see cref="ApiErrorBody"/>. </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="target"></param>
        /// <param name="additionalProperties"> Additional Properties. </param>
        internal ApiErrorBody(string code, string message, string target, IReadOnlyDictionary<string, object> additionalProperties)
        {
            Code = code;
            Message = message;
            Target = target;
            AdditionalProperties = additionalProperties;
        }

        /// <summary> Gets the code. </summary>
        public string Code { get; }
        /// <summary> Gets the message. </summary>
        public string Message { get; }
        /// <summary> Gets the target. </summary>
        public string Target { get; }
        /// <summary> Additional Properties. </summary>
        public IReadOnlyDictionary<string, object> AdditionalProperties { get; }
    }
}
