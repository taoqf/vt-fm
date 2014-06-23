using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
using System.Xml;

namespace Victop.Wpf.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls;assembly=Victop.Wpf.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:VicDataGrid/>
    ///
    /// </summary>
    public class VicDataGrid : DataGrid
    {
        //public static DependencyProperty LayOutFileNameProperty = DependencyProperty.Register("LayOutFileName", typeof(string), typeof(VicDataGrid), new UIPropertyMetadata(string.Empty));

        //[DefaultValue("")]
        //[Description("获取或设置布局文件名称")]
        //public string LayOutFileName
        //{
        //    get { return (string)GetValue(LayOutFileNameProperty); }

        //    set { SetValue(LayOutFileNameProperty, value); }
        //}

        /// <summary>
        /// xml文件路径
        /// </summary>
        private string fullName;

        ContextMenu cm;
        static VicDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicDataGrid), new FrameworkPropertyMetadata(typeof(VicDataGrid)));
        }
        public VicDataGrid()
        {
            this.LoadingRow += VicDataGrid_LoadingRow;
            this.MouseRightButtonDown += VicDataGrid_MouseRightButtonDown;
            //this.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            //this.SelectionMode = DataGridSelectionMode.Extended;
            //this.SelectionUnit = DataGridSelectionUnit.Cell;
            //this.SelectedCellsChanged += VicDataGrid_SelectedCellsChanged;
        }

        private void VicDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            bool isEdit = false;
            switch (currentCell.Column.GetType().Name)
            {
                case "VicDataGridDatePickerColumn":
                    isEdit = true;
                    break;
                case "VicDataGridCheckBoxColumn":
                    isEdit = true;
                    break;
                case "VicDataGridComboBoxColumn":
                    isEdit = true;
                    break;
                case "VicDataGridTextBoxDataRefColumn":
                    isEdit = true;
                    break;
            }
            if (isEdit)
            {
                this.BeginEdit();    //  进入编辑模式  这样单击一次就可以选择ComboBox里面的值了  
            }
            DataRowView drv= (DataRowView)this.SelectedCells[0].Item;

            DataView dv = (DataView)this.ItemsSource;
            DataTable dt = dv.Table;
            int rowIndex = dt.Rows.IndexOf(drv.Row);
            this.SelectedIndex = rowIndex;
            this.SelectedItem = drv;
        }
        /// <summary>增加行号</summary>
        private void VicDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;    //设置行表头的内容值  
        }
        /// <summary>增加右键菜单</summary>
        private void VicDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            cm = new ContextMenu();
            for (int i = 0; i < this.Columns.Count; i++)
            {
                CheckBox cb = new CheckBox();
                for (int j = 0; j < this.Columns.Count; j++)
                {
                    if (i == this.Columns[j].DisplayIndex)
                    {
                        cb.Content = this.Columns[j].Header.ToString();
                        bindColumnAndCheckbox(this.Columns[j], cb);
                    }
                }
                cm.Items.Add(cb);
            }
            Button btnSaveLayout = new Button();
            btnSaveLayout.Content = "保存列表布局";
            btnSaveLayout.Click += btnSaveLayout_Click;
            cm.Items.Add(btnSaveLayout);

            Button btnClearLayout = new Button();
            btnClearLayout.Content = "清除列表布局";
            btnClearLayout.Click += btnClearLayout_Click;
            cm.Items.Add(btnClearLayout);
            this.ContextMenu = cm;
        }
        /// <summary>清除列表布局</summary>
        private void btnClearLayout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(fullName))
                {
                    File.Delete(fullName);
                }
                cm.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 将DataGrid中的列的Visibility属性和CheckBox的IsCheck属性绑定
        /// </summary>
        /// <param name="column">绑定的列名</param>
        /// <param name="checkbox">显示的CheckBox</param>
        private void bindColumnAndCheckbox(object column, CheckBox checkbox)
        {
            Binding binding = new Binding("Visibility");
            binding.Source = column;
            binding.Converter = new ColumnVisibilityToBoolConverter(); // 设定Converter
            checkbox.SetBinding(CheckBox.IsCheckedProperty, binding);
        }

        /// <summary>保存列表布局</summary>
        private void btnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fullName)) return;
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode xmlNode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                XmlNode rootNode = xmlDoc.CreateNode(XmlNodeType.Element, "LayOutInfo", "");
                if (!System.IO.File.Exists(fullName))
                {
                    xmlDoc.AppendChild(xmlNode);
                    xmlDoc.AppendChild(rootNode);
                    XmlElement Columns = xmlDoc.CreateElement("Columns");
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        XmlElement Column = xmlDoc.CreateElement("Column");

                        XmlAttribute Name = xmlDoc.CreateAttribute("Name");
                        string columnName = GetColumnDataPropertyName(this.Columns[i]);//列名
                        if (string.IsNullOrWhiteSpace(columnName)) return;
                        Name.Value = columnName;//列名
                        Column.Attributes.Append(Name);

                        XmlAttribute Header = xmlDoc.CreateAttribute("Header");
                        Header.Value = this.Columns[i].Header.ToString();
                        Column.Attributes.Append(Header);

                        XmlAttribute Visibility = xmlDoc.CreateAttribute("Visibility");
                        Visibility.Value = this.Columns[i].Visibility.ToString();
                        Column.Attributes.Append(Visibility);

                        XmlAttribute DisplayIndex = xmlDoc.CreateAttribute("DisplayIndex");
                        DisplayIndex.Value = this.Columns[i].DisplayIndex.ToString();
                        Column.Attributes.Append(DisplayIndex);

                        XmlAttribute Width = xmlDoc.CreateAttribute("Width");
                        Width.Value = this.Columns[i].Width.DisplayValue.ToString();
                        Column.Attributes.Append(Width);

                        XmlAttribute IsReadOnly = xmlDoc.CreateAttribute("IsReadOnly");
                        IsReadOnly.Value = this.Columns[i].IsReadOnly.ToString();
                        Column.Attributes.Append(IsReadOnly);

                        XmlAttribute SortDirection = xmlDoc.CreateAttribute("SortDirection");
                        SortDirection.Value = this.Columns[i].SortDirection.ToString();
                        Column.Attributes.Append(SortDirection);
                        Columns.AppendChild(Column);
                    }
                    rootNode.AppendChild(Columns);
                    if (!Directory.Exists("GridLayOutConfig"))//若文件夹不存在则新建文件夹   
                    {
                        Directory.CreateDirectory("GridLayOutConfig");
                    }
                }
                else
                {
                    xmlDoc.Load(fullName);
                    XmlNodeList list = xmlDoc.GetElementsByTagName("Columns");
                    foreach (XmlElement element in list[0].ChildNodes)
                    {
                        string Name = element.GetAttribute("Name").ToString();
                        for (int i = 0; i < this.Columns.Count; i++)
                        {
                            if (Name == this.Columns[i].SortMemberPath)
                            {
                                element.SetAttribute("Header", this.Columns[i].Header.ToString());
                                element.SetAttribute("Visibility", this.Columns[i].Visibility.ToString());
                                element.SetAttribute("DisplayIndex", this.Columns[i].DisplayIndex.ToString());
                                element.SetAttribute("Width", this.Columns[i].Width.DisplayValue.ToString());
                                element.SetAttribute("IsReadOnly", this.Columns[i].IsReadOnly.ToString());
                                element.SetAttribute("SortDirection", this.Columns[i].SortDirection.ToString());
                            }
                        }
                    }
                }
                xmlDoc.Save(fullName);
                cm.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取或设置数据源属性的名称或与 DataGrid 绑定的数据库列的名称
        /// </summary>
        /// <returns></returns>
        private string GetColumnDataPropertyName(DataGridColumn column)
        {
            string dataPropertyName = string.Empty;
            string columnName = column.ToString();
            Binding binding=null;
            switch (columnName)
            {
                case "System.Windows.Controls.DataGridTextColumn":
                    DataGridTextColumn columnText = (DataGridTextColumn)column;
                    binding = (Binding)columnText.Binding;
                    break;
                case "System.Windows.Controls.DataGridCheckBoxColumn":
                    DataGridCheckBoxColumn columnCheckBox = (DataGridCheckBoxColumn)column;
                    binding = (Binding)columnCheckBox.Binding;
                    break;
                case "System.Windows.Controls.DataGridComboBoxColumn":
                    DataGridComboBoxColumn columnComboBox = (DataGridComboBoxColumn)column;
                    binding = (Binding)columnComboBox.SelectedValueBinding;
                    if (binding == null) binding = (Binding)columnComboBox.SelectedItemBinding;
                    break;
                case "System.Windows.Controls.DataGridTemplateColumn":
                    DataGridTemplateColumn columnTemplate = (DataGridTemplateColumn)column;
                    DataTemplate template = columnTemplate.CellTemplate;
                    if (template != null)
                    {
                        TextBlock tblock = (TextBlock)template.LoadContent();
                        binding = BindingOperations.GetBinding(tblock, TextBlock.TextProperty);
                    }
                    break;
                case "Victop.Wpf.Controls.VicDataGridTextColumn":
                    VicDataGridTextColumn viccolumnText = (VicDataGridTextColumn)column;
                    binding = (Binding)viccolumnText.Binding;
                    break;
                case "Victop.Wpf.Controls.VicDataGridCheckBoxColumn":
                    VicDataGridCheckBoxColumn viccolumnCheckBox = (VicDataGridCheckBoxColumn)column;
                    binding = (Binding)viccolumnCheckBox.Binding;
                    break;
                case "Victop.Wpf.Controls.VicDataGridComboBoxColumn":
                    VicDataGridComboBoxColumn viccolumnComboBox = (VicDataGridComboBoxColumn)column;
                    binding = (Binding)viccolumnComboBox.SelectedValueBinding;
                    if (binding == null) binding = (Binding)viccolumnComboBox.SelectedItemBinding;
                    break;
                case "Victop.Wpf.Controls.VicDataGridDatePickerColumn":
                    VicDataGridDatePickerColumn viccolumnDate = (VicDataGridDatePickerColumn)column;
                    binding = (Binding)viccolumnDate.Binding;
                    break;
                case "Victop.Wpf.Controls.VicDataGridNumericUpDownColumn":
                    VicDataGridNumericUpDownColumn viccolumnNumeric = (VicDataGridNumericUpDownColumn)column;
                    binding = (Binding)viccolumnNumeric.Binding;
                    break;
                case "Victop.Wpf.Controls.VicDataGridTextBoxDataRefColumn":
                    VicDataGridTextBoxDataRefColumn viccolumnDataRef = (VicDataGridTextBoxDataRefColumn)column;
                    binding = (Binding)viccolumnDataRef.Binding;
                    break;
            }
            if (binding != null)
                dataPropertyName = binding.Path.Path.ToString();
            return dataPropertyName;
        }
        /// <summary>
        /// 读取XML文件中的列表布局信息
        /// </summary>
        /// <param name="filename">列表布局文件名称</param>
        public void ReadXmlFileLayOutInfo(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName)) return;
                fullName = "GridLayOutConfig//" + fileName + ".xml";
                if (!System.IO.File.Exists(fullName)) return;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fullName);
                XmlNodeList list = xmlDoc.GetElementsByTagName("Columns");
                foreach (XmlElement element in list[0].ChildNodes)
                {
                    string Name = element.GetAttribute("Name").ToString();
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        if (Name == this.Columns[i].SortMemberPath)
                        {
                            this.Columns[i].Header = element.GetAttribute("Header").ToString();
                            this.Columns[i].Visibility = (Visibility)Enum.Parse(typeof(Visibility), element.GetAttribute("Visibility").ToString(), true);
                            this.Columns[i].DisplayIndex = Convert.ToInt32(element.GetAttribute("DisplayIndex").ToString());
                            string sort = element.GetAttribute("SortDirection").ToString();
                            if (!string.IsNullOrWhiteSpace(sort))
                            {
                                this.Columns[i].SortDirection = (ListSortDirection)Enum.Parse(typeof(ListSortDirection), sort, true);
                            }
                            this.Columns[i].Width = Convert.ToDouble(element.GetAttribute("Width").ToString());
                            this.Columns[i].IsReadOnly = Convert.ToBoolean(element.GetAttribute("IsReadOnly").ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}

