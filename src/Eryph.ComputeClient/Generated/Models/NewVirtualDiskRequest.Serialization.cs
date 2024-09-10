// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Eryph.ComputeClient.Models
{
    public partial class NewVirtualDiskRequest : IUtf8JsonSerializable
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
            writer.WritePropertyName("projectId"u8);
            writer.WriteStringValue(ProjectId);
            if (Name != null)
            {
                writer.WritePropertyName("name"u8);
                writer.WriteStringValue(Name);
            }
            else
            {
                writer.WriteNull("name");
            }
            if (Location != null)
            {
                writer.WritePropertyName("location"u8);
                writer.WriteStringValue(Location);
            }
            else
            {
                writer.WriteNull("location");
            }
            writer.WritePropertyName("size"u8);
            writer.WriteNumberValue(Size);
            if (Optional.IsDefined(Environment))
            {
                if (Environment != null)
                {
                    writer.WritePropertyName("environment"u8);
                    writer.WriteStringValue(Environment);
                }
                else
                {
                    writer.WriteNull("environment");
                }
            }
            if (Optional.IsDefined(Store))
            {
                if (Store != null)
                {
                    writer.WritePropertyName("store"u8);
                    writer.WriteStringValue(Store);
                }
                else
                {
                    writer.WriteNull("store");
                }
            }
            writer.WriteEndObject();
        }

        /// <summary> Convert into a <see cref="RequestContent"/>. </summary>
        internal virtual RequestContent ToRequestContent()
        {
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(this);
            return content;
        }
    }
}
