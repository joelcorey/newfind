using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Config
{
    private readonly Random random = new Random();
 
    private JObject userAgentObject;
    private JArray userAgentArray;
    private int userAgentListLength;
    private string ipStackApiKey;

    public int UserAgentListLength
    {
        get { return userAgentArray.Count; }
    }
    public string IpStackApiKey
    {
        get { return ipStackApiKey; }
    }

    public Config()
    {
        DotNetEnv.Env.Load("./.env.development");

        // https://www.newtonsoft.com/json/help/html/QueryingLINQtoJSON.htm
        this.userAgentObject = JObject.Parse(File.ReadAllText(@"useragentlite.json"));
        this.userAgentArray = (JArray)userAgentObject["userAgents"];
        this.ipStackApiKey = System.Environment.GetEnvironmentVariable("IPSTACK_API_KEY");
    }

    public void DisplayUserAgents()
    {
        foreach (var userAgent in this.userAgentObject["userAgents"])
        {
            Console.WriteLine(userAgent);
        }
    }

    public void DisplayUserAgent(int agentNumber)
    {
        Console.WriteLine(this.userAgentArray[agentNumber]);
    }

    public string GetUserAgentRandom()
    {
        return (string)this.userAgentArray[this.random.Next(0, UserAgentListLength)];
    }



         
}