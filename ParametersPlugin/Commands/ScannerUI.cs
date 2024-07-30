using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ParametersPlugin.Commands
{
    /// <summary>
    /// Implements the Scanner Class with the Plugin Commands
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ScannerUI : IExternalCommand
    {
        /// <summary>
        /// Execute Revit API Command - Display Main Window
        /// </summary>
        public Result Execute(ExternalCommandData extCommandData, ref string message, ElementSet elements)
        {
            // Create the MainWindow and pass the Application as argument
            UI.MainWindow mainWindow = new UI.MainWindow(extCommandData.Application);

            mainWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}