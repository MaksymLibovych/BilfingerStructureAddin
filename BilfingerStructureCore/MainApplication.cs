using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Reflection;
using BilfingerStructure.Core.ExternalCommands;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BilfingerStructure.Core
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class MainApplication : IExternalApplication
    {
        // ExternalCommands assembly path
        static readonly string AddInPath = typeof(MainApplication).Assembly.Location;
        // Button icons directory
        static readonly string ButtonIconsFolder = Path.GetDirectoryName(AddInPath);
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                CreateRibbonPanel(application);
                return Result.Succeeded;
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Column reinforcement exception", exception.ToString());
                return Result.Failed;
            }
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        private void CreateRibbonPanel(UIControlledApplication application)
        {
            //Ribbon tab
            var ribbonTabName = "Bilfinger Structure";
            application.CreateRibbonTab(ribbonTabName);

            //Ribbon panel
            var ribbonPanel = application.CreateRibbonPanel(ribbonTabName, "Reinforcement");

            //Push button data
            var buttonData1 = new PushButtonData("buttonData1",
                                                 "Rebar overriding",
                                                 AddInPath,
                                                 "BilfingerStructure.Core.ExternalCommands.RebarOverridingInViewCommand");
            buttonData1.LargeImage = new BitmapImage(
                    new Uri(Path.Combine(ButtonIconsFolder, @"Images\Rebar-32.png"), UriKind.Absolute));
            buttonData1.Image= new BitmapImage(
                    new Uri(Path.Combine(ButtonIconsFolder, @"Images\Rebar-16.png"), UriKind.Absolute));
            buttonData1.ToolTip = "Override rebar visibility";
            buttonData1.LongDescription = "Override rebar visibility in current view";

            //Push button
            var pushButton1 = ribbonPanel.AddItem(buttonData1) as PushButton;
        }
    }
}
