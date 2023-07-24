// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    public partial class UpdateProjectBody : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(CorrelationId))
            {
                if (CorrelationId != null)
                {
                    writer.WritePropertyName("correlationId"u8);
                    writer.WriteStringValue(CorrelationId.Value);
                }
                else
                {
                    writer.WriteNull("correlationId");
                }
            }
            if (Optional.IsDefined(Name))
            {
                if (Name != null)
                {
                    writer.WritePropertyName("name"u8);
                    writer.WriteStringValue(Name);
                }
                else
                {
                    writer.WriteNull("name");
                }
            }
            writer.WriteEndObject();
        }
    }
}
