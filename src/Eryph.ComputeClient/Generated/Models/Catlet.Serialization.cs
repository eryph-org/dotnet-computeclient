// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class Catlet
    {
        internal static Catlet DeserializeCatlet(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string name = default;
            string vmId = default;
            Project project = default;
            CatletStatus status = default;
            IReadOnlyList<CatletNetwork> networks = default;
            IReadOnlyList<CatletNetworkAdapter> networkAdapters = default;
            IReadOnlyList<CatletDrive> drives = default;
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
                if (property.NameEquals("vm_id"u8))
                {
                    vmId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("project"u8))
                {
                    project = Project.DeserializeProject(property.Value);
                    continue;
                }
                if (property.NameEquals("status"u8))
                {
                    status = new CatletStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("networks"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<CatletNetwork> array = new List<CatletNetwork>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(CatletNetwork.DeserializeCatletNetwork(item));
                    }
                    networks = array;
                    continue;
                }
                if (property.NameEquals("network_adapters"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<CatletNetworkAdapter> array = new List<CatletNetworkAdapter>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(CatletNetworkAdapter.DeserializeCatletNetworkAdapter(item));
                    }
                    networkAdapters = array;
                    continue;
                }
                if (property.NameEquals("drives"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<CatletDrive> array = new List<CatletDrive>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(CatletDrive.DeserializeCatletDrive(item));
                    }
                    drives = array;
                    continue;
                }
            }
            return new Catlet(
                id,
                name,
                vmId,
                project,
                status,
                networks ?? new ChangeTrackingList<CatletNetwork>(),
                networkAdapters ?? new ChangeTrackingList<CatletNetworkAdapter>(),
                drives ?? new ChangeTrackingList<CatletDrive>());
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static Catlet FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeCatlet(document.RootElement);
        }
    }
}
