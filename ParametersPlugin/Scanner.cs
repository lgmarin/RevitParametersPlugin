using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ParametersPlugin
{
    /// <summary>
    /// Implements the Scanner Class with the Plugin Commands
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Scanner : IExternalCommand
    {
        /// <summary>
        /// Execute Revit API Command
        /// </summary>
        public Result Execute(ExternalCommandData extCommandData, ref string message, ElementSet elements)
        {
            UIApplication uIApplication = extCommandData.Application;
            UI.MainWindow mainWindow = new UI.MainWindow(uIApplication);

            mainWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}