// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The ProjectMemberRole. </summary>
    public partial class ProjectMemberRole
    {
        /// <summary> Initializes a new instance of <see cref="ProjectMemberRole"/>. </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <param name="memberId"></param>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="project"/>, <paramref name="memberId"/>, <paramref name="roleId"/> or <paramref name="roleName"/> is null. </exception>
        internal ProjectMemberRole(string id, Project project, string memberId, string roleId, string roleName)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(project, nameof(project));
            Argument.AssertNotNull(memberId, nameof(memberId));
            Argument.AssertNotNull(roleId, nameof(roleId));
            Argument.AssertNotNull(roleName, nameof(roleName));

            Id = id;
            Project = project;
            MemberId = memberId;
            RoleId = roleId;
            RoleName = roleName;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the project. </summary>
        public Project Project { get; }
        /// <summary> Gets the member id. </summary>
        public string MemberId { get; }
        /// <summary> Gets the role id. </summary>
        public string RoleId { get; }
        /// <summary> Gets the role name. </summary>
        public string RoleName { get; }
    }
}
