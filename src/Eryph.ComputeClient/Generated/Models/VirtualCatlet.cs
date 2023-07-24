// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The VirtualCatlet. </summary>
    public partial class VirtualCatlet
    {
        /// <summary> Initializes a new instance of VirtualCatlet. </summary>
        internal VirtualCatlet()
        {
            Networks = new ChangeTrackingList<CatletNetwork>();
            NetworkAdapters = new ChangeTrackingList<VirtualCatletNetworkAdapter>();
            Drives = new ChangeTrackingList<VirtualCatletDrive>();
        }

        /// <summary> Initializes a new instance of VirtualCatlet. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="networks"></param>
        /// <param name="networkAdapters"></param>
        /// <param name="drives"></param>
        internal VirtualCatlet(string id, string name, CatletStatus? status, IReadOnlyList<CatletNetwork> networks, IReadOnlyList<VirtualCatletNetworkAdapter> networkAdapters, IReadOnlyList<VirtualCatletDrive> drives)
        {
            Id = id;
            Name = name;
            Status = status;
            Networks = networks;
            NetworkAdapters = networkAdapters;
            Drives = drives;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the status. </summary>
        public CatletStatus? Status { get; }
        /// <summary> Gets the networks. </summary>
        public IReadOnlyList<CatletNetwork> Networks { get; }
        /// <summary> Gets the network adapters. </summary>
        public IReadOnlyList<VirtualCatletNetworkAdapter> NetworkAdapters { get; }
        /// <summary> Gets the drives. </summary>
        public IReadOnlyList<VirtualCatletDrive> Drives { get; }
    }
}