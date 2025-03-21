// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class VirtualNetwork
    {
        internal static VirtualNetwork DeserializeVirtualNetwork(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string name = default;
            Project project = default;
            string environment = default;
            string providerName = default;
            string ipNetwork = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("project"u8))
                {
                    project = Project.DeserializeProject(property.Value);
                    continue;
                }
                if (property.NameEquals("environment"u8))
                {
                    environment = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("provider_name"u8))
                {
                    providerName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("ip_network"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        ipNetwork = null;
                        continue;
                    }
                    ipNetwork = property.Value.GetString();
                    continue;
                }
            }
            return new VirtualNetwork(
                id,
                name,
                project,
                environment,
                providerName,
                ipNetwork);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static VirtualNetwork FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeVirtualNetwork(document.RootElement);
        }
    }
}
