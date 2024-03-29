// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ProjectMemberRole. </summary>
    public partial class ProjectMemberRole
    {
        /// <summary> Initializes a new instance of <see cref="ProjectMemberRole"/>. </summary>
        internal ProjectMemberRole()
        {
        }

        /// <summary> Initializes a new instance of <see cref="ProjectMemberRole"/>. </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="projectName"></param>
        /// <param name="memberId"></param>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        internal ProjectMemberRole(string id, string projectId, string projectName, string memberId, string roleId, string roleName)
        {
            Id = id;
            ProjectId = projectId;
            ProjectName = projectName;
            MemberId = memberId;
            RoleId = roleId;
            RoleName = roleName;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the project id. </summary>
        public string ProjectId { get; }
        /// <summary> Gets the project name. </summary>
        public string ProjectName { get; }
        /// <summary> Gets the member id. </summary>
        public string MemberId { get; }
        /// <summary> Gets the role id. </summary>
        public string RoleId { get; }
        /// <summary> Gets the role name. </summary>
        public string RoleName { get; }
    }
}
