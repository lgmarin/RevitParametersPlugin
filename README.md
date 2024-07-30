# Revit Parameters Scanner

## _A Revit plugin to Select or Isolate Elements based on it's parameters_

## Installation

To install the plugin on Revit, follow the instructions bellow.

1. Extract the Parameters Scanner zip file to the **Revit 2020 Addin** folder inside the User Roaming folder
    > The Roaming folder is located in `C:\Users\<YourUser>\AppData\Roaming\Autodesk\Revit\Addins\2020`

    > You can also access this folder by entering `%APPDATA%\Autodesk\Revit\Addins\2020`

    Inside this folder, should be the **ParameterScanner.addin** file and the **ParameterScanner** folder.

2. Open **Revit 2020**, at first it will say that the new plugin was found, Allow it to be executed.

3. The **Parameters Scanner** tab should be available inside the project

## Usage

On the Revit Ribbon, go to the **Parameters Scanner** Tab and click on the button to open the Scanner Window.

On the Windows, you can enter the Parameter Name and Value that you want to find.
> If you leave the **Parameter Value** empty, the Scanner will look for all the Parameters that match the name, no matter what the value is.

You have two options to visualize the Elements that matches the Parameter Name and Value criteria.

1. Select

This option will select all the Elements that matches the criteria, this will apply to the current View that is open.

2. Isolate in View

This option will isolate the Elements that matches the criteria, all othere elements will be hidden in a Temporary View.

If you want to exit this view, do a Undo (Ctrl + Z) operation.

## Technologies

Built using the Autodesk Revit 2020 API. Developed in C# .NET Framework 4.8.1 using WPF Window Controls.