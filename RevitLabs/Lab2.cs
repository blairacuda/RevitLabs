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

            var refPick = rvtUIDoc.Selection.PickObject(ObjectType.Element, "Pick an element");
            var elem = m_rvtDoc.GetElement(refPick);
            var elemTypeId = elem.GetTypeId();
            var elemType = (ElementType)m_rvtDoc.GetElement(elemTypeId);

            //ShowBasicElementInfo(elem);
            //IdentifyElement(elem);
            //ShowParameters(elem, "Element Parameters");
            //ShowParameters(elemType, "Type Parameters");

            RetrieveParameter( elem, "Element Parameter (by Name and BuiltInParameter)");
            RetrieveParameter( elemType, "Type Parameter (by Name and BuiltInParameter)");


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

        public void RetrieveParameter(Element elem, string header)
        {
            string s = string.Empty;

            // as an experiment, let's pick up some arbitrary parameters. 
            // comments - most of instance has this parameter 

            // (1) by BuiltInParameter. 
            var param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            if (param != null)
            {
                s += "Comments (by BuiltInParameter) = " +
                ParameterToString(param) + "\n";
            }

            // (2) by name. (Mark - most of instance has this parameter.) 
            // if you use this method, it will language specific. 
            param = elem.LookupParameter("Mark");
            if (param != null)
            {
                s += "Mark (by Name) = " + ParameterToString(param) + "\n";
            }

            // the following should be in most of type parameter 
            // 
            param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS);
            if (param != null)
            {
                s += "Type Comments (by BuiltInParameter) = " +
                ParameterToString(param) + "\n";
            }

            param = elem.LookupParameter("Fire Rating");
            if (param != null)
            {
                s += "Fire Rating (by Name) = " + ParameterToString(param) +
                    "\n";
            }

            // using the BuiltInParameter, you can sometimes access one that is
            // not in the parameters set. 
            // Note: this works only for element type. 

            param = elem.get_Parameter(
             BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM);
            if (param != null)
            {
                s += "SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM (only by BuiltInParameter) = " +
                 ParameterToString(param) + "\n";
            }

            param = elem.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM);
            if (param != null)
            {
                s += "SYMBOL_FAMILY_NAME_PARAM (only by BuiltInParameter) = "
                    + ParameterToString(param) + "\n";
            }

            // show it. 

            TaskDialog.Show(header, s);

        }
    }
}
