// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The NewVirtualDiskRequest. </summary>
    public partial class NewVirtualDiskRequest
    {
        /// <summary> Initializes a new instance of <see cref="NewVirtualDiskRequest"/>. </summary>
        /// <param name="projectId"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public NewVirtualDiskRequest(Guid projectId, string name, int size)
        {
            ProjectId = projectId;
            Name = name;
            Size = size;
        }

        /// <summary> Initializes a new instance of <see cref="NewVirtualDiskRequest"/>. </summary>
        /// <param name="projectId"></param>
        /// <param name="correlationId"></param>
        /// <param name="name"></param>
        /// <param name="environment"></param>
        /// <param name="store"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        internal NewVirtualDiskRequest(Guid projectId, Guid? correlationId, string name, string environment, string store, string location, int size)
        {
            ProjectId = projectId;
            CorrelationId = correlationId;
            Name = name;
            Environment = environment;
            Store = store;
            Location = location;
            Size = size;
        }

        /// <summary> Gets the project id. </summary>
        public Guid ProjectId { get; }
        /// <summary> Gets or sets the correlation id. </summary>
        public Guid? CorrelationId { get; set; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets or sets the environment. </summary>
        public string Environment { get; set; }
        /// <summary> Gets or sets the store. </summary>
        public string Store { get; set; }
        /// <summary> Gets or sets the location. </summary>
        public string Location { get; set; }
        /// <summary> Gets the size. </summary>
        public int Size { get; }
    }
}
