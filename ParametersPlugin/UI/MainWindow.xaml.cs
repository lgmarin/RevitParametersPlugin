using Autodesk.Revit.UI;
using ParametersPlugin.Commands;
using System.Windows;

namespace ParametersPlugin.UI
{
    public partial class MainWindow : Window
    {
        private ParametersScanner pScanner;

        public MainWindow(UIApplication uIApplication)
        {
            pScanner = new ParametersScanner(uIApplication);
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string parameterName = tbParamName.Text.Trim();
            string parameterValue = tbParamValue.Text.Trim();

            // Execute the Parameter Name validation
            if (ValidateParameterName(parameterName))
                pScanner.SelectElementsByParameter(parameterName, parameterValue);
        }
        private void btnIsolateView_Click(object sender, RoutedEventArgs e)
        {
            string parameterName = tbParamName.Text.Trim();
            string parameterValue = tbParamValue.Text.Trim();

            // Execute the Parameter Name validation
            if (ValidateParameterName(parameterName))
                pScanner.IsolateInViewByParameter(parameterName, parameterValue);
        }

        private bool ValidateParameterName(string parameterName)
        {
            if (parameterName.Length == 0)
            {
                TaskDialog.Show("Parameters Scanner", "The Parameter Name should not be empty!");
                return false;
            }

            // Other checks could be added, such as verify for invalid names.

            return true;
        }

    }
}