// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

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
            Optional<Guid> id = default;
            Optional<string> name = default;
            Optional<string> storageIdentifier = default;
            Optional<string> dataStore = default;
            Optional<string> project = default;
            Optional<string> environment = default;
            Optional<string> path = default;
            Optional<long?> sizeBytes = default;
            Optional<Guid> parentId = default;
            Optional<IReadOnlyList<CatletDrive>> attachedDrives = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    id = property.Value.GetGuid();
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
                if (property.NameEquals("storageIdentifier"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        storageIdentifier = null;
                        continue;
                    }
                    storageIdentifier = property.Value.GetString();
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
                        continue;
                    }
                    parentId = property.Value.GetGuid();
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
            return new VirtualDisk(Optional.ToNullable(id), name.Value, storageIdentifier.Value, dataStore.Value, project.Value, environment.Value, path.Value, Optional.ToNullable(sizeBytes), Optional.ToNullable(parentId), Optional.ToList(attachedDrives));
        }
    }
}
