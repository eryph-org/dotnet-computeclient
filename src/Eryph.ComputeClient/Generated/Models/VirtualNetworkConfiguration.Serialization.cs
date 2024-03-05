// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;

namespace Eryph.ComputeClient.Models
{
    public partial class VirtualNetworkConfiguration
    {
        internal static VirtualNetworkConfiguration DeserializeVirtualNetworkConfiguration(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            JsonElement configuration = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("configuration"u8))
                {
                    configuration = property.Value.Clone();
                    continue;
                }
            }
            return new VirtualNetworkConfiguration(configuration);
        }
    }
}
