// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    public partial class CatletConfiguration
    {
        internal static CatletConfiguration DeserializeCatletConfiguration(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Optional<JsonElement> configuration = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("configuration"u8))
                {
                    configuration = property.Value.Clone();
                    continue;
                }
            }
            return new CatletConfiguration(configuration);
        }
    }
}