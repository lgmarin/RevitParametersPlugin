using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ParametersPlugin.Commands;
using System.Windows;

namespace ParametersPlugin.UI
{
    public partial class MainWindow : Window
    {
        private ParametersScanner pScanner;
        private UIApplication uiApp;

        public MainWindow(UIApplication uIApplication)
        {
            uiApp = uIApplication;
            pScanner = new ParametersScanner(uIApplication);
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string parameterName = tbParamName.Text.Trim();
            string parameterValue = tbParamValue.Text.Trim();

            // Execute the Parameter Name validation
            if (!ValidateParameterName(parameterName))
                return;

            // Check if the Current View is allowed
            if (!CheckCurrentViewType())
                return;

            if (pScanner.SelectElementsByParameter(parameterName, parameterValue))
                this.Close();
            else
                this.Focus();
        }
        private void btnIsolateView_Click(object sender, RoutedEventArgs e)
        {
            string parameterName = tbParamName.Text.Trim();
            string parameterValue = tbParamValue.Text.Trim();

            // Execute the Parameter Name validation
            if (!ValidateParameterName(parameterName))
                return;

            // Check if the Current View is allowed
            if (!CheckCurrentViewType())
                return;

            if (pScanner.IsolateInViewByParameter(parameterName, parameterValue))
                this.Close();
            else
                this.Focus();
        }

        private bool ValidateParameterName(string parameterName)
        {
            if (parameterName.Length == 0)
            {
                TaskDialog.Show("Parameters Scanner", "The Parameter Name should not be empty!");
                this.Focus();
                return false;
            }

            // Other checks could be added, such as verify for invalid names.

            return true;
        }

        private bool CheckCurrentViewType()
        {
            // Get the active Document and the Active View information
            Document doc = uiApp.ActiveUIDocument.Document;
            View activeView = doc.ActiveView;

            if (activeView.ViewType == ViewType.FloorPlan ||
                activeView.ViewType == ViewType.CeilingPlan ||
                activeView.ViewType == ViewType.ThreeD)
            {
                return true;
            }
            else
            {
                TaskDialog.Show("Parameters Scanner", 
                    $"The {activeView.ViewType.ToString()} view type is not supported!\nSupported View types: Floor Plans, Reflected Ceiling Plans or 3D Views");
                this.Focus();
                return false;
            }
        }

    }
}