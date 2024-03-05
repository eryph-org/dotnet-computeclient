// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Eryph.ComputeClient;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualDisk. </summary>
    public partial class VirtualDisk
    {
        /// <summary> Initializes a new instance of <see cref="VirtualDisk"/>. </summary>
        internal VirtualDisk()
        {
            AttachedDrives = new ChangeTrackingList<CatletDrive>();
        }

        /// <summary> Initializes a new instance of <see cref="VirtualDisk"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="storageIdentifier"></param>
        /// <param name="dataStore"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="path"></param>
        /// <param name="sizeBytes"></param>
        /// <param name="parentId"></param>
        /// <param name="attachedDrives"></param>
        internal VirtualDisk(Guid? id, string name, string storageIdentifier, string dataStore, string project, string environment, string path, long? sizeBytes, Guid? parentId, IReadOnlyList<CatletDrive> attachedDrives)
        {
            Id = id;
            Name = name;
            StorageIdentifier = storageIdentifier;
            DataStore = dataStore;
            Project = project;
            Environment = environment;
            Path = path;
            SizeBytes = sizeBytes;
            ParentId = parentId;
            AttachedDrives = attachedDrives;
        }

        /// <summary> Gets the id. </summary>
        public Guid? Id { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the storage identifier. </summary>
        public string StorageIdentifier { get; }
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
        public Guid? ParentId { get; }
        /// <summary> Gets the attached drives. </summary>
        public IReadOnlyList<CatletDrive> AttachedDrives { get; }
    }
}
