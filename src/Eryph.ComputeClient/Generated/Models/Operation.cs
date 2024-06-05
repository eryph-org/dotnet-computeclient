// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The Operation. </summary>
    public partial class Operation
    {
        /// <summary> Initializes a new instance of <see cref="Operation"/>. </summary>
        internal Operation()
        {
            Resources = new ChangeTrackingList<OperationResource>();
            LogEntries = new ChangeTrackingList<OperationLogEntry>();
            Projects = new ChangeTrackingList<Project>();
            Tasks = new ChangeTrackingList<OperationTask>();
        }

        /// <summary> Initializes a new instance of <see cref="Operation"/>. </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="statusMessage"></param>
        /// <param name="resources"></param>
        /// <param name="logEntries"></param>
        /// <param name="projects"></param>
        /// <param name="tasks"></param>
        internal Operation(string id, OperationStatus? status, string statusMessage, IReadOnlyList<OperationResource> resources, IReadOnlyList<OperationLogEntry> logEntries, IReadOnlyList<Project> projects, IReadOnlyList<OperationTask> tasks)
        {
            Id = id;
            Status = status;
            StatusMessage = statusMessage;
            Resources = resources;
            LogEntries = logEntries;
            Projects = projects;
            Tasks = tasks;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the status. </summary>
        public OperationStatus? Status { get; }
        /// <summary> Gets the status message. </summary>
        public string StatusMessage { get; }
        /// <summary> Gets the resources. </summary>
        public IReadOnlyList<OperationResource> Resources { get; }
        /// <summary> Gets the log entries. </summary>
        public IReadOnlyList<OperationLogEntry> LogEntries { get; }
        /// <summary> Gets the projects. </summary>
        public IReadOnlyList<Project> Projects { get; }
        /// <summary> Gets the tasks. </summary>
        public IReadOnlyList<OperationTask> Tasks { get; }
    }
}
