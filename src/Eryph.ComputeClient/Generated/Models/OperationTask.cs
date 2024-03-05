// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Eryph.ComputeClient.Models
{
    /// <summary> The OperationTask. </summary>
    public partial class OperationTask
    {
        /// <summary> Initializes a new instance of <see cref="OperationTask"/>. </summary>
        internal OperationTask()
        {
        }

        /// <summary> Initializes a new instance of <see cref="OperationTask"/>. </summary>
        /// <param name="id"></param>
        /// <param name="parentTask"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="progress"></param>
        /// <param name="status"></param>
        /// <param name="reference"></param>
        internal OperationTask(string id, string parentTask, string name, string displayName, int? progress, OperationTaskStatus? status, OperationTaskReference reference)
        {
            Id = id;
            ParentTask = parentTask;
            Name = name;
            DisplayName = displayName;
            Progress = progress;
            Status = status;
            Reference = reference;
        }

        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the parent task. </summary>
        public string ParentTask { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the display name. </summary>
        public string DisplayName { get; }
        /// <summary> Gets the progress. </summary>
        public int? Progress { get; }
        /// <summary> Gets the status. </summary>
        public OperationTaskStatus? Status { get; }
        /// <summary> Gets the reference. </summary>
        public OperationTaskReference Reference { get; }
    }
}
