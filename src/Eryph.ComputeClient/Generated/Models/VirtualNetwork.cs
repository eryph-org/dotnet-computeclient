// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualNetwork. </summary>
    public partial class VirtualNetwork
    {
        /// <summary> Initializes a new instance of <see cref="VirtualNetwork"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="providerName"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="name"/>, <paramref name="project"/>, <paramref name="environment"/> or <paramref name="providerName"/> is null. </exception>
        internal VirtualNetwork(string id, string name, Project project, string environment, string providerName)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(name, nameof(name));
            Argument.AssertNotNull(project, nameof(project));
            Argument.AssertNotNull(environment, nameof(environment));
            Argument.AssertNotNull(providerName, nameof(providerName));

            Id = id;
            Name = name;
            Project = project;
            Environment = environment;
            ProviderName = providerName;
        }

        /// <summary> Initializes a new instance of <see cref="VirtualNetwork"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="providerName"></param>
        /// <param name="ipNetwork"></param>
        internal VirtualNetwork(string id, string name, Project project, string environment, string providerName, string ipNetwork)
        {
            Id = id;
            Name = name;
            Project = project;
            Environment = environment;
            ProviderName = providerName;
            IpNetwork = ipNetwork;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the project. </summary>
        public Project Project { get; }
        /// <summary> Gets the environment. </summary>
        public string Environment { get; }
        /// <summary> Gets the provider name. </summary>
        public string ProviderName { get; }
        /// <summary> Gets the ip network. </summary>
        public string IpNetwork { get; }
    }
}
