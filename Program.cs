﻿using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {	
            List<string> urls = new List<string>();
            urls.Add("https://salem.craigslist.org/d/software-qa-dba-etc/search/sof");
            // urls.Add("https://salem.craigslist.org/d/web-html-info-design/search/web");
            // urls.Add("https://salem.craigslist.org/d/computer-gigs/search/cpg");
            //string url = "https://salem.craigslist.org/d/web-html-info-design/search/web";
            foreach (var url in urls)
            {
                MainAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            Console.ReadLine();
        }

        async static Task MainAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            HttpClient client = new HttpClient(handler);
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler?view=netframework-4.7.2
            try	
            {
                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                ResponseXpathPartAsync(responseBody);
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

        static void ResponseXpathPartAsync(string pageContents)
        {
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);
            
            //var headlineText = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'pb-f-homepage-hero')]//h3)[1]").InnerText;
            var rowLists = pageDocument.DocumentNode.SelectNodes("(//li[contains(@class,'result-row')])");
            Console.WriteLine("--------------------------");
            Console.WriteLine(rowLists[0].OuterHtml);
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
            //
        }
       
    }
}