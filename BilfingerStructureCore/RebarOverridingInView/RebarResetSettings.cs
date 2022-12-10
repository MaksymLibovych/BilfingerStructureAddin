using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilfingerStructure.Core.RebarOverridingInView
{
    public class RebarResetSettings : IRebarOverridable
    {
        private readonly string _overrideType;
        private ObservableCollection<string> _checkedPartitions;

        public RebarResetSettings()
        {
            _overrideType = "RebarResetSettings";
            _checkedPartitions = new ObservableCollection<string>();
        }

        public string OverrideType
        {
            get { return _overrideType; }
        }
        public ObservableCollection<string> CheckedPartitions
        {
            get { return _checkedPartitions; }
            set { _checkedPartitions = value; }
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

                using (var transaction = new Transaction(doc, "Reset rebar settings"))
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
                TaskDialog.Show(
                    "Rebar Reset Settings Exception", exception.ToString());
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

                if (currentView.ViewType == ViewType.ThreeD)
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (rebar.IsSolidInView(currentView as View3D)
                            || rebar.IsUnobscuredInView(currentView))
                        {
                            rebar.SetSolidInView(currentView as View3D, false);
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if ((rebar.IsSolidInView(currentView as View3D) || rebar.IsUnobscuredInView(currentView))
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            rebar.SetSolidInView(currentView as View3D, false);
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                        else if ((rebar.IsSolidInView(currentView as View3D) || rebar.IsUnobscuredInView(currentView))
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            rebar.SetSolidInView(currentView as View3D, false);
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                    }
                }
                else
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (rebar.IsUnobscuredInView(currentView))
                        {
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if (rebar.IsUnobscuredInView(currentView)
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                        else if (rebar.IsUnobscuredInView(currentView)
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            rebar.SetUnobscuredInView(currentView, false);
                        }
                    }
                }

            }

            if (areaReinforcement != null)
            {
                var rebarPartition = areaReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (currentView.ViewType == ViewType.ThreeD)
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (areaReinforcement.IsSolidInView(currentView as View3D)
                            || areaReinforcement.IsUnobscuredInView(currentView))
                        {
                            areaReinforcement.SetSolidInView(currentView as View3D, false);
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if ((areaReinforcement.IsSolidInView(currentView as View3D) || areaReinforcement.IsUnobscuredInView(currentView))
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            areaReinforcement.SetSolidInView(currentView as View3D, false);
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                        else if ((areaReinforcement.IsSolidInView(currentView as View3D) || areaReinforcement.IsUnobscuredInView(currentView))
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            areaReinforcement.SetSolidInView(currentView as View3D, false);
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                }
                else
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (areaReinforcement.IsUnobscuredInView(currentView))
                        {
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if (areaReinforcement.IsUnobscuredInView(currentView)
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                        else if (areaReinforcement.IsUnobscuredInView(currentView)
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            areaReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                }
            }

            if (pathReinforcement != null)
            {
                var rebarPartition = pathReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (currentView.ViewType == ViewType.ThreeD)
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (pathReinforcement.IsSolidInView(currentView as View3D)
                            || pathReinforcement.IsUnobscuredInView(currentView))
                        {
                            pathReinforcement.SetSolidInView(currentView as View3D, false);
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if ((pathReinforcement.IsSolidInView(currentView as View3D) || pathReinforcement.IsUnobscuredInView(currentView))
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            pathReinforcement.SetSolidInView(currentView as View3D, false);
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                        else if ((pathReinforcement.IsSolidInView(currentView as View3D) || pathReinforcement.IsUnobscuredInView(currentView))
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            pathReinforcement.SetSolidInView(currentView as View3D, false);
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                }
                else
                {
                    if (_checkedPartitions.Count == 0)
                    {
                        if (pathReinforcement.IsUnobscuredInView(currentView))
                        {
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                    else
                    {
                        if (pathReinforcement.IsUnobscuredInView(currentView)
                            && String.IsNullOrEmpty(rebarPartition)
                            && _checkedPartitions.Contains("<Unassigned>"))
                        {
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                        else if (pathReinforcement.IsUnobscuredInView(currentView)
                            && _checkedPartitions.Contains(rebarPartition)
                            && !String.IsNullOrEmpty(rebarPartition))
                        {
                            pathReinforcement.SetUnobscuredInView(currentView, false);
                        }
                    }
                }
            }
        }
    }
}
