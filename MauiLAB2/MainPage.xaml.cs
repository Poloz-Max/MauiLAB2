using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MauiLAB2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        string selectXmlFilePath;
        string selectXslFilePath;
        string enteredphrase;
        string selectElement;
        string selectId;
        IParsStrategyXML xmlParser;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void Choose_Xml_File(object sender, EventArgs e)
        {
            var CustomFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.iOS, new[] {"come.adobe.xml"} },
            {DevicePlatform.macOS, new[] {"xml"} },
            {DevicePlatform.Android, new[] {"application/xml"} },
            {DevicePlatform.WinUI, new[] {".xml"} },
            {DevicePlatform.MacCatalyst, new[] {"xml"} },
        });
            FileResult result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an xml file",
                FileTypes = CustomFileType

            });

            if (result != null)
            {
                selectXmlFilePath = result.FullPath;
                PickScientistId();

            }
        }
        private void PickScientistId()
        {
            XDocument xmlDoc = XDocument.Load(selectXmlFilePath);

            var scientistInfo = xmlDoc.XPathSelectElements("/scientistsList/scientist");
            var uniqueId = new HashSet<string>();

            scientistIdPicker.Items.Clear();
            foreach (var scientist in scientistInfo)
            {
                var idAttribute = scientist.Attribute("id");
                if (idAttribute != null && uniqueId.Add(idAttribute.Value))
                {
                    scientistIdPicker.Items.Add(idAttribute.Value);
                }
            }

            scientistIdPicker.IsEnabled = true;
        }
        private void PickScientistElement(string element1, string element2)
        {
            XDocument xmlDoc = XDocument.Load(selectXmlFilePath);

            var scientistInfo = xmlDoc.XPathSelectElements("/scientistsList/scientist");
            var uniqueEl = new HashSet<string>();

            elementPicker.Items.Clear();
            foreach (var scientist in scientistInfo)
            {
                var searchingElement1 = scientist.Element(element1);
                var searchingElement2 = scientist.Element(element2);

                if (searchingElement1 != null && uniqueEl.Add(element1) && searchingElement2 != null && uniqueEl.Add(element2))
                {
                    elementPicker.Items.Add(element1);

                    elementPicker.Items.Add(element2);

                }
            }
            elementPicker.IsEnabled = true;
        }
        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (SaxRadioButton.IsChecked)
            {
                xmlParser = new Sax(selectXmlFilePath);
                string element1 = "birthYear";
                string element2 = "department";
                PickScientistElement(element1, element2);
            }
            if (DomRadioButton.IsChecked)
            {
                xmlParser = new Dom(selectXmlFilePath);
                string element1 = "workYears";
                string element2 = "financeSupport";
                PickScientistElement(element1, element2);

            }
            if (LinqToXmlRadioButton.IsChecked)
            {
                xmlParser = new LinqToXml(selectXmlFilePath);
                string element1 = "Name";
                string element2 = "faculty";
                PickScientistElement(element1, element2);
            }

        }
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            selectElement = elementPicker.SelectedItem as string;
        }
        private void OnIdPickerSelectedIndexChanged(object sender, EventArgs e) 
        {
            selectId = scientistIdPicker.SelectedItem as string;
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            XDocument doc = XDocument.Load(selectXmlFilePath);
            enteredphrase = keywordEntry.Text;
            bool keywordExists = IsEnteredValueOk(enteredphrase, doc, selectElement);
            if (!keywordExists)
            {
                DisplayAlert("Error", $"{enteredphrase} does not exist as {selectElement} in the chosen file! Please try another one.", "OK");
                keywordEntry.Text = string.Empty;
            }
        }

        static bool IsEnteredValueOk(string enteredphrase, XDocument doc, string selectElement)
        {
            return doc.Descendants("scientist").Elements(selectElement).Any(element => element.Value == enteredphrase);
        }


        private async void Add_Xsl_File(object sender, EventArgs e)
        {
            var CustomFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.iOS, new[] {"come.adobe.xsl"} },
            {DevicePlatform.macOS, new[] {"xsl"} },
            {DevicePlatform.Android, new[] {"application/xsl"} },
            {DevicePlatform.WinUI, new[] {".xsl"} },
            {DevicePlatform.MacCatalyst, new[] {"xsl"} },
        });
            FileResult result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an xsl file",
                FileTypes = CustomFileType

            });

            if (result != null)
            {
                selectXslFilePath = result.FullPath;
            }
        }
        private void OnSearchClicked(object sender, EventArgs e)
        {
            List<Scientist> scientists = xmlParser.SearchScientistXmlFile(enteredphrase, selectElement, selectId);

            if (scientists != null)
            {
                DisplayScientistDetails(scientists);
            }
            else
            {
                DisplayAlert("Message", "Scientist is not found! Please check entered data.", "OK");
            }
        }

        private void DisplayScientistDetails(List<Scientist> scientists)
        {
            scientistInfoLayout.Children.Clear();
            foreach (var scientist in scientists)
            {
                var label1 = new Label
                {
                    Text = $"Id: {scientist.Id} \nName: {scientist.Name} \nfathersName: {scientist.fathersName} \nsurName: {scientist.surName} " +
                    $"\nfaculty: {scientist.faculty} \ndepartment: {scientist.department} \nBirth Year: {scientist.birthYear} \ngender: {scientist.workYears} " +
                    $"\nfinanceSupport: {scientist.financeSupport}"

                };
                scientistInfoLayout.Children.Add(label1);
            };
            scientistInfoLayout.IsVisible = true;
        }

        private void Convert_To_HTML(object sender, EventArgs e)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            try
            {
                xslt.Load(selectXslFilePath);
                DisplayAlert("Message", "Your file is successfully converted to HTML.", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error!", "Something went wrong! Failed to convert your xml file to html. Please check the xsl file you are adding.", "OK");
            }
            string htmlFile = "/Users/Макс Полоз/Desktop/scientistsList.html";
            if (htmlFile != null && selectXmlFilePath != null)
            {
                xslt.Transform(selectXmlFilePath, htmlFile);
            }
        }

        private async void OnExitButtonClicked(Object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Message", "Do you really want to exit the program?", "Yes", "No");
            if (result)
            {
                Application.Current.Quit();
            }
        }
        private void OnClearButtonClicked(Object sender, EventArgs e)
        {
            scientistInfoLayout.Children.Clear();
            keywordEntry.Text = string.Empty;
            elementPicker.SelectedIndex = -1;
            scientistIdPicker.SelectedIndex = -1;

        }
    }
}

        
