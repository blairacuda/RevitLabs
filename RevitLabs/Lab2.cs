using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RevitLabs
{
    [Transaction(TransactionMode.Manual)]
    public class DBElement : IExternalCommand
    {
        //  Member variables 
        Application m_rvtApp;
        Document m_rvtDoc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //  Get the access to the top most objects. 
            var rvtUIApp = commandData.Application;
            var rvtUIDoc = rvtUIApp.ActiveUIDocument;
            m_rvtApp = rvtUIApp.Application;
            m_rvtDoc = rvtUIDoc.Document;

            // (1) pick an object on a screen.
            var refPick = rvtUIDoc.Selection.PickObject( ObjectType.Element, "Pick an element");
            // we have picked something. 
            var elem = m_rvtDoc.GetElement(refPick);
            // (2) let's see what kind of element we got. 
            ShowBasicElementInfo(elem);

            return Result.Succeeded;
        }

        public void ShowBasicElementInfo(Element elem)
        {
            // let's see what kind of element we got. 
            // 
            var s = "You Picked:" + "\n";

            s += " Class name = " + elem.GetType().Name + "\n";
            s += " Category = " + elem.Category.Name + "\n";
            s += " Element id = " + elem.Id.ToString() + "\n" + "\n";

            // and, check its type info. 
            // 
            //Dim elemType As ElementType = elem.ObjectType '' this is obsolete. 
            ElementId elemTypeId = elem.GetTypeId();
            ElementType elemType = (ElementType)m_rvtDoc.GetElement(elemTypeId);

            s += "Its ElementType:" + "\n";
            s += " Class name = " + elemType.GetType().Name + "\n";
            s += " Category = " + elemType.Category.Name + "\n";
            s += " Element type id = " + elemType.Id.ToString() + "\n";

            // finally show it. 

            TaskDialog.Show("Basic Element Info", s);
        }

    }

}
