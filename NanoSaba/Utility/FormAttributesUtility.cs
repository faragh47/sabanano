using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NanoSaba.Utility;

public static class FormAttributesUtility
{
    public static bool GetBoolByKey(string key, HttpRequest request)
    {
        var result = !(string.IsNullOrEmpty(request.Form[key].ToString())
            || request.Form[key].ToString() == "false" );
        return result;
    }
    public static double GetDoubleByKey(string key, HttpRequest request)
    {
        var result= string.IsNullOrEmpty(request.Form[key].ToString())
            ? 0
            : Convert.ToDouble(request.Form[key].ToString());
        return result;
    }
    public static long GetLongByKey(string key, HttpRequest request)
    {
        var result=string.IsNullOrEmpty(request.Form[key].ToString())
            ? 0
            : Convert.ToInt64(request.Form[key].ToString());
        return result;
    }
    
    public static decimal GeDecimalByKey(string key, HttpRequest request)
    {
        var result=string.IsNullOrEmpty(request.Form[key].ToString())
            ? 0
            : Convert.ToDecimal(request.Form[key].ToString());
        return result;
    }
}