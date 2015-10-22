using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
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
            var refPick = rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element");
            // we have picked something. 
            var elem = m_rvtDoc.GetElement(refPick);
            // (2) let's see what kind of element we got. 
            ShowBasicElementInfo(elem);
            // (3) identify each major types of element. 
            IdentifyElement(elem);
            // (4) first parameters. 
            ShowParameters(elem, "Element Parameters");
            //  check to see its type parameter as well 
            ElementId elemTypeId = elem.GetTypeId();
            ElementType elemType = (ElementType)m_rvtDoc.GetElement(elemTypeId);
            ShowParameters(elemType, "Type Parameters");



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

        // identify the type of the element known to the UI. 
        public void IdentifyElement(Element elem)
        {

            // An instance of a system family has a designated class. 
            // You can use it identify the type of element. 
            // e.g., walls, floors, roofs. 
            // 
            string s = "";

            if (elem is Duct)
            {
                s = "Duct";
            }
            else if (elem is Pipe)
            {
                s = "Pipe";
            }
            else
            {
                s = "Non-Mechanical";
            }

            s = "You have picked: " + s;

            // show it. 
            TaskDialog.Show("Identify Element", s);
        }

        // show all the parameter values of the element 
        public void ShowParameters(Element elem, string header)
        {
            var paramSet = elem.GetOrderedParameters();
            var s = string.Empty;

            foreach (var param in paramSet)
            {
                var name = param.Definition.Name;

                // see the helper function below 
                var val = ParameterToString(param);

                s += name + " = " + val + "\n";
            }

            TaskDialog.Show(header, s);
        }

        // Helper function: return a string from of the given parameter. 
        public static string ParameterToString(Parameter param)
        {

            string val = "none";

            if (param == null)
            {
                return val;
            }

            // to get to the parameter value, we need to pause it depending
            // on its strage type 
            switch (param.StorageType)
            {
                case StorageType.Double:
                    double dVal = param.AsDouble();
                    val = dVal.ToString();
                    break;

                case StorageType.Integer:
                    int iVal = param.AsInteger();
                    val = iVal.ToString();
                    break;

                case StorageType.String:
                    string sVal = param.AsString();
                    val = sVal;
                    break;

                case StorageType.ElementId:
                    ElementId idVal = param.AsElementId();
                    val = idVal.IntegerValue.ToString();
                    break;

                case StorageType.None:
                    break;

                default:
                    break;
            }

            return val;
        }


    }

}
