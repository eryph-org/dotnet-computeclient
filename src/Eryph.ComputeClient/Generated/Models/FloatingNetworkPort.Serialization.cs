// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class FloatingNetworkPort
    {
        internal static FloatingNetworkPort DeserializeFloatingNetworkPort(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string name = default;
            string provider = default;
            string subnet = default;
            IReadOnlyList<string> ipV4Addresses = default;
            IReadOnlyList<string> ipV4Subnets = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("provider"u8))
                {
                    provider = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("subnet"u8))
                {
                    subnet = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("ip_v4_addresses"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(item.GetString());
                    }
                    ipV4Addresses = array;
                    continue;
                }
                if (property.NameEquals("ip_v4_subnets"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(item.GetString());
                    }
                    ipV4Subnets = array;
                    continue;
                }
            }
            return new FloatingNetworkPort(name, provider, subnet, ipV4Addresses ?? new ChangeTrackingList<string>(), ipV4Subnets ?? new ChangeTrackingList<string>());
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static FloatingNetworkPort FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeFloatingNetworkPort(document.RootElement);
        }
    }
}
