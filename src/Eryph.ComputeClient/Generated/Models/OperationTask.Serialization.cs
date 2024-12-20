// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class OperationTask
    {
        internal static OperationTask DeserializeOperationTask(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string parentTaskId = default;
            string name = default;
            string displayName = default;
            int progress = default;
            OperationTaskStatus status = default;
            OperationTaskReference reference = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"u8))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("parent_task_id"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        parentTaskId = null;
                        continue;
                    }
                    parentTaskId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("display_name"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        displayName = null;
                        continue;
                    }
                    displayName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("progress"u8))
                {
                    progress = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("status"u8))
                {
                    status = new OperationTaskStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("reference"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    reference = OperationTaskReference.DeserializeOperationTaskReference(property.Value);
                    continue;
                }
            }
            return new OperationTask(
                id,
                parentTaskId,
                name,
                displayName,
                progress,
                status,
                reference);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static OperationTask FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeOperationTask(document.RootElement);
        }
    }
}
