using AC.Library.Models;
using AC.Library.Models.Communication;

namespace AC.Library.Utils;

internal static class ResponseChecker
{
    internal static bool IsReponsePackInfoValid(ResponsePackInfo responsePackInfo)
    {
        if (responsePackInfo == null)
        {
            return false;
        }
        if (responsePackInfo.Type != "pack")
        {
            return false;
        }
        
        return responsePackInfo.Pack != null;
    }
}