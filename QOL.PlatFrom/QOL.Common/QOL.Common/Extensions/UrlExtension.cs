using System;
using System.Web.Mvc;

namespace QOL.Common.Extensions
{
    public static class UrlExtension
    {
        public static bool IsOpenXLiveUrl(this UrlHelper Url, string urlstr)
        {
            if (Url.IsLocalUrl(urlstr))
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(urlstr))
            {
                return false;
            }
            var uri = new Uri(urlstr);
            if (uri.Host.IsOpenXLiveHost())
                return true;
            return false;
        }

        public static bool IsOpenXLiveHost(this string hostname)
        {
            //TODO: 可能存在安全漏洞
            hostname = hostname.ToLower();
            return hostname.EndsWith(".openxlive.com") || hostname.Equals("openxlive.com")
                   || hostname.EndsWith(".openxlive.net") || hostname.Equals("openxlive.net")
                   || hostname.EndsWith(".openxlive.org") || hostname.Equals("openxlive.org")
                   || hostname.EndsWith(".openxliveoa.com") || hostname.Equals("openxliveoa.com");
        }
    }
}
