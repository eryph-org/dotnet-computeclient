// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualDiskGeneInfo. </summary>
    public partial class VirtualDiskGeneInfo
    {
        /// <summary> Initializes a new instance of <see cref="VirtualDiskGeneInfo"/>. </summary>
        /// <param name="geneSet"></param>
        /// <param name="geneName"></param>
        /// <param name="architecture"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="geneSet"/>, <paramref name="geneName"/> or <paramref name="architecture"/> is null. </exception>
        internal VirtualDiskGeneInfo(string geneSet, string geneName, string architecture)
        {
            Argument.AssertNotNull(geneSet, nameof(geneSet));
            Argument.AssertNotNull(geneName, nameof(geneName));
            Argument.AssertNotNull(architecture, nameof(architecture));

            GeneSet = geneSet;
            GeneName = geneName;
            Architecture = architecture;
        }

        /// <summary> Gets the gene set. </summary>
        public string GeneSet { get; }
        /// <summary> Gets the gene name. </summary>
        public string GeneName { get; }
        /// <summary> Gets the architecture. </summary>
        public string Architecture { get; }
    }
}
