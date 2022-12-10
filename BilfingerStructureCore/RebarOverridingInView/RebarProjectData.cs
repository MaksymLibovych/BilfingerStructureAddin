using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Events;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using System.Collections.ObjectModel;

namespace BilfingerStructure.Core.RebarOverridingInView
{
    public class RebarProjectData : IRebarProjectData
    {
        private readonly Document _doc;
        public RebarProjectData(Document doc)
        {
            _doc = doc;
        }
        public IList<string> GetRebarPartitions(Document doc)
        {
            var schema = NumberingSchema.GetNumberingSchema(
                doc, NumberingSchemaTypes.StructuralNumberingSchemas.Rebar);

            var rebarPartiotions = schema.GetNumberingSequences();

            return rebarPartiotions;
        }
    }
}
