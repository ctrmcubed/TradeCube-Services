using System.Collections.Generic;
using System.Text.Json.Serialization;
using Shared.Constants;

namespace Shared.Messages
{
    public class ApiResponse
    {
        [JsonIgnore]
        public int? StatusCode { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Status { get; init; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Warning { get; init; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ErrorCount { get; init; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Errors { get; init; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Warnings { get; init; }
        
        public bool IsSuccess() => 
            Status == ApiConstants.SuccessResult;
    }
}