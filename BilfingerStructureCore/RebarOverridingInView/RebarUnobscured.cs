using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace BilfingerStructure.Core.RebarOverridingInView
{
    public class RebarUnobscured : IRebarOverridable
    {
        private readonly string _overrideType;
        private ObservableCollection<string> _checkedPartitions;

        public RebarUnobscured()
        {
            _checkedPartitions = new ObservableCollection<string>();
            _overrideType = "RebarUnobscured";
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

                using (var transaction = new Transaction(doc, "Set rebar unobscured"))
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
                    "Rebar unobscured Exception", exception.ToString());
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
                    if (!rebar.IsUnobscuredInView(currentView))
                    {
                        rebar.SetUnobscuredInView(currentView, true);
                    }
                }
                else
                {
                    if (!rebar.IsUnobscuredInView(currentView)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        rebar.SetUnobscuredInView(currentView, true);
                    }
                    else if (!rebar.IsUnobscuredInView(currentView)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        rebar.SetUnobscuredInView(currentView, true);
                    }
                }
            }

            if (areaReinforcement != null)
            {
                var rebarPartition = areaReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (_checkedPartitions.Count == 0)
                {
                    if (!areaReinforcement.IsUnobscuredInView(currentView))
                    {
                        areaReinforcement.SetUnobscuredInView(currentView, true);
                    }
                }
                else
                {
                    if (!areaReinforcement.IsUnobscuredInView(currentView)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        areaReinforcement.SetUnobscuredInView(currentView, true);
                    }
                    else if (!areaReinforcement.IsUnobscuredInView(currentView)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        areaReinforcement.SetUnobscuredInView(currentView, true);
                    }
                }
            }

            if (pathReinforcement != null)
            {
                var rebarPartition = pathReinforcement.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).AsString();

                if (_checkedPartitions.Count == 0)
                {
                    if (!pathReinforcement.IsUnobscuredInView(currentView))
                    {
                        pathReinforcement.SetUnobscuredInView(currentView, true);
                    }
                }
                else
                {
                    if (!pathReinforcement.IsUnobscuredInView(currentView)
                        && String.IsNullOrEmpty(rebarPartition)
                        && _checkedPartitions.Contains("<Unassigned>"))
                    {
                        pathReinforcement.SetUnobscuredInView(currentView, true);
                    }
                    else if (!pathReinforcement.IsUnobscuredInView(currentView)
                        && _checkedPartitions.Contains(rebarPartition)
                        && !String.IsNullOrEmpty(rebarPartition))
                    {
                        pathReinforcement.SetUnobscuredInView(currentView, true);
                    }
                }
            }

            
        }
    }
}



