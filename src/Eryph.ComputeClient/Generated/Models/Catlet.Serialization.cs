// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

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
            Optional<string> id = default;
            Optional<string> name = default;
            Optional<CatletStatus> status = default;
            Optional<IReadOnlyList<CatletNetwork>> networks = default;
            Optional<IReadOnlyList<CatletNetworkAdapter>> networkAdapters = default;
            Optional<IReadOnlyList<CatletDrive>> drives = default;
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
                if (property.NameEquals("status"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
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
                if (property.NameEquals("networkAdapters"u8))
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
            return new Catlet(id.Value, name.Value, Optional.ToNullable(status), Optional.ToList(networks), Optional.ToList(networkAdapters), Optional.ToList(drives));
        }
    }
}
