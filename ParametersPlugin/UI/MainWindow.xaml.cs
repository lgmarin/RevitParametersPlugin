
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ParametersPlugin.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow(UIApplication uiApp)
        {
            InitializeComponent();
            UiApp = uiApp;
            UiDoc = UiApp.ActiveUIDocument;
        }

        private UIApplication UiApp { get; }
        private UIDocument UiDoc { get; }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string parameterName = tbParamName.Text.Trim();
            string parameterValue = tbParamValue.Text.Trim();

            // Execute the Parameter Name validation
            if (ValidateParameterName(parameterName))
                SelectElementsByParameter(parameterName, parameterValue);
        }

        private bool ValidateParameterName(string parameterName)
        {
            if (parameterName.Length == 0)
            {
                TaskDialog.Show("Parameters Plugin", "The Parameter Name should not be empty!");
                return false;
            }

            // Other checks could be added, such as verify for invalid names.

            return true;
        }

        private void SelectElementsByParameter(string parameterName, string parameterValue)
        {

            List<Element> matchingElements = GetElementsList(parameterName, parameterValue);

            if (matchingElements.Count > 0)
            {
                if (parameterValue.Length > 0)
                    TaskDialog.Show("Selection", $"Selected {matchingElements.Count} Elements for:\nParameter: {parameterName}\nValue: {parameterValue}");
                else
                    TaskDialog.Show("Selection", $"Selected {matchingElements.Count} Elements for:\nParameter: {parameterName}");

                UiDoc.Selection.SetElementIds(matchingElements.Select(e => e.Id).ToList());
            }
            else
                TaskDialog.Show("Selection", $"Could not find any Element that contains {parameterName}.");
        }

        List<Element> GetElementsList(string parameterName, string parameterValue)
        {
            Document doc = UiDoc.Document;

            // Get a filter to find all elements in the document
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // Need to pass something to the Collector, or it will raise an Exception
            // https://thebuildingcoder.typepad.com/blog/2010/06/filter-for-all-elements.html
            collector.WherePasses(new LogicalOrFilter(
                new ElementIsElementTypeFilter(false),
                new ElementIsElementTypeFilter(true))
            );

            // List to store elements that match the parameter criteria
            List<Element> matchingElements = new List<Element>();


            foreach (Element element in collector)
            {
                // Verify if the Element contains the Parameter Name
                Parameter param = element.LookupParameter(parameterName);
                if (param != null)
                {
                    string paramVal = GetParameterInformation(param, doc).Trim();

                    // Check if the parameter value matches the given value, or if the parameterValue is empty
                    if (parameterValue.Length == 0 || paramVal.Equals(parameterValue))
                        matchingElements.Add(element);
                }
            }

            return matchingElements;
        }

        String GetParameterInformation(Parameter parameter, Document document)
        {
            string paramValue = string.Empty;

            // Return the Parameter Value according to the Data Type stored in the parameter
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    paramValue = parameter.AsValueString();
                    break;

                case StorageType.ElementId:
                    //Find the corresponding Element based on the ID
                    ElementId id = parameter.AsElementId();
                    if (id.IntegerValue >= 0)
                        paramValue = document.GetElement(id).Name;
                    else
                        paramValue = id.IntegerValue.ToString();
                    break;

                case StorageType.Integer:
                    if (ParameterType.YesNo == parameter.Definition.ParameterType)
                    {
                        if (parameter.AsInteger() == 0)
                            paramValue = "False";
                        else
                            paramValue = "True";
                    }
                    else
                        paramValue = parameter.AsInteger().ToString();
                    break;

                case StorageType.String:
                    paramValue = parameter.AsString();
                    break;

                default:
                    paramValue = "Unexposed parameter";
                    break;
            }

            return paramValue;
        }
    }
}
