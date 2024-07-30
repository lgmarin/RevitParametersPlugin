﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParametersPlugin.Commands
{
    /// <summary>
    /// ParameterScanner class contains the logic to find, select and isolate parameters
    /// </summary>
    public class ParametersScanner
    {
        private UIApplication uIApplication;
        private UIDocument uiDocument;
        private Document document;

        /// <summary>
        /// Dictionary to store the Selected Elements, the Id is needed for the Isolation
        /// </summary>
        private Dictionary<ElementId, Element> elementsSelection;

        public ParametersScanner(UIApplication uiApp)
        {
            uIApplication = uiApp;
            uiDocument = uIApplication.ActiveUIDocument;
            document = uiDocument.Document;
            elementsSelection = new Dictionary<ElementId, Element>();
        }

        /// <summary>
        /// Select Elements based on the parameter name and parameter value, if provided.
        /// </summary>
        /// <param name="parameterName">Name for the parameter to be looked for</param>
        /// <param name="parameterValue">Value of the parameter to be looked for</param>
        /// <returns>true if the Selection was successfull, false if no elements were found.</returns>
        public bool SelectElementsByParameter(string parameterName, string parameterValue)
        {
            UpdateElementsList(parameterName, parameterValue);

            if (elementsSelection.Count > 0)
            {
                if (parameterValue.Length > 0)
                    TaskDialog.Show("Selection", $"Selected {elementsSelection.Count} Elements for:\nParameter: {parameterName}\nValue: {parameterValue}");
                else
                    TaskDialog.Show("Selection", $"Selected {elementsSelection.Count} Elements for:\nParameter: {parameterName}");

                uiDocument.Selection.SetElementIds(elementsSelection.Values.Select(e => e.Id).ToList());

                return true;
            }

            TaskDialog.Show("Selection", $"Could not find any Element that contains {parameterName}.");
            return false;
        }

        /// <summary>
        /// Isolate Elements in the current View based on the parameter name and parameter value, if provided.
        /// </summary>
        /// <param name="parameterName">Name for the parameter to be looked for</param>
        /// <param name="parameterValue">Value of the parameter to be looked for</param>
        /// <returns>true if the Selection was successfull, false if no elements were found.</returns>
        public bool IsolateInViewByParameter(string parameterName, string parameterValue)
        {
            Document document = uIApplication.ActiveUIDocument.Document;
            View activeView = document.ActiveView;

            UpdateElementsList(parameterName, parameterValue);

            if (elementsSelection.Count > 0)
            {
                using (Transaction t = new Transaction(document, "IsolateElementsInView"))
                {
                    t.Start();
                    activeView.IsolateElementsTemporary(elementsSelection.Keys);
                    t.Commit();
                }

                if (parameterValue.Length > 0)
                    TaskDialog.Show("Isolate in View", $"Selected {elementsSelection.Count} Elements for:\nParameter: {parameterName}\nValue: {parameterValue}\nActive View: {activeView.Name}");
                else
                    TaskDialog.Show("Isolate in View", $"Selected {elementsSelection.Count} Elements for:\nParameter: {parameterName}\nActive View: {activeView.Name}");

                return true;
            }

            TaskDialog.Show("Isolate in View", $"Could not find any Element that contains {parameterName} in the current view.");
            return false;
        }

        private void UpdateElementsList(string parameterName, string parameterValue)
        {
            // Get a filter to find all elements in the document
            FilteredElementCollector collector = new FilteredElementCollector(document);

            // Need to pass something to the Collector, or it will raise an Exception
            // https://thebuildingcoder.typepad.com/blog/2010/06/filter-for-all-elements.html
            collector.WherePasses(new LogicalOrFilter(
                new ElementIsElementTypeFilter(false),
                new ElementIsElementTypeFilter(true))
            );

            elementsSelection.Clear();

            foreach (Element element in collector)
            {
                // Verify if the Element contains the Parameter Name
                Parameter param = element.LookupParameter(parameterName);
                if (param != null)
                {
                    string paramVal = GetParameterValue(param).Trim();

                    // Check if the parameter value matches the given value, or if the parameterValue is empty
                    if (parameterValue.Length == 0 || paramVal.Equals(parameterValue))
                    {
                        elementsSelection.Add(element.Id, element);
                    }
                }
            }
        }

        /// <summary>
        /// Return the parameter Value in String format.
        /// </summary>
        /// <param name="parameter">Parameter where the Value will be read.</param>
        /// <returns>A String containing the Value.</returns>
        private String GetParameterValue(Parameter parameter)
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
