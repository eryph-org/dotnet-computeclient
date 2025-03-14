// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The CatletStopMode. </summary>
    public readonly partial struct CatletStopMode : IEquatable<CatletStopMode>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="CatletStopMode"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public CatletStopMode(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ShutdownValue = "Shutdown";
        private const string HardValue = "Hard";
        private const string KillValue = "Kill";

        /// <summary> Shutdown. </summary>
        public static CatletStopMode Shutdown { get; } = new CatletStopMode(ShutdownValue);
        /// <summary> Hard. </summary>
        public static CatletStopMode Hard { get; } = new CatletStopMode(HardValue);
        /// <summary> Kill. </summary>
        public static CatletStopMode Kill { get; } = new CatletStopMode(KillValue);
        /// <summary> Determines if two <see cref="CatletStopMode"/> values are the same. </summary>
        public static bool operator ==(CatletStopMode left, CatletStopMode right) => left.Equals(right);
        /// <summary> Determines if two <see cref="CatletStopMode"/> values are not the same. </summary>
        public static bool operator !=(CatletStopMode left, CatletStopMode right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="CatletStopMode"/>. </summary>
        public static implicit operator CatletStopMode(string value) => new CatletStopMode(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is CatletStopMode other && Equals(other);
        /// <inheritdoc />
        public bool Equals(CatletStopMode other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
