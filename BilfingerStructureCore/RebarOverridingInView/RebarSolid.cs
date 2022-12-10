using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using Autodesk.Revit.DB.Structure;
using BilfingerStructure.UI.RebarOverridingInView;
using System.Collections.ObjectModel;
using System.Collections;

namespace BilfingerStructure.Core.RebarOverridingInView
{
    public class RebarSolid : IRebarOverridable
    {
        private readonly string _overrideType;
        private ObservableCollection<string> _checkedPartitions;

        public RebarSolid()
        {
            _checkedPartitions = new ObservableCollection<string>();
            _overrideType = "RebarSolid";
        }

        public ObservableCollection<string> CheckedPartitions
        {
            get { return _checkedPartitions; }
            set { _checkedPartitions = value; }
        }
        public string OverrideType
        {
            get { return _overrideType; }
        }

        public void OverrideInView(UIDocument uidoc)
        {
            try
            {
                var doc = uidoc.Document;

                var currentView = uidoc.ActiveView;

                var multiClassFilter = new ElementMulticlassFilter(
                    new List<Type>()
                {
                    typeof(Rebar), typeof(AreaReinforcement), typeof(PathReinforcement)
                });

                var allRebarElementsInView = new FilteredElementCollector(doc, currentView.Id)
                    .WherePasses(multiClassFilter).WhereElementIsNotElementType();

                using (var transaction = new Transaction(doc, "Set rebar solid"))
                {
                    transaction.Start();

                    foreach (var rebarElement in allRebarElementsInView)
                    {
                        DowncastToRebarElement(uidoc, rebarElement);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                throw new IncorrectViewException("Select 3D view to set rebar solid", exception);
            }
        }
        private void DowncastToRebarElement(UIDocument uidoc, Element element)
        {
            var currentView = uidoc.ActiveView;

            Rebar rebar = element as Rebar;
            AreaReinforcement areaReinforcement = element as AreaReinforcement;
            PathReinforcement pathReinforcement = element as PathReinforcement;

            if (rebar != null)
            {
                var rebarPartition = rebar.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (_checkedPartitions.Count == 0)
                {
                    if (!rebar.IsSolidInView(currentView as View3D))
                    {
                        rebar.SetSolidInView(currentView as View3D, true);
                    }
                }
                else
                {
                    if (!rebar.IsSolidInView(currentView as View3D)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        rebar.SetSolidInView(currentView as View3D, true);
                    }
                    else if (!rebar.IsSolidInView(currentView as View3D)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        rebar.SetSolidInView(currentView as View3D, true);
                    }
                }
            }

            if (areaReinforcement != null)
            {
                var rebarPartition = areaReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (_checkedPartitions.Count == 0)
                {
                    if (!areaReinforcement.IsSolidInView(currentView as View3D))
                    {
                        areaReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                }
                else
                {
                    if (!areaReinforcement.IsSolidInView(currentView as View3D)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        areaReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                    else if (!areaReinforcement.IsSolidInView(currentView as View3D)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        areaReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                }
            }

            if (pathReinforcement != null)
            {
                var rebarPartition = pathReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (_checkedPartitions.Count == 0)
                {
                    if (!pathReinforcement.IsSolidInView(currentView as View3D))
                    {
                        pathReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                }
                else
                {
                    if (!pathReinforcement.IsSolidInView(currentView as View3D)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        pathReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                    else if (!pathReinforcement.IsSolidInView(currentView as View3D)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        pathReinforcement.SetSolidInView(currentView as View3D, true);
                    }
                }
            }
        }
    }
}
