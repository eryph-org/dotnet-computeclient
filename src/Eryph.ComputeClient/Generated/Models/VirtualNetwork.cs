// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualNetwork. </summary>
    public partial class VirtualNetwork
    {
        /// <summary> Initializes a new instance of <see cref="VirtualNetwork"/>. </summary>
        internal VirtualNetwork()
        {
        }

        /// <summary> Initializes a new instance of <see cref="VirtualNetwork"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="projectId"></param>
        /// <param name="projectName"></param>
        /// <param name="tenantId"></param>
        /// <param name="providerName"></param>
        /// <param name="ipNetwork"></param>
        internal VirtualNetwork(string id, string name, string projectId, string projectName, string tenantId, string providerName, string ipNetwork)
        {
            Id = id;
            Name = name;
            ProjectId = projectId;
            ProjectName = projectName;
            TenantId = tenantId;
            ProviderName = providerName;
            IpNetwork = ipNetwork;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the project id. </summary>
        public string ProjectId { get; }
        /// <summary> Gets the project name. </summary>
        public string ProjectName { get; }
        /// <summary> Gets the tenant id. </summary>
        public string TenantId { get; }
        /// <summary> Gets the provider name. </summary>
        public string ProviderName { get; }
        /// <summary> Gets the ip network. </summary>
        public string IpNetwork { get; }
    }
}
