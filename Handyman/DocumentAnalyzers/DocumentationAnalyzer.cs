using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Handyman.DocumentAnalyzers
{
    public class DocumentationAnalyzer
    {
        private XDocument doc;

        public DocumentationAnalyzer(string documentationXml)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(documentationXml))
                {
                    this.doc = XDocument.Parse(documentationXml, LoadOptions.None);
                }
            }
            catch (Exception)
            {
                this.doc = null;
            }
        }

        public string GetParameter(string name)
        {
            return Trim(this.doc?.Descendants(XName.Get("param"))
                .FirstOrDefault(e => e.FirstAttribute?.Value == name)?
                .Value ?? string.Empty);
        }

        public string Returns
        {
            get
            {
                return Trim(this.doc?.Descendants(XName.Get("returns"))
                    .FirstOrDefault()?
                    .Value ?? string.Empty);
            }
        }

        public string Summary
        {
            get
            {
                return Trim(this.doc?.Descendants(XName.Get("summary"))
                    .FirstOrDefault()?
                    .Value ?? string.Empty);
            }
        }

        private static string Trim(string value)
        {
            return value.Trim(' ', '\t', '\r', '\n');
        }
    }
}
