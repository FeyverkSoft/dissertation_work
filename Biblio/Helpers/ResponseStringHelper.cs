using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Biblio.Helpers
{


    public class ResponseStringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiUrl">Url</param>
        /// <param name="method">Тип метода</param>
        /// <param name="requestBody">Данные</param>
        /// <param name="contentType">Тип данных</param>
        /// <param name="httpStatus">Статус</param>
        /// <returns></returns>
        public String GetResponseString(String apiUrl, String requestBody, out Int32 httpStatus, String method = "GET", String contentType = "application/json", IDictionary<String, String> headers = null)
        {
            httpStatus = 500;
            WebException exception = null;
            var request = (HttpWebRequest)WebRequest.Create(method.ToUpper() == "GET" ? apiUrl + requestBody : apiUrl);
            request.ContentType = contentType ?? "application/json";
            request.Accept = contentType ?? "application/json";
            request.Timeout = (60*1000)*4;
            request.ReadWriteTimeout = (60 * 1000) * 10;
            if (headers != null)
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }

            request.Method = method.ToUpper();
            switch (method.ToUpper())
            {
                case "GET":
                    try
                    {
                        using (var resp = request.GetResponse())
                        {
                            httpStatus = 200;
                            using (var stream = resp.GetResponseStream())
                            {
                                if (stream != null)
                                    using (var sr = new StreamReader(stream))
                                    {
                                        return sr.ReadToEnd();
                                    }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        exception = ex;
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            httpStatus = (int)response.StatusCode;
                        }
                        else httpStatus = 0;
                    }
                    break;
                case "POST":
                    var postData = Encoding.UTF8.GetBytes(requestBody);
                    request.ContentLength = postData.Length;
                    try
                    {
                        using (var stream = request.GetRequestStream())
                            stream.Write(postData, 0, postData.Length);
                        String response;
                        using (var webResponse = request.GetResponse())
                        using (var streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                            response = streamReader.ReadToEnd();
                        httpStatus = 200;
                        return response;
                    }
                    catch (WebException ex)
                    {
                        exception = ex;
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            httpStatus = (int)response.StatusCode;
                        }
                        else httpStatus = 0;
                        using (var streamReader = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
            }
            var log = new
            {
                @UnitpayService = new
                {
                    @Error = new
                    {
                        @HttpStatus = httpStatus,
                        @UserError = "Адрес временно недоступен, или вернул неверный ответ",
                        @Exception = exception
                    },
                    @Request = new
                    {
                        @ApiUrl = apiUrl,
                        @RequestBody = requestBody,
                        @Method = method.ToUpper(),
                        @ContentType = contentType ?? "application/json",
                        @HttpStatus = 544
                    }
                }
            };
            throw new Exception(log.ToJsonExtended());
        }

        public string GetResponseString(string apiUrl, string requestBody, string method, ref string cookies, ref string referer, string userAgent, string connection, string acceptLanguage, string contentType = null, string host = null)
        {
            int httpStatus = 500;
            WebException exception = null;
            var request = (HttpWebRequest)WebRequest.Create(method.ToUpper() == "GET" ? apiUrl + '?' + requestBody : apiUrl);
            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            request.Method = method.ToUpper();
            if (!string.IsNullOrEmpty(host))
            {
                request.Host = host;
            }
            request.Referer = referer;
            request.UserAgent = userAgent;
            if (connection == "keep-alive")
            {
                request.KeepAlive = true;
            }
            if (!string.IsNullOrEmpty(cookies))
            {
                request.Headers["Cookie"] = cookies;
            }
            request.Headers["Accept-Language"] = acceptLanguage;
            switch (method.ToUpper())
            {
                case "GET":
                    try
                    {
                        using (var resp = request.GetResponse())
                        {
                            httpStatus = 200;
                            using (var stream = resp.GetResponseStream())
                            {
                                if (stream != null)
                                    using (var sr = new StreamReader(stream))
                                    {
                                        cookies = string.IsNullOrEmpty(resp.Headers["Set-Cookie"]) ? cookies : resp.Headers["Set-Cookie"];
                                        referer = resp.ResponseUri.ToString();
                                        return sr.ReadToEnd();
                                    }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        exception = ex;
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            httpStatus = (int)response.StatusCode;
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream != null)
                                    using (var sr = new StreamReader(stream))
                                    {
                                        return sr.ReadToEnd();
                                    }
                            }
                        }
                        else httpStatus = 0;
                    }
                    break;
                case "POST":
                    var postData = Encoding.UTF8.GetBytes(requestBody);
                    request.ContentLength = postData.Length;
                    try
                    {
                        using (var stream = request.GetRequestStream())
                            stream.Write(postData, 0, postData.Length);
                        String response;
                        using (var webResponse = request.GetResponse())
                        using (var streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            cookies = string.IsNullOrEmpty(webResponse.Headers["Set-Cookie"]) ? cookies : webResponse.Headers["Set-Cookie"];
                            referer = webResponse.ResponseUri.ToString();
                            response = streamReader.ReadToEnd();
                        }
                        httpStatus = 200;
                        return response;
                    }
                    catch (WebException ex)
                    {
                        exception = ex;
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            httpStatus = (int)response.StatusCode;
                        }
                        else httpStatus = 0;
                        using (var streamReader = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
            }
            var log = new
            {
                @UnitpayService = new
                {
                    @Error = new
                    {
                        @HttpStatus = httpStatus,
                        @UserError = "Адрес временно недоступен, или вернул неверный ответ",
                        @Exception = exception
                    },
                    @Request = new
                    {
                        @ApiUrl = apiUrl,
                        @RequestBody = requestBody,
                        @Method = method.ToUpper(),
                        @ContentType = contentType,
                        @Cookie = cookies,
                        @Referer = referer,
                        @UserAgent = userAgent,
                        @HttpStatus = 544
                    }
                }
            };
            throw new Exception(log.ToJsonExtended());
        }
    }
}