// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Eryph.ComputeClient;

namespace Eryph.ComputeClient.Models
{
    internal partial class OperationList
    {
        internal static OperationList DeserializeOperationList(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string count = default;
            string nextLink = default;
            IReadOnlyList<Operation> value = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("count"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        count = null;
                        continue;
                    }
                    count = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("nextLink"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        nextLink = null;
                        continue;
                    }
                    nextLink = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("value"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<Operation> array = new List<Operation>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Operation.DeserializeOperation(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new OperationList(count, nextLink, value ?? new ChangeTrackingList<Operation>());
        }
    }
}
