using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


namespace RevitLabs
{
    /// <summary>
    /// A minimum Revit external command. 
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class HelloWorld : IExternalCommand
    {
        public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show( "My Dialog Title", "Hello World!");

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// A minimum Revit external application. 
    /// </summary>
    public class HelloWorldApp : IExternalApplication
    {
        // OnShutdown() - called when Revit ends.  
        public Result OnShutdown( UIControlledApplication application )
        {
            return Result.Succeeded;
        }

        // OnStartup() - called when Revit starts. 
        public Result OnStartup( UIControlledApplication application )
        {
            TaskDialog.Show( "My Dialog Title", "Hello World from App!" );
            return Result.Succeeded;
        }
    }
}
