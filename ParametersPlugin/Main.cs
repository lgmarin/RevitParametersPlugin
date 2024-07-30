using System;
using System.Windows.Media.Imaging;
using System.Reflection;
using Autodesk.Revit.UI;
using System.IO;

namespace ParametersPlugin
{
    public class Main : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create Parameters Ribbon Tab
            string tabName = "Parameters";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel to the new Tab
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Parameters");

            // Create a push button to trigger a command and add it to the ribbon panel.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("cmdParametersScanner",
               "Parameters Scanner", thisAssemblyPath, "ParametersPlugin.Commands.ScannerUI");

            PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            pushButton.ToolTip = "Run the Parameters Scanner on the project.";

            // Add the desired Icon to the button
            Uri uriImage = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "icon_32.png"));
            BitmapImage largeImage = new BitmapImage(uriImage);
            pushButton.LargeImage = largeImage;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
