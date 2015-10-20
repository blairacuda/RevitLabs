using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


namespace RevitLabs
{
    /// <summary>
    /// Hello World #1 - A minimum Revit external command. 
    /// </summary>
    [Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    public class HelloWorld : IExternalCommand
    {
        public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show( "My Dialog Title", "Hello World!");

            return Result.Succeeded;
        }
    }

}
