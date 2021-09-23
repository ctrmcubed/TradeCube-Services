﻿using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class BoxResultResponse
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        public BoxResultEnvelope Envelope { get; set; }
    }
}