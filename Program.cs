﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AsyncApp
{
    class Program
    {
        // https://dotnetcoretutorials.com/2018/02/27/loading-parsing-web-page-net-core/

        // Pass WebProxy object ??
        // https://docs.microsoft.com/en-us/dotnet/api/system.net.webproxy?view=netframework-4.7.2

        // TODO
        // - add ability to use proxies
        // - automate feedback from proxies, favoribility system
        // - return information from scraped websites, HTTP response code etc
        // - Make most/all methods async?

        static void Main(string[] args)
        {	
            JObject userAgentList = JObject.Parse(File.ReadAllText(@"useragent.json"));
            foreach (var userAgent in userAgentList)
            {
                Console.WriteLine(userAgent);
            }
            // List<string> urls = new List<string>();
            // urls.Add("https://salem.craigslist.org/d/software-qa-dba-etc/search/sof");
            // urls.Add("https://salem.craigslist.org/d/web-html-info-design/search/web");
            // urls.Add("https://salem.craigslist.org/d/computer-gigs/search/cpg");
            // //string url = "https://salem.craigslist.org/d/web-html-info-design/search/web";
            // foreach (var url in urls)
            // {
            //     MainAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
            // }
            Console.ReadLine();
        }

        async static Task MainAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", GetUserAgent());
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler?view=netframework-4.7.2
            try	
            {
                HttpResponseMessage response = await client.GetAsync(url);
                
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                
                ResponseXpathParse(responseBody);
                //Console.WriteLine(responseBody);
            }  
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            // Need to call dispose on the HttpClient and HttpClientHandler objects 
            // when done using them, so the app doesn't leak resources
            // handler.Dispose(true);
            // client.Dispose(true);
        }

        static String GetUserAgent()
        {
            String userAgent = "Mozilla/5.0 (compatible; AcmeInc/1.0)";
            return userAgent;
        }

        static HtmlAgilityPack.HtmlNode SearchXpath(string contents, string xpathSearchString)
        {
            // Make async in future ?
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(contents);

            HtmlAgilityPack.HtmlNode xpathObject = pageDocument.DocumentNode.SelectSingleNode(xpathSearchString);
            return xpathObject; 
        }

        static void ResponseXpathParse(string pageContents)
        {
            // Make async in future ?
            HtmlAgilityPack.HtmlNode title = SearchXpath(pageContents, "(//title)");
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