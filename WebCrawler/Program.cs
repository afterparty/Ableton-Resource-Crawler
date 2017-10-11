using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Crawler: \n");
            var startingHTML = "http://www.spaz.org/~krud/files/Ableton.Live.Suite9.7/ableton_live_suite_9.7_32/Ableton/Live%209%20Suite/Resources/Core%20Library/Ableton%20Folder%20Info/Previews/Devices/Instruments/";
            //generate list of instruments
            HtmlWeb web = new HtmlWeb();
            var instrumentList = new List<string>();
            var htmlDoc = web.Load(startingHTML);
            var instrumentNodes = htmlDoc.DocumentNode.SelectNodes("//tr/td/a/@href");

            var instrumentUrls = new List<string>();
            foreach (HtmlNode inode in instrumentNodes)
            {
                instrumentUrls.Add(startingHTML + inode.InnerText);
            }
            //delete the first url since it's for parent directory
            instrumentUrls.RemoveAt(0);

            //generate list of instrument types urls
            var instrumentTypeUrls = new List<string>();
            var allInstrumentFileUrls = new List<string>();
            var fileTypePattern = @".ad\w.";
            var r = new Regex(fileTypePattern);

            foreach (string url in instrumentUrls)
            {
                var htmlDocInstrument = web.Load(url);
                var instrumentTypeNodes = htmlDocInstrument.DocumentNode.SelectNodes("//tr/td/a/@href");
                foreach (HtmlNode iTNode in instrumentTypeNodes)
                {
                    Match m = r.Match(iTNode.InnerText);
                    if (iTNode.InnerText != "Parent Directory")
                        if (m.Success)                    
                            allInstrumentFileUrls.Add(url + iTNode.InnerText);
                        else
                            instrumentTypeUrls.Add(url + iTNode.InnerText);
                }
            }

            //Grab remaining instrument files

            Console.Write("Current Instrument Type urls");
            foreach (string url in instrumentTypeUrls)
            {
                Console.Write(url + "\n");
            }

            Console.Write("\n Current Instrument Files");
            foreach (string url in allInstrumentFileUrls)
            {
                Console.Write(url + "\n");
            }

            Console.Write("Crawl Finished: ");

            //remove the line below when actually deploying
            Console.ReadLine();
        }
    }
}