using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Collections.Generic;
using QOL.Common.IO;
using QOL.Common.Log;
using QOL.Common.Serialization;

namespace QOL.Common.Rest
{
    public class RestClient
    {
        /// <summary>
        /// 通信的超时时间
        /// </summary>
        public TimeSpan TrafficTimeout { set; get; }

        /// <summary>
        /// 用指定的url构造Rest请求客户端
        /// </summary>
        /// <param name="url">操作的url，不带参数</param>
        public RestClient(string url)
        {
            //_baseUrl = url;
            //UrlParameters = new List<HttpParameter>();
            //RequestBodyParameters = new List<HttpParameter>();
            //Headers = new List<HttpParameter>();
            //TrafficTimeout = TimeSpan.FromSeconds(30);
            _baseUrl = url;

            Init();
        }

        public RestClient(string url, string proxyUrl, int proxyPort)
        {
            _baseUrl = url;
            Init();
            _proxyUrl = proxyUrl;
            _proxyPort = proxyPort;
        }

        private void Init()
        {
            UrlParameters = new List<HttpParameter>();
            RequestBodyParameters = new List<HttpParameter>();
            Headers = new List<HttpParameter>();
            TrafficTimeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Url后跟的参数
        /// </summary>
        public List<HttpParameter> UrlParameters { private set; get; }

        /// <summary>
        /// 用于放置在Request的Body部分的参数，如果需要multipart形式的post并提交文件流，请添加BinaryParameter
        /// </summary>
        public List<HttpParameter> RequestBodyParameters { private set; get; }

        /// <summary>
        /// 需要添加的头
        /// </summary>
        public List<HttpParameter> Headers { private set; get; }

        /// <summary>
        /// 直接Post，返回响应的body
        /// </summary>
        public string Post()
        {
            string responseString = null;
            string transportUrl = "";
            string dataString = "";

            try
            {
                var request = BuildPost(out transportUrl, out dataString);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Encoding responseEncoding = Encoding.Default;
                    try
                    {
                        if (response.CharacterSet != null)
                        {
                            responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("ResetClient Post set responseEncoding error", GetType(), e);
                    }

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader stream = new StreamReader(responseStream, responseEncoding))
                            {
                                responseString = stream.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(responseString) && ex is WebException)
                {
                    responseString = ReadResponseStreamAndClose((ex as WebException).Response as HttpWebResponse);
                }

                Logger.Error(string.Format("Error when executing Post(), Transport Url : {0}, Post Data : {1}\r\nResponse :\r\n{2}", transportUrl, dataString, responseString), GetType(), ex);
                return null;
            }

            Logger.Trace(string.Format("Rest Transport Completed : Post(), Transport Url : {0}, Post Data : {1}", transportUrl, dataString), GetType());
            return responseString;
        }

        /// <summary>
        /// Post信息T并将结果反序列化为U
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Post<T>() where T : class
        {
            T result = null;
            string transportUrl = "";
            string dataString = "";

            try
            {
                var request = BuildPost(out transportUrl, out dataString);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            result = JsonSerializer.Deserialize<T>(responseStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string responseString = string.Empty;
                if (string.IsNullOrWhiteSpace(responseString) && ex is WebException)
                {
                    responseString = ReadResponseStreamAndClose((ex as WebException).Response as HttpWebResponse);
                }

                Logger.Error(string.Format("Error when executing Post<{0}>(), Transport Url : {1}, Post Data : {2}\r\nResponse :\r\n{3}", typeof(T), transportUrl, dataString, responseString), GetType(), ex);
                return null;
            }

            Logger.Trace(string.Format("Rest Transport Completed : Post<{0}>(), Transport Url : {1}, Post Data : {2}", typeof(T), transportUrl, dataString), GetType());
            return result;
        }

        /// <summary>
        /// 以multipart/form-data形式Post数据
        /// </summary>
        /// <param name="isReturnBodyString"></param>
        /// <returns></returns>
        public string PostMultiPart(bool isReturnBodyString = false)
        {
            string responseString = null;
            string transportUrl = "";

            try
            {
                var request = BuildRFC2045Post(out transportUrl);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (isReturnBodyString)
                    {
                        Encoding responseEncoding = Encoding.Default;
                        try
                        {
                            if (response.CharacterSet != null)
                            {
                                responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                            }
                        }
                        catch (Exception)
                        {
                            Logger.Error("set response error", GetType());
                        }

                        using (Stream responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (StreamReader stream = new StreamReader(responseStream, responseEncoding))
                                {
                                    responseString = stream.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(responseString) && ex is WebException)
                {
                    responseString = ReadResponseStreamAndClose((ex as WebException).Response as HttpWebResponse);
                }

                Logger.Error(string.Format("Error when executing PostMultiPart({0}), Transport Url : {1}\r\nResponse :\r\n{2}", isReturnBodyString, transportUrl, responseString), GetType(), ex);
                return null;
            }

            Logger.Trace(string.Format("Rest Transport Completed : PostMultiPart({0}), Transport Url : {1}", isReturnBodyString, transportUrl), GetType());
            return responseString;
        }

        /// <summary>
        /// 直接Get，返回响应Body
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            string secondTry;
            return Get<string, string>(RestDataTypes.String, out secondTry);
        }

        /// <summary>
        /// Get信息并以Json格式反序列化为指定模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetJson<T>() where T : class
        {
            object secondTry;
            return Get<T, object>(RestDataTypes.Json, out secondTry);
        }

        /// <summary>
        /// Get信息并以Json格式反序列化为指定模型，失败则尝试反序列化为第二种类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="secondTry"></param>
        /// <returns></returns>
        public T GetJson<T, U>(out U secondTry)
            where T : class
            where U : class
        {
            return Get<T, U>(RestDataTypes.Json, out secondTry);
        }

        /// <summary>
        /// Get信息并以Xml格式反序列化为指定模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetXml<T>() where T : class
        {
            object secondTry;
            return Get<T, object>(RestDataTypes.Xml, out secondTry);
        }

        /// <summary>
        /// Get信息并以Xml格式反序列化为指定模型，失败则尝试反序列化为第二种类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public T GetXml<T, U>(out U secondTry)
            where T : class
            where U : class
        {
            return Get<T, U>(RestDataTypes.Xml, out secondTry);
        }

        #region Privates

        private T Get<T, U>(RestDataTypes restDataType, out U secondTryDataType)
            where T : class
            where U : class
        {
            T result = null;
            secondTryDataType = null;
            string transportUrl = "";
            string responseContent = "";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)BuildGet(out transportUrl).GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            byte[] buffer;

                            int contentLength = (int)response.ContentLength;
                            if (contentLength > 0)
                            {
                                buffer = new byte[contentLength];

                                int offset = 0;
                                while (offset < contentLength)
                                {
                                    offset += responseStream.Read(buffer, offset, contentLength - offset);
                                }
                            }
                            else
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    buffer = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                                }
                            }

                            byte[] convertedBuffer;

                            using (MemoryStream ms = new MemoryStream(buffer))
                            {
                                responseContent = AutoDetectEncodingStreamReader.ReadAllText(ms, Encoding.UTF8);
                                convertedBuffer = Encoding.UTF8.GetBytes(responseContent);
                            }

                            using (MemoryStream ms = new MemoryStream(convertedBuffer))
                            {
                                switch (restDataType)
                                {
                                    case RestDataTypes.Json:
                                        result = JsonSerializer.Deserialize<T>(ms);
                                        break;
                                    case RestDataTypes.Xml:
                                        result = XmlSerializer.Deserialize<T>(ms, Encoding.UTF8);
                                        break;
                                    case RestDataTypes.String:
                                        result = Encoding.UTF8.GetString(convertedBuffer) as T;
                                        break;
                                }
                            }

                            if (result == null)
                            {
                                using (MemoryStream ms = new MemoryStream(convertedBuffer))
                                {
                                    switch (restDataType)
                                    {
                                        case RestDataTypes.Json:
                                            secondTryDataType = JsonSerializer.Deserialize<U>(ms);
                                            break;
                                        case RestDataTypes.Xml:
                                            secondTryDataType = XmlSerializer.Deserialize<U>(ms, Encoding.UTF8);
                                            break;
                                        case RestDataTypes.String:
                                            secondTryDataType = Encoding.UTF8.GetString(convertedBuffer) as U;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(responseContent) && ex is WebException)
                {
                    responseContent = ReadResponseStreamAndClose((ex as WebException).Response as HttpWebResponse);
                }

                Logger.Error(string.Format("Error when executing GET<{0}>({1}), Transport Url : {2}\r\nResponse :\r\n{3}", typeof(T), restDataType, transportUrl, responseContent), GetType(), ex);
                return null;
            }

            Logger.Trace(string.Format("Rest Transport Completed : GET<{0}>({1}), Transport Url : {2}\r\nResponse : \r\n{3}", typeof(T), restDataType, transportUrl, responseContent), GetType());
            return result;
        }

        private HttpWebRequest BuildGet(out string url)
        {
            string postUrl = GenerateRequestUrl();
            url = postUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);

            //proxy
            if (!string.IsNullOrWhiteSpace(_proxyUrl))
            {
                var proxy = new WebProxy(_proxyUrl, _proxyPort);
                request.Proxy = proxy;
                Logger.Info(string.Format("request {0} by proxy host:{1},port:{2}", url, _proxyUrl, _proxyPort));
            }
            request.Timeout = (int)TrafficTimeout.TotalMilliseconds;
            request.ReadWriteTimeout = (int)TrafficTimeout.TotalMilliseconds;
            request.Method = "GET";
            AddHeaders(request);
            return request;
        }

