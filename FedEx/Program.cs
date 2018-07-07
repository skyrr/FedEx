using ExcelDataReader;
using FedEx.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace FedEx
{
    class Program
    {
        public static List<TrackingNumbers> trackingNumbers;
        public static int TrackingNumbersQuantity = 5;
        public static string BasicUrl = "https://www.fedex.com/apps/fedextrack/?action=track&trackingnumber=";
        public static string CompleteResponce;
        public static string content;
        public static HttpWebResponse response;
        public static RequestManager requestManager = new RequestManager();
        public static List<TrackingStatus> trackingStatus = new List<TrackingStatus>();

        static void Main(string[] args)
        {   
            trackingNumbers = FedExFileReader.ReadDataFile();
            RequestIteration(TrackingNumbersQuantity);
            string resultHtml = ResultReader();
            ParseResult(resultHtml);
            ResultFileWriter(trackingStatus);
            //var ts = trackingStatus[0];
            Console.WriteLine("tracking status last - " + trackingStatus.Count);
            Console.ReadKey();
        }

        private static void ParseResult(string resultHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(resultHtml);

            var htmlNodesNumbers = htmlDoc.DocumentNode.SelectNodes("//tr/td/div/div/a");
            var htmlNodesStatus = htmlDoc.DocumentNode.SelectNodes("//tr/td[2]/div/span[3]");
            var numbersQuantity = htmlNodesNumbers.Count;
            int startNumber = 0;
            var trNumber = "";
            var trStatus = "";
            while (startNumber < numbersQuantity) {
                trNumber = htmlNodesNumbers[startNumber].InnerHtml;
                trStatus = htmlNodesStatus[startNumber].InnerHtml;
                Console.Write(trNumber + " ");
                Console.Write(trStatus);
                Console.WriteLine();
                trackingStatus.Add(new TrackingStatus() { TrackingNumber = trNumber, DeliveryStatus = trStatus });
                startNumber++;
            }
        }

        private static void ResultFileWriter(List<TrackingStatus> trackingStatus)
        {
        }

        private static string ResultReader()
        {
            string path = "htmlResult.html";
            using (FileStream fs = File.Open(path, FileMode.Open)) {
                byte[] b = new byte[1024];
                string s = "";
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0) {
                    s += temp.GetString(b);
                }
                return s;
            }
        }

        private static void RequestIteration(int TrackingNumbersQuantity)
        {
            int len = trackingNumbers.Count;
            int groupIndex = 0;
            int index = 0;
            string numbersList = "";
            Console.WriteLine(len);
            while (groupIndex < len)
            {
                content = "";
                numbersList = "";
                Console.WriteLine("print " + TrackingNumbersQuantity + " items: ");
                while (index < TrackingNumbersQuantity)
                {
                    if (index == len)
                        break;
                    else {
                        Console.WriteLine(trackingNumbers[index].TrackingNumber);
                        numbersList += trackingNumbers[index].TrackingNumber + ",";
                    }
                    ++index;
                }
                response = requestManager.SendGETRequest(BasicUrl + numbersList, null, null, false);
                content += requestManager.GetResponseContent(response);
                Console.WriteLine(BasicUrl + numbersList);
                Console.WriteLine(numbersList);
                groupIndex += 5;
                TrackingNumbersQuantity += 5;
                Console.WriteLine(groupIndex);
            }
        }
    }
}
