using System;
using System.Collections.Generic;
using System.Text;
using ExcelDataReader;
using System.Data;
using System.IO;
using FedEx.Model;

namespace FedEx
{
    class FedExFileReader
    {
        public static DataSet result;
        public static List<TrackingNumbers> trackingNumbers = new List<TrackingNumbers>();
        public static string fileName = "FedEx2.xlsx";

        public static List<TrackingNumbers> ReadDataFile() {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //open file and returns as Stream
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    result = reader.AsDataSet();
                    int i = 0;
                    foreach (DataTable dt in result.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            foreach (DataColumn column in dt.Columns)
                            {
                                string s = row[column].ToString();
                                Console.WriteLine(row[column].ToString());
                                trackingNumbers.Add(new TrackingNumbers() {Id=0, TrackingNumber = s });
                                ++i;
                            }
                        }
                    }
                    reader.Close();
                    return trackingNumbers;
                }
            }
        }
        
    }
}
