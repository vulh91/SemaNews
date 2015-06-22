using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemaNews.Utilities
{
    public static class HtmlHandler
    {
        public static string DefaultUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";

        public static string StripHtmlTags(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string GetRawHtmlSource(string url, int retryNumber = 5)
        {
            string htmlSource = "";
            url = NormalizeUrl(url);
            if (ValidateUrl(url) == false)
            {
                throw new FormatException();
            }

            int check = retryNumber;
            while (check > 0)
            {
                try
                {
                    //Create request for given url
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    //Create response-object
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //Take response stream
                    StreamReader sr = new StreamReader(response.GetResponseStream());

                    //Read response stream (html code)
                    string html = DecodeData(response);
                    html = System.Web.HttpUtility.HtmlDecode(html);
                    //Close streamreader and response
                    sr.Close();
                    response.Close();

                    if (Regex.IsMatch(html, "html|HTML"))
                    {
                        htmlSource = html;
                        break;
                    }
                    else
                        check--;

                }
                catch
                {
                    check--;
                }
            }
            return htmlSource;
        }

        public static string ExtractHtmlContent(string htmlSource, string path, int minLength = 1, int maxLength = 0)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);

            var results = HtmlQueryHelpers.GetNodesByXpathAttr(htmlDoc, path);
            if (results == null || results.Count == 0)
                return string.Empty;

            //Heuristic to get exact tag' content
            results = results.Except(results.Where(str => str.InnerText.Trim().Length < minLength)).ToList();
            results = results.Except(results.Where(str => str.InnerText.Trim().Length > maxLength)).ToList();
            if (results.Count == 0)
                return string.Empty;
            else
                return results[0].InnerText;
        }

        public static string GetHtmlNextPage(string htmlCurrentPage, string paginationAddr)
        {
            throw new NotImplementedException();
        }

        public static string RelToAbsConvert(string domain, string relUrl)
        {
            var baseUri = new Uri(domain);
            return new Uri(baseUri, relUrl).AbsoluteUri;
        }

        public static HtmlDocument GetHtmlSource(string url, int retryNumber = 5)
        {
            if (url == string.Empty)
                return null;
            // request 50 times maximum
            int check = retryNumber;
            while (check > 0)
            {
                try
                {
                    //WebClient webClient = new WebClient();
                    ////webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
                    ////webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                    //webClient.Encoding = Encoding.UTF8;

                    //string s = webClient.DownloadString(url);

                    //Create request for given url
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    //Create response-object
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //Take response stream
                    StreamReader sr = new StreamReader(response.GetResponseStream());

                    //Read response stream (html code)
                    string html = DecodeData(response);

                    //Close streamreader and response
                    sr.Close();
                    response.Close();

                    if (Regex.IsMatch(html, "html|HTML"))
                    {
                        return FixRelativeToAbsoluteForSource(url, html);
                    }
                    else
                        check--;

                }
                catch
                {
                    check--;
                }
            }
            return null;
        }

        private static HtmlDocument FixRelativeToAbsoluteForSource(string url, string htmlStr)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlStr);
                var baseUri = new Uri(url);
                var images = doc.DocumentNode.SelectNodes("//img/@src").ToList();
                var links = doc.DocumentNode.SelectNodes("//a/@href").ToList();
                foreach (var item in images.Concat(links))
                {
                    if (item.Attributes["src"] != null)
                    {
                        var absoluteUrl = new Uri(baseUri, item.Attributes["src"].Value).AbsoluteUri;
                        item.SetAttributeValue("src", absoluteUrl);
                    }
                    else if (item.Attributes["href"] != null)
                    {
                        var absoluteUrl = new Uri(baseUri, item.Attributes["href"].Value).AbsoluteUri;
                        item.SetAttributeValue("href", absoluteUrl);
                    }

                }
                return doc;
            }
            catch
            {
                return null;
            }
        }

        private static bool ValidateUrl(string url)
        {
            //Not implemented yet
            return true;
        }

        public static string NormalizeUrl(string url)
        {
            url = HtmlEntity.DeEntitize(url);
            if (!url.StartsWith("http"))
                if (url.StartsWith("/"))
                    url = url.Insert(0, @"http:/");
                else
                    url = url.Insert(0, @"http://");

            url = Regex.Replace(url, "#(.[^/]*)?$", "");
            return url;
        }

        public static string DetermineDomain(string url)
        {
            MatchCollection slash = Regex.Matches(url, "/");
            if (slash.Count == 2)
                return url;
            else if (slash.Count > 2)
                return url.Substring(0, slash[2].Index);
            return "error";
        }

        public static string ReSetSourceStyle(string htmlCode, string domain)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlCode);

            var eleWithLink = (from lnks in doc.DocumentNode.Descendants()
                               where lnks.Attributes["src"] != null ||
                                     lnks.Attributes["href"] != null
                               select lnks).ToList();
            for (int i = 0; i < eleWithLink.Count; i++)
            {
                var item = eleWithLink[i];

                if (item.Attributes.Any(x => x.Name == "src"))
                {
                    string srcValue = item.Attributes["src"].Value;

                    if (!srcValue.StartsWith("http"))
                    {
                        srcValue = domain + srcValue;
                        eleWithLink[i].SetAttributeValue("src", srcValue);
                    }
                }
                if (item.Attributes.Any(x => x.Name == "href"))
                {
                    string hrefValue = item.Attributes["href"].Value;


                    if (!hrefValue.StartsWith("http"))
                    {
                        hrefValue = domain + hrefValue;
                        eleWithLink[i].SetAttributeValue("href", hrefValue);
                    }

                }
            }
            return doc.DocumentNode.OuterHtml;
        }

        public static List<string> ExtractUrls(string htmlSource, string path)
        {
            List<string> links = new List<string>();

            if (string.IsNullOrEmpty(htmlSource) || string.IsNullOrEmpty(path))
                return links;

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlSource);

            // get Node match with Element in the htmldocument
            var elements = HtmlQueryHelpers.GetNodesByXpathAttr(htmlDoc, path);
            if (elements != null && elements.Count > 0)
            {
                var temp = elements[0].Descendants("a").Where(a => a.Attributes["href"] != null)
                .Select(a => a.Attributes["href"].Value);
          
                // get all link exist in Node
                links = links.Union(temp).ToList();
            }
            return links;
        }

        public static string FixLink(string link, string fullLink)
        {
            var baseUrl = new Uri(fullLink);
            var url = new Uri(baseUrl, link);
            return url.AbsoluteUri;
        }

        private static String DecodeData(WebResponse w)
        {

            //
            // first see if content length header has charset = calue
            //
            String charset = null;
            String ctype = w.Headers["content-type"];
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=");
                if (ind != -1)
                {
                    charset = ctype.Substring(ind + 8);
                    Console.WriteLine("CT: charset=" + charset);
                }
            }

            // save data to a memorystream
            MemoryStream rawdata = new MemoryStream();
            byte[] buffer = new byte[1024];
            Stream rs = w.GetResponseStream();
            int read = rs.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                rawdata.Write(buffer, 0, read);
                read = rs.Read(buffer, 0, buffer.Length);
            }

            rs.Close();

            //
            // if ContentType is null, or did not contain charset, we search in body
            //
            if (charset == null)
            {
                MemoryStream ms = rawdata;
                ms.Seek(0, SeekOrigin.Begin);

                StreamReader srr = new StreamReader(ms, Encoding.ASCII);
                String meta = srr.ReadToEnd();

                if (meta != null)
                {
                    int start_ind = meta.IndexOf("charset=");
                    int end_ind = -1;
                    if (start_ind != -1)
                    {
                        end_ind = meta.IndexOf("\"", start_ind);
                        if (end_ind != -1)
                        {
                            int start = start_ind + 8;
                            charset = meta.Substring(start, end_ind - start + 1);
                            charset = charset.TrimEnd(new Char[] { '>', '"' });
                            Console.WriteLine("META: charset=" + charset);
                        }
                    }
                }
            }

            Encoding e = null;
            if (charset == null)
            {
                e = Encoding.ASCII; //default encoding
            }
            else
            {
                try
                {
                    e = Encoding.GetEncoding(charset);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Exception: GetEncoding: " + charset);
                    Console.WriteLine(ee.ToString());
                    e = Encoding.ASCII;
                }
            }

            rawdata.Seek(0, SeekOrigin.Begin);

            StreamReader sr = new StreamReader(rawdata, e);

            String s = sr.ReadToEnd();

            return s;
        }
    }
}
