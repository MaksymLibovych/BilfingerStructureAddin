using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace BilfingerStructure.Interfaces.RebarOverridingInView
{
    public interface IRebarProjectData
    {
        IList<string> GetRebarPartitions(Document doc);
    }
}