        private HttpWebRequest BuildPost(out string url, out string postDataString)
        {
            string postUrl = GenerateRequestUrl();
            url = postUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);

            //proxy
            if (!string.IsNullOrWhiteSpace(_proxyUrl))
            {
                var proxy = new WebProxy(_proxyUrl, _proxyPort);
                request.Proxy = proxy;

                request.Proxy.Credentials = null;

                Logger.Info(string.Format("request {0} by proxy host:{1},port:{2}", url, _proxyUrl, _proxyPort));
            }

            request.Timeout = (int)TrafficTimeout.TotalMilliseconds;
            request.ReadWriteTimeout = (int)TrafficTimeout.TotalMilliseconds;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            AddHeaders(request);

            var dataToPost = GenerateRequestBody(out postDataString);
            request.ContentLength = dataToPost.Length;
            try
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(dataToPost, 0, dataToPost.Length);
                }

                return request;
            }
            catch (Exception e)
            {
                Logger.Error("RestClient BuildPost error!", GetType(), e);
                return null;
            }
        }



        private HttpWebRequest BuildRFC2045Post(out string url)
        {
            string postUrl = GenerateRequestUrl();
            url = postUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Timeout = (int)TrafficTimeout.TotalMilliseconds;
            request.ReadWriteTimeout = (int)TrafficTimeout.TotalMilliseconds;
            request.Method = "POST";
            AddHeaders(request);

            FillRFC2045RequestBody(request);

            return request;
        }

        private void AddHeaders(WebRequest request)
        {
            foreach (var parameter in Headers)
            {
                string name, value;
                EncodeHttpParameter(parameter, out name, out value);
                request.Headers.Add(name, value);
            }
        }

        private string GenerateRequestUrl()
        {
            if (UrlParameters.Count == 0)
            {
                return _baseUrl;
            }

            StringBuilder sb = new StringBuilder(_baseUrl);
            sb.Append("?");

            foreach (var parameter in UrlParameters)
            {
                string name, value;
                EncodeHttpParameter(parameter, out name, out value);
                sb.Append(string.Format("{0}={1}&", name, value));
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private byte[] GenerateRequestBody(out string postDataString)
        {
            if (RequestBodyParameters.Count == 0)
            {
                postDataString = string.Empty;
                return new byte[0];
            }

            StringBuilder sb = new StringBuilder();

            foreach (var parameter in RequestBodyParameters)
            {
                string name, value;
                EncodeHttpParameter(parameter, out name, out value);
                sb.Append(string.Format("{0}={1}&", name, value));
            }

            sb.Remove(sb.Length - 1, 1);
            postDataString = sb.ToString();
            return Encoding.UTF8.GetBytes(postDataString);
        }

        private void FillRFC2045RequestBody(HttpWebRequest request)
        {
            string boundary = string.Format("------Boundary{0}", DateTime.Now.Ticks.ToString("x"));
            byte[] beginBoundary = Encoding.UTF8.GetBytes(string.Format("--{0}\r\n", boundary));
            byte[] endBoundary = Encoding.UTF8.GetBytes(string.Format("--{0}--\r\n", boundary));
            byte[] newline = Encoding.UTF8.GetBytes("\r\n");

            const string httpParameterFormat = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            const string binaryParameterHeaderFormat = "Content-Disposition: form-data; name=\"{0}\"; filename=\"whatever.jpg\"\r\n\r\n";

            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using (var bodyStream = new MemoryStream())
            {
                foreach (HttpParameter bodyParameter in RequestBodyParameters)
                {
                    bodyStream.Write(beginBoundary, 0, beginBoundary.Length);

                    if (bodyParameter is BinaryParameter)
                    {
                        var binaryParameter = bodyParameter as BinaryParameter;
                        string name = EncodeBinaryParameter(binaryParameter);
                        string header = string.Format(binaryParameterHeaderFormat, name);
                        byte[] headerBytes = Encoding.UTF8.GetBytes(header);

                        bodyStream.Write(headerBytes, 0, headerBytes.Length);
                        bodyStream.Write(binaryParameter.Value, 0, binaryParameter.Value.Length);
                        bodyStream.Write(newline, 0, newline.Length);
                    }
                    else
                    {
                        string name, value;
                        EncodeHttpParameter(bodyParameter, out name, out value);
                        string parameter = string.Format(httpParameterFormat, name, value);
                        byte[] parameterBytes = Encoding.UTF8.GetBytes(parameter);

                        bodyStream.Write(parameterBytes, 0, parameterBytes.Length);
                    }
                }

                bodyStream.Write(endBoundary, 0, endBoundary.Length);

                using (var requestStream = request.GetRequestStream())
                {
                    bodyStream.Position = 0;
                    bodyStream.WriteTo(requestStream);
                }
            }
        }

        private static void EncodeHttpParameter(HttpParameter parameter, out string name, out string value)
        {
            name = parameter.EncodeName ? HttpUtility.UrlEncode(parameter.Name) : parameter.Name;
            value = parameter.EncodeValue ? HttpUtility.UrlEncode(parameter.Value) : parameter.Value;
        }

        private static string EncodeBinaryParameter(BinaryParameter parameter)
        {
            return parameter.EncodeName ? HttpUtility.UrlEncode(parameter.Name) : parameter.Name;
        }

        private static string ReadResponseStreamAndClose(HttpWebResponse response)
        {
            var textContent = String.Empty;

            if (response == null)
            {
                return textContent;
            }

            try
            {
                var responseEncoding = Encoding.Default;
                try
                {
                    if (response.CharacterSet != null)
                    {
                        responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("set responseEncoding error", typeof(RestClient));
                }

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader stream = new StreamReader(responseStream, responseEncoding))
                        {
                            textContent = stream.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error when executing ReadResponseStreamAndClose.", typeof(RestClient), ex);
            }
            finally
            {
                try
                {
                    response.Close();
                }
                catch (Exception e)
                {
                    Logger.Error("response close error", typeof(RestClient));
                }
            }

            return textContent;
        }

        private readonly string _baseUrl;

        #region 代理 Proxy

        private string _proxyUrl;

        private int _proxyPort;

        #endregion

        #endregion
    }
}
