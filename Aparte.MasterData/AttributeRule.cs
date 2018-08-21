using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Aparte.MasterData
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttributeRule : long
    {
        None = 0,
        IsUniqe = 1,
        UsePreset = 2
    }
}
