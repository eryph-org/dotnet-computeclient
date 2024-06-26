// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    internal partial class ProjectList
    {
        internal static ProjectList DeserializeProjectList(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string count = default;
            string nextLink = default;
            IReadOnlyList<Project> value = default;
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
                    List<Project> array = new List<Project>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Project.DeserializeProject(item));
                    }
                    value = array;
                    continue;
                }
            }
            return new ProjectList(count, nextLink, value ?? new ChangeTrackingList<Project>());
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static ProjectList FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeProjectList(document.RootElement);
        }
    }
}
