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
            string project = default;
            string environment = default;
            string path = default;
            long? sizeBytes = default;
            string parentId = default;
            IReadOnlyList<CatletDrive> attachedDrives = default;
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
                if (property.NameEquals("location"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        location = null;
                        continue;
                    }
                    location = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("dataStore"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dataStore = null;
                        continue;
                    }
                    dataStore = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("project"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        project = null;
                        continue;
                    }
                    project = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("environment"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        environment = null;
                        continue;
                    }
                    environment = property.Value.GetString();
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
                if (property.NameEquals("sizeBytes"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sizeBytes = null;
                        continue;
                    }
                    sizeBytes = property.Value.GetInt64();
                    continue;
                }
                if (property.NameEquals("parentId"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        parentId = null;
                        continue;
                    }
                    parentId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("attachedDrives"u8))
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
                    attachedDrives = array;
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
                path,
                sizeBytes,
                parentId,
                attachedDrives ?? new ChangeTrackingList<CatletDrive>());
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
