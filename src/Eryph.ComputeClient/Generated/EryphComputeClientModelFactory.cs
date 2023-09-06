// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Eryph.ComputeClient.Models
{
    /// <summary> Model factory for models. </summary>
    public static partial class EryphComputeClientModelFactory
    {
        /// <summary> Initializes a new instance of Operation. </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="statusMessage"></param>
        /// <param name="resources"></param>
        /// <param name="logEntries"></param>
        /// <param name="projects"></param>
        /// <returns> A new <see cref="Models.Operation"/> instance for mocking. </returns>
        public static Operation Operation(string id = null, OperationStatus? status = null, string statusMessage = null, IEnumerable<OperationResource> resources = null, IEnumerable<OperationLogEntry> logEntries = null, IEnumerable<Project> projects = null)
        {
            resources ??= new List<OperationResource>();
            logEntries ??= new List<OperationLogEntry>();
            projects ??= new List<Project>();

            return new Operation(id, status, statusMessage, resources?.ToList(), logEntries?.ToList(), projects?.ToList());
        }

        /// <summary> Initializes a new instance of OperationResource. </summary>
        /// <param name="id"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceType"></param>
        /// <returns> A new <see cref="Models.OperationResource"/> instance for mocking. </returns>
        public static OperationResource OperationResource(string id = null, string resourceId = null, ResourceType? resourceType = null)
        {
            return new OperationResource(id, resourceId, resourceType);
        }

        /// <summary> Initializes a new instance of OperationLogEntry. </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="timestamp"></param>
        /// <returns> A new <see cref="Models.OperationLogEntry"/> instance for mocking. </returns>
        public static OperationLogEntry OperationLogEntry(string id = null, string message = null, DateTimeOffset? timestamp = null)
        {
            return new OperationLogEntry(id, message, timestamp);
        }

        /// <summary> Initializes a new instance of Project. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="tenantId"></param>
        /// <returns> A new <see cref="Models.Project"/> instance for mocking. </returns>
        public static Project Project(string id = null, string name = null, string tenantId = null)
        {
            return new Project(id, name, tenantId);
        }

        /// <summary> Initializes a new instance of Catlet. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="networks"></param>
        /// <param name="networkAdapters"></param>
        /// <param name="drives"></param>
        /// <returns> A new <see cref="Models.Catlet"/> instance for mocking. </returns>
        public static Catlet Catlet(string id = null, string name = null, CatletStatus? status = null, IEnumerable<CatletNetwork> networks = null, IEnumerable<CatletNetworkAdapter> networkAdapters = null, IEnumerable<CatletDrive> drives = null)
        {
            networks ??= new List<CatletNetwork>();
            networkAdapters ??= new List<CatletNetworkAdapter>();
            drives ??= new List<CatletDrive>();

            return new Catlet(id, name, status, networks?.ToList(), networkAdapters?.ToList(), drives?.ToList());
        }

        /// <summary> Initializes a new instance of CatletNetwork. </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="ipV4Addresses"></param>
        /// <param name="ipV6Addresses"></param>
        /// <param name="iPv4DefaultGateway"></param>
        /// <param name="iPv6DefaultGateway"></param>
        /// <param name="dnsServerAddresses"></param>
        /// <param name="ipV4Subnets"></param>
        /// <param name="ipV6Subnets"></param>
        /// <returns> A new <see cref="Models.CatletNetwork"/> instance for mocking. </returns>
        public static CatletNetwork CatletNetwork(string name = null, string provider = null, IEnumerable<string> ipV4Addresses = null, IEnumerable<string> ipV6Addresses = null, string iPv4DefaultGateway = null, string iPv6DefaultGateway = null, IEnumerable<string> dnsServerAddresses = null, IEnumerable<string> ipV4Subnets = null, IEnumerable<string> ipV6Subnets = null)
        {
            ipV4Addresses ??= new List<string>();
            ipV6Addresses ??= new List<string>();
            dnsServerAddresses ??= new List<string>();
            ipV4Subnets ??= new List<string>();
            ipV6Subnets ??= new List<string>();

            return new CatletNetwork(name, provider, ipV4Addresses?.ToList(), ipV6Addresses?.ToList(), iPv4DefaultGateway, iPv6DefaultGateway, dnsServerAddresses?.ToList(), ipV4Subnets?.ToList(), ipV6Subnets?.ToList());
        }

        /// <summary> Initializes a new instance of CatletNetworkAdapter. </summary>
        /// <param name="name"></param>
        /// <param name="macAddress"></param>
        /// <returns> A new <see cref="Models.CatletNetworkAdapter"/> instance for mocking. </returns>
        public static CatletNetworkAdapter CatletNetworkAdapter(string name = null, string macAddress = null)
        {
            return new CatletNetworkAdapter(name, macAddress);
        }

        /// <summary> Initializes a new instance of CatletDrive. </summary>
        /// <param name="type"></param>
        /// <param name="attachedDiskId"></param>
        /// <returns> A new <see cref="Models.CatletDrive"/> instance for mocking. </returns>
        public static CatletDrive CatletDrive(CatletDriveType? type = null, Guid? attachedDiskId = null)
        {
            return new CatletDrive(type, attachedDiskId);
        }

        /// <summary> Initializes a new instance of VirtualDisk. </summary>
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
        /// <returns> A new <see cref="Models.VirtualDisk"/> instance for mocking. </returns>
        public static VirtualDisk VirtualDisk(Guid? id = null, string name = null, string storageIdentifier = null, string dataStore = null, string project = null, string environment = null, string path = null, long? sizeBytes = null, Guid? parentId = null, IEnumerable<CatletDrive> attachedDrives = null)
        {
            attachedDrives ??= new List<CatletDrive>();

            return new VirtualDisk(id, name, storageIdentifier, dataStore, project, environment, path, sizeBytes, parentId, attachedDrives?.ToList());
        }

        /// <summary> Initializes a new instance of VirtualNetwork. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="projectId"></param>
        /// <param name="projectName"></param>
        /// <param name="tenantId"></param>
        /// <param name="providerName"></param>
        /// <param name="ipNetwork"></param>
        /// <returns> A new <see cref="Models.VirtualNetwork"/> instance for mocking. </returns>
        public static VirtualNetwork VirtualNetwork(string id = null, string name = null, string projectId = null, string projectName = null, string tenantId = null, string providerName = null, string ipNetwork = null)
        {
            return new VirtualNetwork(id, name, projectId, projectName, tenantId, providerName, ipNetwork);
        }

        /// <summary> Initializes a new instance of CatletConfiguration. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <returns> A new <see cref="Models.CatletConfiguration"/> instance for mocking. </returns>
        public static CatletConfiguration CatletConfiguration(JsonElement configuration = default)
        {
            return new CatletConfiguration(configuration);
        }

        /// <summary> Initializes a new instance of VirtualNetworkConfiguration. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <returns> A new <see cref="Models.VirtualNetworkConfiguration"/> instance for mocking. </returns>
        public static VirtualNetworkConfiguration VirtualNetworkConfiguration(JsonElement configuration = default)
        {
            return new VirtualNetworkConfiguration(configuration);
        }
    }
}
