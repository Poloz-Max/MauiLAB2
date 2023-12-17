using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLAB2
{
    public interface IParsStrategyXML
    {

        public List<Scientist> SearchScientistXmlFile(string enteredphrase, string selectElement, string selectId);

    }
}
