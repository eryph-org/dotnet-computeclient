// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualDisk. </summary>
    public partial class VirtualDisk
    {
        /// <summary> Initializes a new instance of <see cref="VirtualDisk"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="dataStore"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="name"/>, <paramref name="location"/>, <paramref name="dataStore"/>, <paramref name="project"/> or <paramref name="environment"/> is null. </exception>
        internal VirtualDisk(string id, string name, string location, string dataStore, string project, string environment)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(name, nameof(name));
            Argument.AssertNotNull(location, nameof(location));
            Argument.AssertNotNull(dataStore, nameof(dataStore));
            Argument.AssertNotNull(project, nameof(project));
            Argument.AssertNotNull(environment, nameof(environment));

            Id = id;
            Name = name;
            Location = location;
            DataStore = dataStore;
            Project = project;
            Environment = environment;
            AttachedCatlets = new ChangeTrackingList<VirtualDiskAttachmentInfo>();
        }

        /// <summary> Initializes a new instance of <see cref="VirtualDisk"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="dataStore"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="path"></param>
        /// <param name="sizeBytes"></param>
        /// <param name="parentId"></param>
        /// <param name="attachedCatlets"></param>
        internal VirtualDisk(string id, string name, string location, string dataStore, string project, string environment, string path, long? sizeBytes, string parentId, IReadOnlyList<VirtualDiskAttachmentInfo> attachedCatlets)
        {
            Id = id;
            Name = name;
            Location = location;
            DataStore = dataStore;
            Project = project;
            Environment = environment;
            Path = path;
            SizeBytes = sizeBytes;
            ParentId = parentId;
            AttachedCatlets = attachedCatlets;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the location. </summary>
        public string Location { get; }
        /// <summary> Gets the data store. </summary>
        public string DataStore { get; }
        /// <summary> Gets the project. </summary>
        public string Project { get; }
        /// <summary> Gets the environment. </summary>
        public string Environment { get; }
        /// <summary> Gets the path. </summary>
        public string Path { get; }
        /// <summary> Gets the size bytes. </summary>
        public long? SizeBytes { get; }
        /// <summary> Gets the parent id. </summary>
        public string ParentId { get; }
        /// <summary> Gets the attached catlets. </summary>
        public IReadOnlyList<VirtualDiskAttachmentInfo> AttachedCatlets { get; }
    }
}
