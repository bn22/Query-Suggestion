﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private PerformanceCounter memProcess = new PerformanceCounter("Memory", "Available MBytes");
        private static trie wikiTitles = new trie();
        private static String filepath;

        [WebMethod]
        public void downloadWiki()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("blob");
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference("Post-Processed-Titles.txt");
            if (container.Exists())
            {
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        filepath = System.IO.Path.GetTempFileName();
                        using (var fileStream = System.IO.File.OpenWrite(filepath))
                        {
                            blockBlob2.DownloadToStream(fileStream);
                        }
                    }
                }
            }
        }

        [WebMethod]
        public void buildTrie()
        {
            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (lineCount % 1000 == 0)
                        {
                            float memUsage = memProcess.NextValue();
                            if (memUsage < 50)
                            {
                                break;
                            }
                        }
                        else
                        {
                            wikiTitles.addTitles(line);
                        }
                        lineCount = lineCount + 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String searchTrie(String prefix)
        {
            List<String> results = new List<String>();
            results = wikiTitles.findSuggestions(prefix);
            return new JavaScriptSerializer().Serialize(results);
        }
    }
}