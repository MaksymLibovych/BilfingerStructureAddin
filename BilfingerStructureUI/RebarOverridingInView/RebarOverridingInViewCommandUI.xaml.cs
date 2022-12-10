using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using BilfingerStructure.Interfaces.RebarOverridingInView;
using System.Collections.ObjectModel;

namespace BilfingerStructure.UI.RebarOverridingInView
{
    public partial class RebarOverridingInViewCommandUI : Window
    {
        private readonly UIDocument _uidoc;
        private readonly Document _doc;
        private readonly IList<IRebarOverridable> _rebarOverridables;
        private readonly IRebarProjectData _rebarProjectData;
        public RebarOverridingInViewCommandUI(UIDocument uidoc,
                                              IList<IRebarOverridable> rebarOverridables,
                                              IRebarProjectData rebarProjectData)
        {
            _uidoc = uidoc;
            _doc = _uidoc.Document;
            _rebarOverridables = new List<IRebarOverridable>();
            _rebarProjectData = rebarProjectData;
            Partitions = new List<string>();
            CheckedPartitions = new ObservableCollection<string>();

            foreach (var partition in _rebarProjectData.GetRebarPartitions(_doc))
            {
                if (String.IsNullOrEmpty(partition))
                {
                    Partitions.Add("<Unassigned>");
                }
                else
                {
                    Partitions.Add(partition);
                }
            }

            foreach (IRebarOverridable rebarOverridable in rebarOverridables)
            {
                _rebarOverridables.Add(rebarOverridable);
            }

            InitializeComponent();
        }
        public List<string> Partitions { get; set; }
        public ObservableCollection<string> CheckedPartitions { get; set; }
        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is childItem)
                    return (childItem)child;

                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
        private void SolidClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _rebarOverridables.First(o => o.OverrideType == "RebarSolid")
                .OverrideInView(_uidoc);
            }
            catch (Exception exception)
            {
                TaskDialog.Show("Exception", exception.Message);
                MainWindow.Close();
            }
        }
        private void UnobscuredClick(object sender, RoutedEventArgs e)
        {
            _rebarOverridables.First(o => o.OverrideType == "RebarUnobscured")
                .OverrideInView(_uidoc);
        }
        private void ResetSettingsClick(object sender, RoutedEventArgs e)
        {
            _rebarOverridables.First(o => o.OverrideType == "RebarResetSettings")
                .OverrideInView(_uidoc);
        }
        private void PartitionChecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < PartitionsListBox.Items.Count; i++)
            {
                DependencyObject obj = PartitionsListBox.ItemContainerGenerator.ContainerFromIndex(i);

                CheckBox box = FindVisualChild<CheckBox>(obj);

                if (box.IsChecked == true)
                {
                    if (!CheckedPartitions.Contains(box.Content.ToString()))
                    {
                        CheckedPartitions.Add(box.Content.ToString());

                        _rebarOverridables.First(o => o.OverrideType == "RebarSolid")
                            .CheckedPartitions.Add(box.Content.ToString());

                        _rebarOverridables.First(o => o.OverrideType == "RebarUnobscured")
                            .CheckedPartitions.Add(box.Content.ToString());

                        _rebarOverridables.First(o => o.OverrideType == "RebarResetSettings")
                            .CheckedPartitions.Add(box.Content.ToString());
                    }
                }
            }
        }
        private void PartitionUnchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < PartitionsListBox.Items.Count; i++)
            {
                DependencyObject obj = PartitionsListBox.ItemContainerGenerator.ContainerFromIndex(i);

                CheckBox box = FindVisualChild<CheckBox>(obj);

                if (box.IsChecked == false)
                {
                    CheckedPartitions.Remove(box.Content.ToString());

                    _rebarOverridables.First(o => o.OverrideType == "RebarSolid")
                        .CheckedPartitions.Remove(box.Content.ToString());

                    _rebarOverridables.First(o => o.OverrideType == "RebarUnobscured")
                        .CheckedPartitions.Remove(box.Content.ToString());

                    _rebarOverridables.First(o => o.OverrideType == "RebarResetSettings")
                        .CheckedPartitions.Remove(box.Content.ToString());
                }
            }
        }
        
    }
}
