using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;


namespace RestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            PostDefectToRally.LogDefect(args);
        }
    }
}