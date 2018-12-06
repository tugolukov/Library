using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Library.Parser.Interfaces;

namespace Library.Parser.Services
{
    public class Parser : IParser
    {
        public HtmlNode GetRootNode(string uri)
        {
            var htmlDocument = new HtmlDocument();
            var htmlString = GetStringFromUri(uri);
            htmlDocument.LoadHtml(htmlString);

            var root = htmlDocument.DocumentNode;
            return root;
        }

        public string GetStringFromUri(string uri)
        {
            WebRequest request; 
            WebResponse response;
            try
            {
                request = WebRequest.Create(uri);
                request.Method = "GET";
                response = request.GetResponse();

            }
            catch (Exception)
            {
                request = WebRequest.Create(uri + "?store=1%2c0&group=div_book");
                request.Method = "GET";
                response = request.GetResponse();
            }
            
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();
            
            return result;     
        }

        public string GetNormalizedString(string str)
        {
            return Regex.Replace(str, @"\n", string.Empty).Trim(new char[]{' '});
        }
    }
}