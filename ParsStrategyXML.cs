using System;

namespace MauiLAB2
{
    public interface ParsStrategyXML
    {

        public List<Student> SearchStudentInXmlFile(string enteredphrase, string selectElement, string selectId);

    }
}
