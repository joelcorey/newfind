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
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(pageContents);
            // Console.ReadLine();

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