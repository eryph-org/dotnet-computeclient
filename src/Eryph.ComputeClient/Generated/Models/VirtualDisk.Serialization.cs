// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class VirtualDisk
    {
        internal static VirtualDisk DeserializeVirtualDisk(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string name = default;
            string location = default;
            string dataStore = default;
            Project project = default;
            string environment = default;
            VirtualDiskGeneInfo gene = default;
            string path = default;
            long? sizeBytes = default;
            string parentId = default;
            IReadOnlyList<VirtualDiskAttachedCatlet> attachedCatlets = default;
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
                if (property.NameEquals("location"u8))
                {
                    location = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("data_store"u8))
                {
                    dataStore = property.Value.GetString();
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
                if (property.NameEquals("gene"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    gene = VirtualDiskGeneInfo.DeserializeVirtualDiskGeneInfo(property.Value);
                    continue;
                }
                if (property.NameEquals("path"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        path = null;
                        continue;
                    }
                    path = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("size_bytes"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sizeBytes = null;
                        continue;
                    }
                    sizeBytes = property.Value.GetInt64();
                    continue;
                }
                if (property.NameEquals("parent_id"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        parentId = null;
                        continue;
                    }
                    parentId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("attached_catlets"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<VirtualDiskAttachedCatlet> array = new List<VirtualDiskAttachedCatlet>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(VirtualDiskAttachedCatlet.DeserializeVirtualDiskAttachedCatlet(item));
                    }
                    attachedCatlets = array;
                    continue;
                }
            }
            return new VirtualDisk(
                id,
                name,
                location,
                dataStore,
                project,
                environment,
                gene,
                path,
                sizeBytes,
                parentId,
                attachedCatlets ?? new ChangeTrackingList<VirtualDiskAttachedCatlet>());
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static VirtualDisk FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeVirtualDisk(document.RootElement);
        }
    }
}
