using System.Web.Mvc;

namespace CapStonePhase2.CustomHelpers
{
    public static class HtmlCustomHelper
    {
        public static MvcHtmlString BoolToString(this HtmlHelper helper, bool condition)
        {
            var result = condition ? "Yes" : "No";
            return new MvcHtmlString(result);
        }
    }
}