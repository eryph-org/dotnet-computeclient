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
        /// <summary> Initializes a new instance of <see cref="Models.Operation"/>. </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="statusMessage"></param>
        /// <param name="resources"></param>
        /// <param name="logEntries"></param>
        /// <param name="projects"></param>
        /// <param name="tasks"></param>
        /// <param name="result">
        /// Please note <see cref="OperationResult"/> is the base class. According to the scenario, a derived class of the base class might need to be assigned here, or this property needs to be casted to one of the possible derived classes.
        /// The available derived classes include <see cref="CatletConfigOperationResult"/>.
        /// </param>
        /// <returns> A new <see cref="Models.Operation"/> instance for mocking. </returns>
        public static Operation Operation(string id = null, OperationStatus status = default, string statusMessage = null, IEnumerable<OperationResource> resources = null, IEnumerable<OperationLogEntry> logEntries = null, IEnumerable<Project> projects = null, IEnumerable<OperationTask> tasks = null, OperationResult result = null)
        {
            resources ??= new List<OperationResource>();
            logEntries ??= new List<OperationLogEntry>();
            projects ??= new List<Project>();
            tasks ??= new List<OperationTask>();

            return new Operation(
                id,
                status,
                statusMessage,
                resources?.ToList(),
                logEntries?.ToList(),
                projects?.ToList(),
                tasks?.ToList(),
                result);
        }

        /// <summary> Initializes a new instance of <see cref="Models.OperationResource"/>. </summary>
        /// <param name="id"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceType"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> or <paramref name="resourceId"/> is null. </exception>
        /// <returns> A new <see cref="Models.OperationResource"/> instance for mocking. </returns>
        public static OperationResource OperationResource(string id = null, string resourceId = null, ResourceType resourceType = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            return new OperationResource(id, resourceId, resourceType);
        }

        /// <summary> Initializes a new instance of <see cref="Models.OperationLogEntry"/>. </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <param name="message"></param>
        /// <param name="timestamp"></param>
        /// <returns> A new <see cref="Models.OperationLogEntry"/> instance for mocking. </returns>
        public static OperationLogEntry OperationLogEntry(string id = null, string taskId = null, string message = null, DateTimeOffset timestamp = default)
        {
            return new OperationLogEntry(id, taskId, message, timestamp);
        }

        /// <summary> Initializes a new instance of <see cref="Models.Project"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="tenantId"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="name"/> or <paramref name="tenantId"/> is null. </exception>
        /// <returns> A new <see cref="Models.Project"/> instance for mocking. </returns>
        public static Project Project(string id = null, string name = null, string tenantId = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (tenantId == null)
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            return new Project(id, name, tenantId);
        }

        /// <summary> Initializes a new instance of <see cref="Models.OperationTask"/>. </summary>
        /// <param name="id"></param>
        /// <param name="parentTaskId"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="progress"></param>
        /// <param name="status"></param>
        /// <param name="reference"></param>
        /// <returns> A new <see cref="Models.OperationTask"/> instance for mocking. </returns>
        public static OperationTask OperationTask(string id = null, string parentTaskId = null, string name = null, string displayName = null, int progress = default, OperationTaskStatus status = default, OperationTaskReference reference = null)
        {
            return new OperationTask(
                id,
                parentTaskId,
                name,
                displayName,
                progress,
                status,
                reference);
        }

        /// <summary> Initializes a new instance of <see cref="Models.OperationTaskReference"/>. </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> or <paramref name="projectName"/> is null. </exception>
        /// <returns> A new <see cref="Models.OperationTaskReference"/> instance for mocking. </returns>
        public static OperationTaskReference OperationTaskReference(string id = null, TaskReferenceType type = default, string projectName = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (projectName == null)
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            return new OperationTaskReference(id, type, projectName);
        }

        /// <summary> Initializes a new instance of <see cref="Models.Gene"/>. </summary>
        /// <param name="id"></param>
        /// <param name="geneType"></param>
        /// <param name="geneSet"></param>
        /// <param name="name"></param>
        /// <param name="architecture"></param>
        /// <param name="size"></param>
        /// <param name="hash"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="geneSet"/>, <paramref name="name"/>, <paramref name="architecture"/> or <paramref name="hash"/> is null. </exception>
        /// <returns> A new <see cref="Models.Gene"/> instance for mocking. </returns>
        public static Gene Gene(string id = null, GeneType geneType = default, string geneSet = null, string name = null, string architecture = null, long size = default, string hash = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (geneSet == null)
            {
                throw new ArgumentNullException(nameof(geneSet));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (architecture == null)
            {
                throw new ArgumentNullException(nameof(architecture));
            }
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            return new Gene(
                id,
                geneType,
                geneSet,
                name,
                architecture,
                size,
                hash);
        }

        /// <summary> Initializes a new instance of <see cref="Models.VirtualDisk"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="dataStore"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="gene"></param>
        /// <param name="path">
        /// The file system path of the virtual disk. This information
        /// is only available to administrators.
        /// </param>
        /// <param name="sizeBytes"></param>
        /// <param name="parentId"> The ID of the parent disk when this disk is a differential disk. </param>
        /// <param name="attachedCatlets"></param>
        /// <returns> A new <see cref="Models.VirtualDisk"/> instance for mocking. </returns>
        public static VirtualDisk VirtualDisk(string id = null, string name = null, string location = null, string dataStore = null, Project project = null, string environment = null, VirtualDiskGeneInfo gene = null, string path = null, long? sizeBytes = null, string parentId = null, IEnumerable<VirtualDiskAttachedCatlet> attachedCatlets = null)
        {
            attachedCatlets ??= new List<VirtualDiskAttachedCatlet>();

            return new VirtualDisk(
                id,
                name,
                location,
                dataStore,
                project,
                environment,
                gene,
                path,
                sizeBytes,
                parentId,
                attachedCatlets?.ToList());
        }

        /// <summary> Initializes a new instance of <see cref="Models.VirtualDiskGeneInfo"/>. </summary>
        /// <param name="geneSet"></param>
        /// <param name="name"></param>
        /// <param name="architecture"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="geneSet"/>, <paramref name="name"/> or <paramref name="architecture"/> is null. </exception>
        /// <returns> A new <see cref="Models.VirtualDiskGeneInfo"/> instance for mocking. </returns>
        public static VirtualDiskGeneInfo VirtualDiskGeneInfo(string geneSet = null, string name = null, string architecture = null)
        {
            if (geneSet == null)
            {
                throw new ArgumentNullException(nameof(geneSet));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (architecture == null)
            {
                throw new ArgumentNullException(nameof(architecture));
            }

            return new VirtualDiskGeneInfo(geneSet, name, architecture);
        }

        /// <summary> Initializes a new instance of <see cref="Models.VirtualDiskAttachedCatlet"/>. </summary>
        /// <param name="type"></param>
        /// <param name="catletId"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="catletId"/> is null. </exception>
        /// <returns> A new <see cref="Models.VirtualDiskAttachedCatlet"/> instance for mocking. </returns>
        public static VirtualDiskAttachedCatlet VirtualDiskAttachedCatlet(CatletDriveType type = default, string catletId = null)
        {
            if (catletId == null)
            {
                throw new ArgumentNullException(nameof(catletId));
            }

            return new VirtualDiskAttachedCatlet(type, catletId);
        }

        /// <summary> Initializes a new instance of <see cref="Models.ProjectMemberRole"/>. </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <param name="memberId"></param>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="project"/>, <paramref name="memberId"/>, <paramref name="roleId"/> or <paramref name="roleName"/> is null. </exception>
        /// <returns> A new <see cref="Models.ProjectMemberRole"/> instance for mocking. </returns>
        public static ProjectMemberRole ProjectMemberRole(string id = null, Project project = null, string memberId = null, string roleId = null, string roleName = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if (memberId == null)
            {
                throw new ArgumentNullException(nameof(memberId));
            }
            if (roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            return new ProjectMemberRole(id, project, memberId, roleId, roleName);
        }

        /// <summary> Initializes a new instance of <see cref="Models.Catlet"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="project"></param>
        /// <param name="status"></param>
        /// <param name="networks"></param>
        /// <param name="networkAdapters"></param>
        /// <param name="drives"></param>
        /// <returns> A new <see cref="Models.Catlet"/> instance for mocking. </returns>
        public static Catlet Catlet(string id = null, string name = null, Project project = null, CatletStatus status = default, IEnumerable<CatletNetwork> networks = null, IEnumerable<CatletNetworkAdapter> networkAdapters = null, IEnumerable<CatletDrive> drives = null)
        {
            networks ??= new List<CatletNetwork>();
            networkAdapters ??= new List<CatletNetworkAdapter>();
            drives ??= new List<CatletDrive>();

            return new Catlet(
                id,
                name,
                project,
                status,
                networks?.ToList(),
                networkAdapters?.ToList(),
                drives?.ToList());
        }

        /// <summary> Initializes a new instance of <see cref="Models.CatletNetwork"/>. </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="ipV4Addresses"></param>
        /// <param name="iPv4DefaultGateway"></param>
        /// <param name="dnsServerAddresses"></param>
        /// <param name="ipV4Subnets"></param>
        /// <param name="floatingPort"></param>
        /// <returns> A new <see cref="Models.CatletNetwork"/> instance for mocking. </returns>
        public static CatletNetwork CatletNetwork(string name = null, string provider = null, IEnumerable<string> ipV4Addresses = null, string iPv4DefaultGateway = null, IEnumerable<string> dnsServerAddresses = null, IEnumerable<string> ipV4Subnets = null, FloatingNetworkPort floatingPort = null)
        {
            ipV4Addresses ??= new List<string>();
            dnsServerAddresses ??= new List<string>();
            ipV4Subnets ??= new List<string>();

            return new CatletNetwork(
                name,
                provider,
                ipV4Addresses?.ToList(),
                iPv4DefaultGateway,
                dnsServerAddresses?.ToList(),
                ipV4Subnets?.ToList(),
                floatingPort);
        }

        /// <summary> Initializes a new instance of <see cref="Models.FloatingNetworkPort"/>. </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="subnet"></param>
        /// <param name="ipV4Addresses"></param>
        /// <param name="ipV4Subnets"></param>
        /// <returns> A new <see cref="Models.FloatingNetworkPort"/> instance for mocking. </returns>
        public static FloatingNetworkPort FloatingNetworkPort(string name = null, string provider = null, string subnet = null, IEnumerable<string> ipV4Addresses = null, IEnumerable<string> ipV4Subnets = null)
        {
            ipV4Addresses ??= new List<string>();
            ipV4Subnets ??= new List<string>();

            return new FloatingNetworkPort(name, provider, subnet, ipV4Addresses?.ToList(), ipV4Subnets?.ToList());
        }

        /// <summary> Initializes a new instance of <see cref="Models.CatletNetworkAdapter"/>. </summary>
        /// <param name="name"></param>
        /// <param name="macAddress"></param>
        /// <returns> A new <see cref="Models.CatletNetworkAdapter"/> instance for mocking. </returns>
        public static CatletNetworkAdapter CatletNetworkAdapter(string name = null, string macAddress = null)
        {
            return new CatletNetworkAdapter(name, macAddress);
        }

        /// <summary> Initializes a new instance of <see cref="Models.CatletDrive"/>. </summary>
        /// <param name="type"></param>
        /// <param name="attachedDiskId">
        /// The ID of the actual virtual disk which is attached.
        /// This can be null, e.g. when the VHD has been deleted,
        /// but it is still configured in the virtual machine.
        /// </param>
        /// <returns> A new <see cref="Models.CatletDrive"/> instance for mocking. </returns>
        public static CatletDrive CatletDrive(CatletDriveType type = default, string attachedDiskId = null)
        {
            return new CatletDrive(type, attachedDiskId);
        }

        /// <summary> Initializes a new instance of <see cref="Models.GeneWithUsage"/>. </summary>
        /// <param name="id"></param>
        /// <param name="geneType"></param>
        /// <param name="geneSet"></param>
        /// <param name="name"></param>
        /// <param name="architecture"></param>
        /// <param name="size"></param>
        /// <param name="hash"></param>
        /// <param name="catlets"></param>
        /// <param name="disks"></param>
        /// <returns> A new <see cref="Models.GeneWithUsage"/> instance for mocking. </returns>
        public static GeneWithUsage GeneWithUsage(string id = null, GeneType geneType = default, string geneSet = null, string name = null, string architecture = null, long size = default, string hash = null, IEnumerable<string> catlets = null, IEnumerable<string> disks = null)
        {
            catlets ??= new List<string>();
            disks ??= new List<string>();

            return new GeneWithUsage(
                id,
                geneType,
                geneSet,
                name,
                architecture,
                size,
                hash,
                catlets?.ToList(),
                disks?.ToList());
        }

        /// <summary> Initializes a new instance of <see cref="Models.VirtualNetwork"/>. </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="project"></param>
        /// <param name="environment"></param>
        /// <param name="providerName"></param>
        /// <param name="ipNetwork"></param>
        /// <returns> A new <see cref="Models.VirtualNetwork"/> instance for mocking. </returns>
        public static VirtualNetwork VirtualNetwork(string id = null, string name = null, Project project = null, string environment = null, string providerName = null, string ipNetwork = null)
        {
            return new VirtualNetwork(
                id,
                name,
                project,
                environment,
                providerName,
                ipNetwork);
        }

        /// <summary> Initializes a new instance of <see cref="Models.CatletConfiguration"/>. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <returns> A new <see cref="Models.CatletConfiguration"/> instance for mocking. </returns>
        public static CatletConfiguration CatletConfiguration(JsonElement configuration = default)
        {
            return new CatletConfiguration(configuration);
        }

        /// <summary> Initializes a new instance of <see cref="Models.VirtualNetworkConfiguration"/>. </summary>
        /// <param name="configuration"> Anything. </param>
        /// <returns> A new <see cref="Models.VirtualNetworkConfiguration"/> instance for mocking. </returns>
        public static VirtualNetworkConfiguration VirtualNetworkConfiguration(JsonElement configuration = default)
        {
            return new VirtualNetworkConfiguration(configuration);
        }

        /// <summary> Initializes a new instance of <see cref="Models.CatletConfigValidationResult"/>. </summary>
        /// <param name="isValid"> Indicates whether the catlet configuration is valid. </param>
        /// <param name="errors"> Contains a list of the issues when the configuration is invalid. </param>
        /// <returns> A new <see cref="Models.CatletConfigValidationResult"/> instance for mocking. </returns>
        public static CatletConfigValidationResult CatletConfigValidationResult(bool isValid = default, IEnumerable<ValidationIssue> errors = null)
        {
            errors ??= new List<ValidationIssue>();

            return new CatletConfigValidationResult(isValid, errors?.ToList());
        }

        /// <summary> Initializes a new instance of <see cref="Models.ValidationIssue"/>. </summary>
        /// <param name="member">
        /// The JSON path which identifies the member which has the issue.
        /// Can be null when the issue is not related to
        /// a specific member.
        /// </param>
        /// <param name="message"> The details of the issue. </param>
        /// <returns> A new <see cref="Models.ValidationIssue"/> instance for mocking. </returns>
        public static ValidationIssue ValidationIssue(string member = null, string message = null)
        {
            return new ValidationIssue(member, message);
        }
    }
}
