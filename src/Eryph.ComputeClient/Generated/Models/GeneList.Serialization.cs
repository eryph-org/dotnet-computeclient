// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    internal partial class GeneList
    {
        internal static GeneList DeserializeGeneList(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            IReadOnlyList<Gene> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("value"u8))
                {
                    List<Gene> array = new List<Gene>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Gene.DeserializeGene(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new GeneList(value);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static GeneList FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeGeneList(document.RootElement);
        }
    }
}
