// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The CatletStatus. </summary>
    public readonly partial struct CatletStatus : IEquatable<CatletStatus>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="CatletStatus"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public CatletStatus(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string StoppedValue = "Stopped";
        private const string RunningValue = "Running";
        private const string PendingValue = "Pending";
        private const string ErrorValue = "Error";

        /// <summary> Stopped. </summary>
        public static CatletStatus Stopped { get; } = new CatletStatus(StoppedValue);
        /// <summary> Running. </summary>
        public static CatletStatus Running { get; } = new CatletStatus(RunningValue);
        /// <summary> Pending. </summary>
        public static CatletStatus Pending { get; } = new CatletStatus(PendingValue);
        /// <summary> Error. </summary>
        public static CatletStatus Error { get; } = new CatletStatus(ErrorValue);
        /// <summary> Determines if two <see cref="CatletStatus"/> values are the same. </summary>
        public static bool operator ==(CatletStatus left, CatletStatus right) => left.Equals(right);
        /// <summary> Determines if two <see cref="CatletStatus"/> values are not the same. </summary>
        public static bool operator !=(CatletStatus left, CatletStatus right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="CatletStatus"/>. </summary>
        public static implicit operator CatletStatus(string value) => new CatletStatus(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is CatletStatus other && Equals(other);
        /// <inheritdoc />
        public bool Equals(CatletStatus other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}