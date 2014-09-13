using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Server.Controls.Runtime;

namespace Victop.Frame.Component
{
    /// <summary>
    /// UnitRowDataPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UnitRowDataPanel : UserControl
    {
        public UnitRowDataPanel()
        {
            InitializeComponent();
        }
        public void DoRender()
        {
            DefinPluginsModel pluginModel = this.Tag as DefinPluginsModel;
            if (pluginModel.PluginBlock.BlockDt != null && pluginModel.PluginBlock.BlockDt.Rows != null && pluginModel.PluginBlock.BlockDt.Rows.Count > 0)
            {
                panelGrid.Children.Clear();
                foreach (DataColumn item in pluginModel.PluginBlock.BlockDt.Rows[0].Table.Columns)
                {
                    DockPanel dockpanel = new DockPanel();
                    Label itemlabel = new Label();
                    itemlabel.Content = item.ColumnName;
                    dockpanel.Children.Add(itemlabel);
                    TextBox itemtBox = new TextBox();
                    itemtBox.Name = item.ColumnName;
                    itemtBox.Text = pluginModel.PluginBlock.BlockDt.Rows[0][item.ColumnName].ToString();
                    dockpanel.Children.Add(itemtBox);
                    panelGrid.Children.Add(dockpanel);
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            DefinPluginsModel pluginModel = this.Tag as DefinPluginsModel;
            foreach (var item in panelGrid.Children)
            {
                if (item is DockPanel)
                {
                    DockPanel dockpanel = (DockPanel)item;
                    foreach (var childitem in dockpanel.Children)
                    {
                        if (childitem is TextBox)
                        {
                            TextBox textbox = (TextBox)childitem;
                            if (pluginModel.PluginBlock.BlockDt.Columns.Contains(textbox.Name))
                            {
                                pluginModel.PluginBlock.BlockDt.Rows[0][textbox.Name] = textbox.Text;
                            }
                        }
                    }
                }
            }
        }
    }
}
