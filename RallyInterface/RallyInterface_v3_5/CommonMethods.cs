using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace RestAPI
{
    class CommonMethods
    {
        // Converts image to Base 64 Encoded string
        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                // Convert Image to byte[]                
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }

        public static string ExceptionLogDetails(string strExpLofFilePath)
        {
            try
            {
                string[] abc = File.ReadAllLines(strExpLofFilePath);
                Array.Reverse(abc);
                var lst = new List<String>();
                foreach (var item in abc)
                {
                    lst.Add(item);
                    if (item.Contains("Machine Name:"))
                        break;
                }
                lst.Reverse();

                return String.Join(Environment.NewLine,
                         lst.Select(a => String.Join(", ", a)));
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("Failed to Read ClientFramework log:");
                Console.WriteLine(e.Message);
                throw e;
            }

        }

        public static int CountLinesInFile(string f)
        {
            int count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        }

        public static string RTProductOwner(string strModuleName)
        {
            string strreturnvalue = string.Empty;

            switch (strModuleName)
            {
                case "Installer":
                    {
                        strreturnvalue = "Veerendar Nettem";
                        break;
                    }
                case "Client":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "Contact":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "Employee":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "Maintenance":
                    {
                        strreturnvalue = "Veerendar Nettem";
                        break;
                    }
                case "TaxManager":
                    {
                        strreturnvalue = "Nor Hafidzah";
                        break;
                    }
                case "AUTAX":
                    {
                        strreturnvalue = "Sharath Komuravelli";
                        break;
                    }
                case "CACore":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "DesktopAF":
                    {
                        strreturnvalue = "Veerendar Nettem";
                        break;
                    }
                case "Document Manager":
                    {
                        strreturnvalue = "Veerendar Nettem";
                        break;
                    }
                case "Workpapers":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "PracticeManager":
                    {
                        strreturnvalue = "Veerendar Nettem";
                        break;
                    }
                case "TaxManagerPerformance":
                    {
                        strreturnvalue = "Nor Hafidzah";
                        break;
                    }
                case "AssetsLive":
                    {
                        strreturnvalue = "Damodara Vasista";
                        break;
                    }
                case "OnilneAF":
                    {
                        strreturnvalue = "Damodara Vasista";
                        break;
                    }
                case "CompanyDocs":
                    {
                        strreturnvalue = "Preethi Lingala";
                        break;
                    }
                case "OnlineHelp":
                    {
                        strreturnvalue = "Damodara Vasista";
                        break;
                    }
                case "OnTheGo":
                    {
                        strreturnvalue = "Sridhar Nuguru";
                        break;
                    }
                case "SBR":
                    {
                        strreturnvalue = "Sridhar Nuguru";
                        break;
                    }
                case "StatutoryReporter":
                    {
                        strreturnvalue = "Srikanth Meka";
                        break;
                    }
                case "CorporateCompliance":
                    {
                        strreturnvalue = "Damodara Vasista";
                        break;
                    }
                case "AEReporter":
                    {
                        strreturnvalue = "Yee Ling Wong";
                        break;
                    }
                case "AOGLFormats":
                    {
                        strreturnvalue = "Yee Ling Wong";
                        break;
                    }
                case "AOClassicTaxNZ":
                    {
                        strreturnvalue = "Nor Hafidzah";
                        break;
                    }
                case "AETaxNZ":
                    {
                        strreturnvalue = "Nor Hafidzah";
                        break;
                    }
            }
            return strreturnvalue;
        }

        public static string Readxmlvalue(string strXMLFile, string xmlParent, string strElement)
        {
            //Loading XML file
            XDocument xdoc = XDocument.Load(strXMLFile);//XML File Path

            //Getting all child nodes in XML
            XElement Rally_Parent = xdoc.Descendants(xmlParent).Elements().FirstOrDefault();

            //Getting the value from desired Node
            XElement strval = xdoc.Descendants(strElement).FirstOrDefault();
            return strval.Value.ToString();
        }
    }
}
