using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MauiLAB2
{
    public class LinqToXml: IParsStrategyXML
    {
        XDocument doc;
        public LinqToXml(string xmlFilePath)
        {
            doc = XDocument.Load(xmlFilePath);
        }

        public List<Scientist> SearchStudentInXmlFile(string enteredphrase, string selectElement, string selectId)
        {
            var query = from d in doc.Descendants("scientist")
                        where (d.Element(selectElement).Value == enteredphrase && d.Attribute("ID").Value == selectId)
                        select new Scientist
                        {
                            Id = d.Attribute("id")?.Value,
                            Name = d.Element("Name")?.Value,
                            fathersName = d.Element("fathersName")?.Value,
                            surName = d.Element("surName")?.Value,
                            faculty = d.Element("faculty")?.Value,
                            department = d.Element("department")?.Value,
                            birthYear = d.Element("birthYear")?.Value,
                            gender = d.Element("gender")?.Value,
                            workYears = d.Element("workYears")?.Value,
                            financeSupport = d.Element("financeSupport")?.Value

                        };

            List<Scientist> scientists = query.ToList();
            return scientists;

        }
    }
}

