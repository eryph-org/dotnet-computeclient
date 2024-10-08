// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure;

namespace Eryph.ComputeClient.Models
{
    public partial class VirtualDiskAttachmentInfo
    {
        internal static VirtualDiskAttachmentInfo DeserializeVirtualDiskAttachmentInfo(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            CatletDriveType type = default;
            string catletId = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("type"u8))
                {
                    type = new CatletDriveType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("catlet_id"u8))
                {
                    catletId = property.Value.GetString();
                    continue;
                }
            }
            return new VirtualDiskAttachmentInfo(type, catletId);
        }

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static VirtualDiskAttachmentInfo FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeVirtualDiskAttachmentInfo(document.RootElement);
        }
    }
}
