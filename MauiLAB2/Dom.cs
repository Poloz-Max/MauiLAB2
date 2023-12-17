using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MauiLAB2
{
    public class Dom : IParsStrategyXML
    {
        XmlDocument doc = new XmlDocument();
        public Dom(string xmlFilePath)
        {
            doc.Load(xmlFilePath);
        }
        public List<Scientist> SearchScientistInXmlFile(string enteredphrase, string selectElement, string selectId)
        {
            List<Scientist> scientists = new List<Scientist>();

            string xpathQuery = $"//scientist[{selectElement} = '{enteredphrase}' and @ID = '{selectId}']";
            XmlNodeList scientistNodes = doc.SelectNodes(xpathQuery);

            foreach (XmlNode scientistNode in scientistNodes)
            {
                Scientist scientist = new Scientist
                {
                    Id = scientistNode.Attributes["id"].Value,
                    Name = scientistNode["Name"].InnerText,
                    fathersName = scientistNode["fathersName"].InnerText,
                    surName = scientistNode["surName"].InnerText,
                    faculty = scientistNode["faculty"].InnerText,
                    department = scientistNode["department"].InnerText,
                    birthYear = scientistNode["birthYear"].InnerText,
                    gender = scientistNode["gender"].InnerText,
                    workYears = scientistNode["workYears"].InnerText,
                    financeSupport = scientistNode["financeSupport"].InnerText
                };

                scientists.Add(scientist);
            }

            return scientists;
        }
    }
}
