using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MauiLAB2
{
    public class Sax : IParsStrategyXML
    {
        string path;
        Scientist scientist;
        public Sax(string xmlFilePath)
        {
            path = xmlFilePath;
        }
        private XmlTextReader CreateNewXmlReader(string xmlFilePath)
        {
            return new XmlTextReader(xmlFilePath);
        }
        public List<Scientist> SearchScientistInXmlFile(string enteredphrase, string selectElement, string selectId)
        {
            List<Scientist> allScientists = new List<Scientist>();


            using (XmlTextReader xmlReader = CreateNewXmlReader(path))
            {
                while (xmlReader.Read())
                {

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "scientist":
                                scientist = new Scientist();
                                if (xmlReader.MoveToAttribute("ID"))
                                {
                                    scientist.Id = xmlReader.Value;
                                    xmlReader.MoveToElement();
                                };
                                break;
                            case "Name":

                                if (scientist != null)
                                    scientist.Name = xmlReader.ReadElementContentAsString();
                                break;
                            case "fathersName":
                                if (scientist != null)
                                    scientist.fathersName = xmlReader.ReadElementContentAsString();
                                break;
                            case "surName":
                                if (scientist != null)
                                    scientist.surName = xmlReader.ReadElementContentAsString();
                                break;
                            case "faculty":
                                if (scientist != null)
                                    scientist.faculty = xmlReader.ReadElementContentAsString();
                                break;
                            case "department":
                                if (scientist != null)
                                    scientist.department = xmlReader.ReadElementContentAsString();
                                break;
                            case "gender":
                                if (scientist != null)
                                    scientist.gender = xmlReader.ReadElementContentAsString();
                                break;
                            case "workYears":
                                if (scientist != null)
                                    scientist.workYears = xmlReader.ReadElementContentAsString();
                                break;
                            case "financeSupport":
                                if (scientist != null)
                                    scientist.financeSupport = xmlReader.ReadElementContentAsString();
                                break;

                        }


                    }
                    if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "scientist" && scientist != null)
                    {
                        allScientists.Add(scientist);
                    }

                }
            }
            return FilterSelectedScientists(allScientists, enteredphrase, selectElement, selectId);
        }


        public List<Scientist> FilterSelectedScientists(List<Scientist> allScientists, string enteredphrase, string selectElement, string selectId)
        {
            List<Scientist> scientists = new List<Scientist>();
            if (selectElement == "workYears")
            {
                foreach (var scientist in allScientists)
                {
                    if (scientist != null && scientist.workYears == enteredphrase && scientist.Id == selectId)
                    {
                        scientists.Add(scientist);
                    }
                }
            }
            if (selectElement == "department")
            {
                foreach (var scientist in allScientists)
                {
                    if (scientist != null && scientist.department == enteredphrase && scientist.Id == selectId)
                    {
                        scientists.Add(scientist);
                    }
                }
            }

            return scientists;
        }

        public List<Scientist> SearchScientistXmlFile(string enteredphrase, string selectElement, string selectId)
        {
            throw new NotImplementedException();
        }
    }
}
