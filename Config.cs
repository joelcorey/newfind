using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Config
{
    private readonly Random random = new Random();
    private int randomNumber;
    //private int userAgentListLength;

    private JObject userAgentObject;
    private JArray userAgentArray;

    private int userAgentListLength;
    
    public string UserAgentRandom
    {
        get { return "hi"; }
    }

    public int UserAgentListLength
    {
        get { return this.userAgentArray.Count; }
    }

    public int RandomNumber
    {
        get { return this.random.Next(3000) + 1000; }
    }

    public Config()
    {
        // https://www.newtonsoft.com/json/help/html/QueryingLINQtoJSON.htm
        this.userAgentObject = JObject.Parse(File.ReadAllText(@"useragentlite.json"));
        this.userAgentArray = (JArray)userAgentObject["userAgents"];
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