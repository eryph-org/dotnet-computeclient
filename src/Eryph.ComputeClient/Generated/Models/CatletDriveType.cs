// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Eryph.ComputeClient.Models
{
    /// <summary> The CatletDriveType. </summary>
    public readonly partial struct CatletDriveType : IEquatable<CatletDriveType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="CatletDriveType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public CatletDriveType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string VHDValue = "VHD";
        private const string SharedVHDValue = "SharedVHD";
        private const string PHDValue = "PHD";
        private const string DVDValue = "DVD";
        private const string VHDSetValue = "VHDSet";

        /// <summary> VHD. </summary>
        public static CatletDriveType VHD { get; } = new CatletDriveType(VHDValue);
        /// <summary> SharedVHD. </summary>
        public static CatletDriveType SharedVHD { get; } = new CatletDriveType(SharedVHDValue);
        /// <summary> PHD. </summary>
        public static CatletDriveType PHD { get; } = new CatletDriveType(PHDValue);
        /// <summary> DVD. </summary>
        public static CatletDriveType DVD { get; } = new CatletDriveType(DVDValue);
        /// <summary> VHDSet. </summary>
        public static CatletDriveType VHDSet { get; } = new CatletDriveType(VHDSetValue);
        /// <summary> Determines if two <see cref="CatletDriveType"/> values are the same. </summary>
        public static bool operator ==(CatletDriveType left, CatletDriveType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="CatletDriveType"/> values are not the same. </summary>
        public static bool operator !=(CatletDriveType left, CatletDriveType right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="CatletDriveType"/>. </summary>
        public static implicit operator CatletDriveType(string value) => new CatletDriveType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is CatletDriveType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(CatletDriveType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
