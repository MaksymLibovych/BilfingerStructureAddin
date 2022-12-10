using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BilfingerStructure.Interfaces.RebarOverridingInView
{
    public interface IRebarOverridable
    {
        string OverrideType { get; }
        ObservableCollection<string> CheckedPartitions { get; set; }
        void OverrideInView(UIDocument uidoc);
    }
}
