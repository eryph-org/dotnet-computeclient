// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    internal partial class CatletList
    {
        internal static CatletList DeserializeCatletList(JsonElement element)
        {
            Optional<string> count = default;
            Optional<string> nextLink = default;
            Optional<IReadOnlyList<Catlet>> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("count"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        count = null;
                        continue;
                    }
                    count = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("nextLink"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        nextLink = null;
                        continue;
                    }
                    nextLink = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("value"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        value = null;
                        continue;
                    }
                    List<Catlet> array = new List<Catlet>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Catlet.DeserializeCatlet(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new CatletList(count.Value, nextLink.Value, Optional.ToList(value));
        }
    }
}
