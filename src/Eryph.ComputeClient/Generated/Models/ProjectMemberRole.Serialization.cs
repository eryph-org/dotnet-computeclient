// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;

namespace Eryph.ComputeClient.Models
{
    public partial class ProjectMemberRole
    {
        internal static ProjectMemberRole DeserializeProjectMemberRole(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            string id = default;
            string projectId = default;
            string projectName = default;
            string memberId = default;
            string roleId = default;
            string roleName = default;
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
                if (property.NameEquals("projectId"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        projectId = null;
                        continue;
                    }
                    projectId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("projectName"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        projectName = null;
                        continue;
                    }
                    projectName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("memberId"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        memberId = null;
                        continue;
                    }
                    memberId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("roleId"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        roleId = null;
                        continue;
                    }
                    roleId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("roleName"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        roleName = null;
                        continue;
                    }
                    roleName = property.Value.GetString();
                    continue;
                }
            }
            return new ProjectMemberRole(
                id,
                projectId,
                projectName,
                memberId,
                roleId,
                roleName);
        }
    }
}
