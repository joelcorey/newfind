﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace AsyncApp
{
    class Program
    {
        // https://dotnetcoretutorials.com/2018/02/27/loading-parsing-web-page-net-core/

        // Pass WebProxy object ??
        // https://docs.microsoft.com/en-us/dotnet/api/system.net.webproxy?view=netframework-4.7.2

        // TODO
        // - Testing: add ability to use proxies
        // - function: test proxy with https://iplookup.flagfox.net 
        // - function: test proxy with http://dyndns.com
        // - function: test: comparitive between iplookup and dyndns
        // - automate feedback from proxies, favoribility system, what proxies failed, time on target etc
        // - return information from scraped websites, HTTP response code etc
        // - Make most/all methods async?

        static void Main(string[] args)
        {	
            Config config = new Config(); // https://iplookup.flagfox.net

            List<string> urls = new List<string>();
            urls.Add("http://checkip.dyndns.com/");
            //urls.Add("http://api.ipstack.com/203.150.250.131?access_key=" + config.IpStackApiKey);
            //urls.Add("https://salem.craigslist.org/d/software-qa-dba-etc/search/sof");
            // urls.Add("https://salem.craigslist.org/d/web-html-info-design/search/web");
            // urls.Add("https://salem.craigslist.org/d/computer-gigs/search/cpg");
            //string url = "https://salem.craigslist.org/d/web-html-info-design/search/web";
            foreach (var url in urls)
            {
                MainAsync(url, config.GetUserAgentRandom(), "58.137.62.133:80", true).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            Console.ReadLine();
        }

        async static Task MainAsync(string url, string userAgent, string proxyAddress, bool useProxy = false)
        {
            HttpClientHandler handler = ClientHandler(proxyAddress, useProxy);
          
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler?view=netframework-4.7.2
            try	
            {
                HttpResponseMessage response = await client.GetAsync(url);
                
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);

                // At a later date response codes will be important:
                //int responseCode = (int)response.StatusCode;
                //ResponseXpathParse(responseBody);
            }  
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient and HttpClientHandler objects 
            // when done using them, so the app doesn't leak resources
            // handler.Dispose(true);
            // client.Dispose(true);
        }

        static HttpClientHandler ClientHandler(string proxyAddress, bool useProxy = false)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if(useProxy == true)
            {
                handler.Proxy = new WebProxy(proxyAddress);
                handler.UseProxy = true;
            }
            handler.UseDefaultCredentials = true;
            return handler;
        }

        static HtmlAgilityPack.HtmlNode SearchXpathSingle(string contents, string xpathSearchString)
        {
            // Make async in future ?
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(contents);

            HtmlAgilityPack.HtmlNode xpathObject = pageDocument.DocumentNode.SelectSingleNode(xpathSearchString);
            return xpathObject; 
        }

        static HtmlAgilityPack.HtmlNodeCollection SearchXpathMulti(string contents, string xpathSearchString)
        {
            // Make async in future ?
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(contents);

            HtmlAgilityPack.HtmlNodeCollection xpathObject = htmlDoc.DocumentNode.SelectNodes(xpathSearchString);
            return xpathObject; 
        }


        static void ResponseXpathParse(string pageContents)
        {
            // Make async in future ?
            HtmlAgilityPack.HtmlNode title = SearchXpathSingle(pageContents, "(//title)");
            Console.WriteLine(title.OuterHtml);
            //Console.WriteLine(title.GetType());
            // foreach (var row in rowLists)
            // {
            //     Console.WriteLine(row.InnerHtml);
            // }
            //var childs = rowLists[0].ChildNodes;

            // var childs = rowLists[0].Descendants();
            // Console.WriteLine(childs.GetEnumerator());
            // foreach (var child in childs)
            // {
            //     Console.WriteLine(child.OuterHtml);
            // }
        }
       
    }
}