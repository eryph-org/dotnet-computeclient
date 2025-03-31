// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ApiVersion. </summary>
    public partial class ApiVersion
    {
        /// <summary> Initializes a new instance of <see cref="ApiVersion"/>. </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        internal ApiVersion(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        /// <summary> Gets the major. </summary>
        public int Major { get; }
        /// <summary> Gets the minor. </summary>
        public int Minor { get; }
    }
}
