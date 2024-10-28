// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class VirtualDiskGeneInfo
    {
        internal static VirtualDiskGeneInfo DeserializeVirtualDiskGeneInfo(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string geneSet = default;
            string name = default;
            string architecture = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("gene_set"u8))
                {
                    geneSet = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("architecture"u8))
                {
                    architecture = property.Value.GetString();
                    continue;
                }
            }
            return new VirtualDiskGeneInfo(geneSet, name, architecture);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static VirtualDiskGeneInfo FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeVirtualDiskGeneInfo(document.RootElement);
        }
    }
}
