using System.ComponentModel;

namespace ExtensionsLib.Enums
{
    public enum ClaimsEnum
    {
        [Description("All Claims")]
        AllClaims = 1,
        [Description("Open Claims")]
        OpenClaims = 2,
        [Description("Closed Claims")]
        ClosedClaims = 3
    }
}
