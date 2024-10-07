using System.Runtime.Serialization;

namespace WebXeDapAPI.Models.Enum
{
    public enum StatusToken
    {
        [EnumMember(Value = "Valid")]
        Valid,
        [EnumMember(Value = "Expired")]
        Expired
    }
}
