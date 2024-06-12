// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class Operation
    {
        internal static Operation DeserializeOperation(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            OperationStatus? status = default;
            string statusMessage = default;
            IReadOnlyList<OperationResource> resources = default;
            IReadOnlyList<OperationLogEntry> logEntries = default;
            IReadOnlyList<Project> projects = default;
            IReadOnlyList<OperationTask> tasks = default;
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
                if (property.NameEquals("status"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    status = new OperationStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("statusMessage"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        statusMessage = null;
                        continue;
                    }
                    statusMessage = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resources"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<OperationResource> array = new List<OperationResource>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(OperationResource.DeserializeOperationResource(item));
                    }
                    resources = array;
                    continue;
                }
                if (property.NameEquals("logEntries"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<OperationLogEntry> array = new List<OperationLogEntry>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(OperationLogEntry.DeserializeOperationLogEntry(item));
                    }
                    logEntries = array;
                    continue;
                }
                if (property.NameEquals("projects"u8))
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
                    projects = array;
                    continue;
                }
                if (property.NameEquals("tasks"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<OperationTask> array = new List<OperationTask>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(OperationTask.DeserializeOperationTask(item));
                    }
                    tasks = array;
                    continue;
                }
            }
            return new Operation(
                id,
                status,
                statusMessage,
                resources ?? new ChangeTrackingList<OperationResource>(),
                logEntries ?? new ChangeTrackingList<OperationLogEntry>(),
                projects ?? new ChangeTrackingList<Project>(),
                tasks ?? new ChangeTrackingList<OperationTask>());
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static Operation FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeOperation(document.RootElement);
        }
    }
}
