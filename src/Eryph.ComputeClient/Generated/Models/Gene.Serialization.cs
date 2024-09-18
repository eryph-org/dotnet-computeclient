// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class Gene
    {
        internal static Gene DeserializeGene(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            GeneType? geneType = default;
            string geneSet = default;
            string name = default;
            long? size = default;
            string hash = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        id = null;
                        continue;
                    }
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("geneType"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    geneType = new GeneType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("geneSet"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        geneSet = null;
                        continue;
                    }
                    geneSet = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        name = null;
                        continue;
                    }
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("size"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    size = property.Value.GetInt64();
                    continue;
                }
                if (property.NameEquals("hash"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        hash = null;
                        continue;
                    }
                    hash = property.Value.GetString();
                    continue;
                }
            }
            return new Gene(
                id,
                geneType,
                geneSet,
                name,
                size,
                hash);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static Gene FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeGene(document.RootElement);
        }
    }
}
