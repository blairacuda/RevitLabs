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


    [Transaction(TransactionMode.ReadOnly)]
    public class CommandData : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var rvtUiApp = commandData.Application;
            var rvtApp = rvtUiApp.Application;
            var rvtUiDoc = rvtUiApp.ActiveUIDocument;
            var rvtDoc = rvtUiDoc.Document;

            var versionName = rvtApp.VersionName;
            var documentTitle = rvtDoc.Title;

            TaskDialog.Show( "Revit Intro Lab", "Version Name = " + versionName + "\nDocument Title = " + documentTitle);

            var collector = new FilteredElementCollector(rvtDoc);
            collector.OfClass(typeof(WallType));
            var s = "";
            foreach (WallType wallType in collector)
            {
                s += wallType.Name + "\r\n";
            }

            TaskDialog.Show( "Revit Intro Lab", "Wall Types (in main instruction):\n\n" + s);


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
            //TaskDialog.Show( "My Dialog Title", "Hello World from App!" );
            return Result.Succeeded;
        }
    }
}
