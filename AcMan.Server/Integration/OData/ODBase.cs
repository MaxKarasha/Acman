﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AcMan.Server.Integration.OData
{
    public class ODBase
    {
        private static readonly XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static readonly XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";

        private string _dataServer;
        public string _dataServiceUrl;
        private string _dataServiceLogin;
        private string _dataServicePassword;
        private int? _dataServiceSolutionId;
        private string _authMethod;
        private string _authVersion;

        private bool _useHttps = false; // временный костыль
        private CookieContainer _cookieContainer;

        private int _requestsCompleted = 0;
        public int requestsCompeted {
            get { return _requestsCompleted; }
        }

        private void countRequest()
        {
            this._requestsCompleted++;
        }
        public List<string> debugMessages = new List<string>();
        public List<string> errorMessages = new List<string>();

        public bool hideConsole = false;

        public void ResetErrorMessages()
        {
            this.errorMessages = new List<string>();
        }

        public static class CommonIds
        {
            public static string fileTypeFile = "529BC2F8-0EE0-DF11-971B-001D60E938C6".ToLower();
            public static string fileTypeLink = "539BC2F8-0EE0-DF11-971B-001D60E938C6".ToLower();
        }

        public ODBase(string url, string login, string password, int? solutionId = null, string authMethod = "POST", string authVersion = "5.4")
        {
            this._dataServer = url;
            this._dataServiceSolutionId = solutionId;
            if (authVersion == "5.4" && solutionId == null) {
                solutionId = 0;
            }
            this._dataServiceUrl = url + (solutionId != null ? "/" + ((int)solutionId).ToString() : "") + "/ServiceModel/EntityDataService.svc/";
            this._dataServiceLogin = login;
            this._dataServicePassword = password;
            this._authMethod = authMethod;
            this._authVersion = authVersion;

            if (url.StartsWith("https://")) {
                this._useHttps = true;
            }
            TryLogin();
        }


        protected internal bool TryLogin()
        {
            bool result = false;
            if (this._authMethod == "POST") {
                string loginPath = _dataServer + "/ServiceModel/AuthService.svc/Login";

                var request = HttpWebRequest.Create(loginPath) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                _cookieContainer = new CookieContainer();
                request.CookieContainer = _cookieContainer;
                request.Headers.Add("ForceUseSession", "true");

                using (var requestStream = request.GetRequestStream()) {
                    using (var writer = new StreamWriter(requestStream)) {
                        switch (this._authVersion) {
                            case "5.1": {
                                    writer.Write(@"{
										""UserLogin"":""" + _dataServiceLogin + @""",
										""UserPassword"":""" + _dataServicePassword + @""",
										""Language"":""Ru-ru""
										}");
                                    break;
                                }
                            case "5.4": {
                                    writer.Write(@"{
										""UserName"":""" + _dataServiceLogin + @""",
										""UserPassword"":""" + _dataServicePassword + @""",
										""Language"":""Ru-ru""
										}");

                                    break;
                                }
                        }
                    }
                }
                using (var response = (HttpWebResponse)request.GetResponse()) {
                    if (response.StatusCode == HttpStatusCode.OK) {
                        result = true;
                    }
                    response.Close();
                }
            }
            if (this._authMethod == "GET") {
                string loginPath = _dataServer + "/ServiceModel/AuthService.svc/Login?UserName=" + _dataServiceLogin + "&UserPassword=" + _dataServicePassword + "&SolutionName=TSBpm";

                var request = HttpWebRequest.Create(loginPath) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                _cookieContainer = new CookieContainer();
                request.CookieContainer = _cookieContainer;
                request.Headers.Add("ForceUseSession", "true");

                using (var response = (HttpWebResponse)request.GetResponse()) {
                    if (response.StatusCode == HttpStatusCode.OK) {
                        result = true;
                    }
                    response.Close();
                }
            }
            return result;
        }

        public List<string> GetCollections()
        {
            List<string> collections = new List<string>();
            var request = (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl);
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "GET";
            request.Headers.Add("ForceUseSession", "true");

            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;

            try {
                using (var response = request.GetResponse()) {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(response.GetResponseStream());

                    if (true || xd.SelectNodes("child::service").Count == 1) {
                        foreach (XmlElement xe in xd["service"]["workspace"].ChildNodes) {
                            if (xe.Name == "collection" && xe.HasAttribute("href")) {
                                string href = xe.GetAttribute("href");
                                if (!collections.Contains(href)) {
                                    collections.Add(href);
                                }
                            }
                        }
                    } else {
                        if (!this.hideConsole) {
                            Console.WriteLine("Missing [service] node in resulting xml");
                        }
                        this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "Failed to retrieve collections");
                    }
                    response.Close();
                }
            } catch (WebException e) {
                throw new ODWebException(e);
            }

            this.countRequest();
            return collections;
        }

        public string GetCollectionPageUrl(string collectionName)
        {
            return this._dataServiceUrl + collectionName;
        }

        public static string DateTimeToOdataString(DateTime value)
        {
            return value.ToUniversalTime().ToString("o");
            //return value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff");
            //return value.ToString("o");
        }

        protected internal List<XmlElement> GetAllPages(string collection, string query, int maxIterations = 10)
        {
            List<XmlElement> result = new List<XmlElement>();

            bool goNext = true;
            string goUrl = this._dataServiceUrl + collection + "Collection" + (query != "" && query != string.Empty ? "?" + query : "");
            //this.debugMessages.Add(goUrl);
            int iter = 0;
            while (goNext && iter < maxIterations) {
                goNext = false;
                List<XmlElement> entries = this.GetPage(goUrl);

                foreach (XmlElement entry in entries) {
                    if (entry.Name == "entry") {
                        result.Add(entry);
                    }
                    if (entry.Name == "link" && entry.HasAttribute("rel") && entry.GetAttribute("rel") == "next" && entry.HasAttribute("href")) {
                        string goUrlNext = entry.GetAttribute("href");
                        if (goUrl != goUrlNext) {
                            goNext = true;
                            goUrl = goUrlNext;
                        }
                    }
                }
                iter++;
            }

            return result;
        }

        public int GetCollectionSize(string Collection)
        {
            int result = 0;
            string url = this._dataServiceUrl + Collection + "Collection?$inlinecount=allpages&$select=Id&$top=1";
            if (this._useHttps && url.StartsWith("http://")) // костыль
            {
                url = url.Replace("http://", "https://");
            }

            XmlDocument xd = new XmlDocument();
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "GET";
            request.Headers.Add("ForceUseSession", "true");

            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;

            try {
                using (var response = (HttpWebResponse)request.GetResponse()) {
                    xd.Load(response.GetResponseStream());
                    if (xd.GetElementsByTagName("feed").Count == 1) {
                        foreach (XmlElement xe in xd["feed"].ChildNodes) {
                            if (xe.Name == "m:count") {
                                int.TryParse(xe.InnerText, out result);
                            }
                        }
                    }
                    response.Close();
                }
            } catch (WebException e) {
                throw new ODWebException(e);
            }

            return result;
        }






        protected internal byte[] GetData(string url)
        {
            byte[] bytes = new byte[0];
            if (this._useHttps && url.StartsWith("http://")) // костыль
            {
                url = url.Replace("http://", "https://");
            }

            try {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
                request.Method = "GET";
                request.Headers.Add("ForceUseSession", "true");

                if (_cookieContainer == null || _cookieContainer.Count == 0) {
                    this.TryLogin();
                }
                request.CookieContainer = _cookieContainer;

                using (var response = (HttpWebResponse)request.GetResponse()) {
                    Stream s = response.GetResponseStream();
                    using (var ms = new MemoryStream()) {
                        s.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    s.Close();
                    response.Close();
                }
            } catch (WebException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Failed to retrieve data @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "Failed to retrieve data @ " + url);

                throw new ODWebException(e);
            }
            this.countRequest();
            return bytes;
        }

        public List<XmlElement> GetPage(string url)
        {
            List<XmlElement> result = new List<XmlElement>();
            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            if (this._useHttps && url.StartsWith("http://")) {
                url = url.Replace("http://", "https://");
            }

            try {
                using (var handler = new HttpClientHandler {
                    Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword),
                    CookieContainer = _cookieContainer
                })
                using (HttpClient client = new HttpClient(handler)) {
                    client.DefaultRequestHeaders.Add("ForceUseSession", "true");
                    using (var task = client.GetAsync(url)) {
                        var res = task.Result;
                        using (HttpContent content = res.Content) {
                            string data = content.ReadAsStringAsync().Result;
                            if (data != null) {
                                var xe = XElement.Parse(data);
                                foreach (var element in xe.Elements()) {
                                    if (element.Name.LocalName.Equals("entry")) {
                                        var doc = new XmlDocument();
                                        doc.Load(element.CreateReader());
                                        result.Add(doc.DocumentElement);
                                    }
                                }
                            }
                        }
                    }                    
                }
            } catch (XmlException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "] in XML @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "] in XML page @ " + url);
            } catch (WebException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "]. Failed to retrieve page @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "]. Failed to retrieve page @ " + url);

                throw new ODWebException(e);
            }

            this.countRequest();
            return result;
        }

        public List<XmlElement> GetPage_old(string url)
        {
            if (this._useHttps && url.StartsWith("http://")) // костыль
            {
                url = url.Replace("http://", "https://");
            }

            XmlDocument xd = new XmlDocument();
            List<XmlElement> result = new List<XmlElement>();


            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
                request.Method = "GET";
                request.Headers.Add("ForceUseSession", "true");

                if (_cookieContainer == null || _cookieContainer.Count == 0) {
                    this.TryLogin();
                }
                request.CookieContainer = _cookieContainer;

                using (var response = (HttpWebResponse)request.GetResponse()) {
                    var responseStream = response.GetResponseStream();
                    xd.Load(responseStream);
                    if (xd.GetElementsByTagName("feed").Count == 1) {
                        foreach (XmlElement xe in xd["feed"].ChildNodes) {
                            if (xe.Name == "entry") {
                                result.Add(xe);
                            }
                            if (xe.Name == "link") // признак следующей страницы
                            {
                                result.Add(xe);
                            }
                        }
                    } else {
                        if (xd.GetElementsByTagName("entry").Count == 1) {
                            result.Add(xd["entry"]);
                        }
                    }
                    response.Close();
                }
            } catch (XmlException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "] in XML @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Error message [" + e.Message + "] in XML page @ " + url);
            } catch (WebException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Failed to retrieve page @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Failed to retrieve page @ " + url);

                throw new ODWebException(e);
            }


            this.countRequest();
            return result;
        }


        public static string Dequote(string Name)
        {
            return Name.Replace("'", "''");
        }

        public static string XMLQuotes(string Name)
        {
            if (Name != null) {
                return Name.Replace("&", "&")/*.Replace("\"", """)*/.Replace("<", "<").Replace(">", ">").Replace("\n", " ").Replace("\r", " ");
            } else {
                return null;
            }
        }

        public static List<XElement> GetXEData(Dictionary<string, object> data, bool useNull = false)
        {
            List<XElement> xeData = new List<XElement>();
            foreach (KeyValuePair<string, object> kvp in data) {
                if (kvp.Value != null) {
                    if (kvp.Value is DateTime) {
                        xeData.Add(new XElement(ds + kvp.Key, DateTimeToOdataString((DateTime)kvp.Value)));                        
                    } else {
                        xeData.Add(new XElement(ds + kvp.Key, kvp.Value.ToString()));
                    }
                } else if (useNull) {
                    XElement ne = new XElement(ds + kvp.Key);
                    ne.SetAttributeValue(dsmd + "null", "true");
                    xeData.Add(ne);
                }
            }
            return xeData;
        }

        public string AddItem(string Collection, Dictionary<string, object> data)
        {
            List<XElement> xeData = GetXEData(data);

            XElement content = new XElement(dsmd + "properties", xeData);
            XElement entry = new XElement(atom + "entry", new XElement(atom + "content", new XAttribute("type", "application/xml"), content));

            var request = (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl + Collection + "Collection/");
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "POST";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            //request.ContentLength = 0;
            request.Headers.Add("ForceUseSession", "true");

            // TODO: Error, if we use a cookie for post request.
            /*if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }            
            request.CookieContainer = _cookieContainer;*/

            string result = "?";
            using (var writer = XmlWriter.Create(request.GetRequestStream())) {
                entry.WriteTo(writer);
            }

            try {
                WebResponse response = request.GetResponse();
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created) {
                    result = response.Headers["Location"];
                }
                response.Close();
            } catch (WebException e) {
                throw new ODWebException(e);
            }

            this.countRequest();
            return result;
        }

        protected internal static string GetDataLink(XmlElement entry)
        {
            string result = "";
            foreach (XmlElement xe in entry.ChildNodes) {
                if (xe.Name == "link" && xe.HasAttribute("title") && xe.GetAttribute("title") == "Data" && xe.HasAttribute("rel") && xe.GetAttribute("rel").EndsWith("edit-media/Data") && xe.HasAttribute("href")) {
                    result = xe.GetAttribute("href");
                    break;
                }
            }
            return result;
        }

        protected internal static Dictionary<string, object> GetEntryFields(XmlElement entry)
        {
            Dictionary<string, object> val = new Dictionary<string, object>();
            foreach (XmlElement xe in entry.ChildNodes) {
                if (xe.Name == "content") {
                    foreach (XmlElement d in xe["m:properties"].ChildNodes) {
                        string dName = d.Name.Replace("d:", "");
                        if (d.InnerText != "") {
                            string anotherValue = "";
                            if (d.HasAttribute("m:type")) // пропускаем null-значения разных типов
                            {
                                string attr = d.GetAttribute("m:type");
                                if (attr == "Edm.Guid" && d.InnerText == Guid.Empty.ToString()) {
                                    continue;
                                }
                                if (attr == "Edm.DateTime" && (d.InnerText == "0001-01-01T00:00:00" || d.InnerText == DateTime.MinValue.ToString())) {
                                    continue;
                                }
                                if (attr == "Edm.Boolean") {
                                    if (d.InnerText == "false") { anotherValue = "0"; }
                                    if (d.InnerText == "true") { anotherValue = "1"; }
                                }
                            }

                            val[dName] = anotherValue == "" ? d.InnerText : anotherValue;
                        }
                    }
                }
            }
            return val;
        }



        public void DeleteItem(string Collection, string Guid)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl + Collection + "Collection(guid'" + Guid + "')/");
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "DELETE";
            request.Headers.Add("ForceUseSession", "true");

            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;

            try {
                WebResponse response = request.GetResponse();
                response.Close();
            } catch (WebException e) {
                throw new ODWebException(e);
            }

        }

        public void DeleteLink(string Collection, string Guid, string CollectionTo)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl + Collection + "Collection(guid'" + Guid + "')/$links/" + CollectionTo);
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "DELETE";
            request.Headers.Add("ForceUseSession", "true");

            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;

            try {
                WebResponse response = request.GetResponse();
                response.Close();
            } catch (WebException e) {
                throw new ODWebException(e);
            }
        }

        public string UploadBinary(string Collection, string Guid, byte[] bytes, bool returnSHA256 = true)
        {
            var request =
                    (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl + Collection + "Collection(guid'" + Guid + "')/Data");
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            request.Method = "PUT";
            request.Accept = "application/atom+xml";
            request.ContentLength = bytes.Length;
            request.SendChunked = true;
            request.ContentType = "application/octet-stream";
            request.Headers.Add("ForceUseSession", "true");

            if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;

            MemoryStream ms = new MemoryStream(bytes);
            ms.WriteTo(request.GetRequestStream());
            try {
                WebResponse response = request.GetResponse();
                if (returnSHA256) {
                    SHA256 sha = SHA256Managed.Create();
                    byte[] hash = sha.ComputeHash(bytes);
                    string result = "";
                    int i;
                    for (i = 0; i < hash.Length; i++) {
                        result += String.Format("{0:X2}", hash[i]);
                    }
                    return result;
                } else {
                    return "OK";
                }
            } catch (WebException e) {
                throw new ODWebException(e);
            }
        }

        public string UpdateItem(string Collection, string Guid, Dictionary<string, object> data)
        {
            List<XElement> xeData = GetXEData(data, true);

            XElement content = new XElement(dsmd + "properties", xeData);

            XElement entry = new XElement(atom + "entry", new XElement(atom + "content", new XAttribute("type", "application/xml"), content));

            var request =
                    (HttpWebRequest)HttpWebRequest.Create(this._dataServiceUrl + Collection + "Collection(guid'" + Guid + "')/");
            request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
            //request.Method = "PUT";
            request.Method = "MERGE";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            //request.ContentLength = 0;
            request.Headers.Add("ForceUseSession", "true");

            /*if (_cookieContainer == null || _cookieContainer.Count == 0) {
                this.TryLogin();
            }
            request.CookieContainer = _cookieContainer;*/

            string result = "";
            using (var writer = XmlWriter.Create(request.GetRequestStream())) {
                entry.WriteTo(writer);
            }

            try {
                WebResponse response = request.GetResponse();
                result = "ok";
                response.Close();
            } catch (WebException e) {
                throw new ODWebException(e);
            }

            this.countRequest();
            return result;
        }


        public XmlDocument GetMetadata()
        {
            XmlDocument xd = new XmlDocument();
            ;
            string url = this._dataServiceUrl + "$metadata";

            if (this._useHttps && url.StartsWith("http://")) // костыль
            {
                url = url.Replace("http://", "https://");
            }

            try {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Credentials = new NetworkCredential(this._dataServiceLogin, this._dataServicePassword);
                request.Method = "GET";
                request.Headers.Add("ForceUseSession", "true");

                if (_cookieContainer == null || _cookieContainer.Count == 0) {
                    this.TryLogin();
                }
                request.CookieContainer = _cookieContainer;

                using (var response = (HttpWebResponse)request.GetResponse()) {
                    xd.Load(response.GetResponseStream());
                    response.Close();
                }
            } catch (WebException e) {
                if (!this.hideConsole) {
                    Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Failed to retrieve page @ " + url);
                }
                this.errorMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Failed to retrieve page @ " + url);

                throw new ODWebException(e);
            }
            this.countRequest();
            return xd;
        }



        public List<string> GetCollectionRelations(string collection)
        {
            List<string> result = new List<string>();
            XmlDocument metadata = this.GetMetadata();


            return result;
        }


        /// /// /// /// /// /// /// /// ///
        /// /// /// /// /// /// /// /// ///
        /// /// /// /// /// /// /// /// ///
        /// /// /// /// /// /// /// /// ///

        protected static internal ODObject getObjectFromEntry(string collection, XmlElement entry)
        {
            ODObject result = new ODObject();
            result._data = ODBase.GetEntryFields(entry);
            result._binaryDataLink = ODBase.GetDataLink(entry);
            result._Collection = collection;
            result.Guid = result["Id"].ToString();
            return result;
        }


        public ODObject GetFirstItemByUniqueField(string collection, string field, string fieldValue, string mode = "eq")
        {
            ODObject result = null;
            List<XmlElement> entries;
            switch (mode) {
                case "contains": {
                        entries = this.GetPage(this._dataServiceUrl + collection + "Collection?$filter=substringof('" + fieldValue + "'," + field + ")");
                        break;
                    }
                case "eq":
                case "equals":
                default: {
                        if (fieldValue != "true" && fieldValue != "false") {
                            fieldValue = "'" + fieldValue + "'";
                        }
                        entries = this.GetPage(this._dataServiceUrl + collection + "Collection?$filter=" + field + " eq " + fieldValue);
                        break;
                    }
            }

            foreach (XmlElement entry in entries) {
                if (entry.Name == "entry") {
                    result = ODBase.getObjectFromEntry(collection, entry);
                    return result;
                }
            }
            return result;
        }

        public ODObject GetFirstItemByQuery(string collection, string query)
        {
            ODObject result = null;
            List<XmlElement> entries = this.GetPage(this._dataServiceUrl + collection + "Collection?$filter=" + query);
            foreach (XmlElement entry in entries) {
                if (entry.Name == "entry") {
                    result = ODBase.getObjectFromEntry(collection, entry);
                    return result;
                }
            }
            return result;
        }


        public List<ODObject> GetSomeItems(string collection, int skip)
        {
            return this.GetSomeItemsByQuery(collection, "", skip);
        }

        public List<ODObject> GetSomeItems(string collection)
        {
            return this.GetSomeItemsByQuery(collection, "", 0);
        }

        public List<ODObject> GetSomeItemsByQuery(string collection, string query)
        {
            return this.GetSomeItemsByQuery(collection, query, 0);
        }

        public List<ODObject> GetSomeItemsByQuery(string collection, string query, int skip)
        {
            List<ODObject> result = new List<ODObject>();
            List<XmlElement> entries = this.GetPage(this._dataServiceUrl + collection + "Collection?$filter=" + (query != "" ? query : "1 eq 1") + (skip > 0 ? "&$skip=" + skip : ""));
            foreach (XmlElement entry in entries) {
                if (entry.Name == "entry") {
                    ODObject item = ODBase.getObjectFromEntry(collection, entry);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// HIGHLY NOT RECOMMENDED
        /// </summary>
        public List<ODObject> GetAllItemsByQuery(string collection, string query, int maxIterations = 10)
        {
            List<ODObject> result = new List<ODObject>();
            List<XmlElement> entries = this.GetAllPages(collection, query, maxIterations);

            foreach (XmlElement entry in entries) {
                if (entry.Name == "entry") {
                    ODObject item = ODBase.getObjectFromEntry(collection, entry);
                    result.Add(item);
                }
            }
            return result;
        }



        /* DEPRECATED */
        private ODObject GetOrCreateByUniqueField(string collection, string field, string name)
        {
            int errsNum = this.errorMessages.Count;
            ODObject result = this.GetFirstItemByUniqueField(collection, field, name);
            if (result != null) {
                return result;
            }
            if (this.errorMessages.Count == errsNum) // если ошибок не было
            {
                result = ODObject.NewObject(collection);
                result["Name"] = name;
                result.Update(this);
                result = this.GetFirstItemByUniqueField(collection, field, name);
            }
            return result;
        }

        public Dictionary<string, ODObject> GetDictionaryByUniqueField(string collection, string field, int maxIterations = 10)
        {
            return this.GetDictionaryByUniqueField(collection, field, "", maxIterations);
        }

        public Dictionary<string, ODObject> GetDictionaryByUniqueField(string collection, string field, string query, int maxIterations = 10)
        {
            List<XmlElement> entries = this.GetAllPages(collection, query, maxIterations);
            Dictionary<string, ODObject> result = new Dictionary<string, ODObject>();
            foreach (XmlElement entry in entries) {
                if (entry.Name == "entry") {
                    ODObject o = ODBase.getObjectFromEntry(collection, entry);

                    if (o._data.ContainsKey(field)) {
                        result[o[field].ToString()] = o;
                    }
                }
            }
            return result;
        }

    }
}
