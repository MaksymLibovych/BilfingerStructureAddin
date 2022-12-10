using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Events;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB.Structure;
using BilfingerStructure.UI.RebarOverridingInView;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using BilfingerStructure.Core.RebarOverridingInView;

namespace BilfingerStructure.Core.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class RebarOverridingInViewCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            try
            {
                var rebarOverridingUI = new RebarOverridingInViewCommandUI(
                    uidoc,
                    new List<IRebarOverridable>()
                    {
                        new RebarSolid(),
                        new RebarUnobscured(),
                        new RebarResetSettings()
                    },
                    new RebarProjectData(doc));

                rebarOverridingUI.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception exception)
            {
                TaskDialog.Show(
                    "RebarOverridingInViewCommand Exception", exception.ToString());

                return Result.Failed;

            }
        }
    }
}
