using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Victop.Wpf.Controls
{
    public static class TreeViewHelper
    {
        public static void BringSelectItemIntoView(this ItemsControl treeView, object item)
        {
            ExpandAndSelectItem(treeView, item);
        }

        public static void ExpandAll(this ItemsControl treeView)
        {
            ExpandSubContainers(treeView);
        }

        public static bool ExpandAndSelectDataRowView(ItemsControl parentContainer, object itemToSelect, string colName)
        {
            if (parentContainer != null)
            {
                foreach (object obj2 in (IEnumerable)parentContainer.Items)
                {
                    if (parentContainer.ItemContainerGenerator.ContainerFromItem(obj2) != null)
                    {
                        TreeViewItem item = parentContainer.ItemContainerGenerator.ContainerFromItem(obj2) as TreeViewItem;
                        if ((obj2 as DataRowView)[colName].ToString().Equals(itemToSelect) && (item != null))
                        {
                            item.IsSelected = true;
                            item.BringIntoView();
                            return true;
                        }
                    }
                }
                foreach (object obj3 in (IEnumerable)parentContainer.Items)
                {
                    if (parentContainer.ItemContainerGenerator.ContainerFromItem(obj3) != null)
                    {
                        TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(obj3) as TreeViewItem;
                        if ((currentContainer != null) && (currentContainer.Items.Count > 0))
                        {
                            bool isExpanded = currentContainer.IsExpanded;
                            currentContainer.IsExpanded = true;
                            if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                            {
                                EventHandler eh = null;
                                eh = delegate
                                {
                                    if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                                    {
                                        if (!ExpandAndSelectDataRowView(currentContainer, itemToSelect, colName))
                                        {
                                            currentContainer.IsExpanded = false;
                                        }
                                        currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                                    }
                                };
                                currentContainer.ItemContainerGenerator.StatusChanged += eh;
                            }
                            else
                            {
                                if (!ExpandAndSelectDataRowView(currentContainer, itemToSelect, colName))
                                {
                                    currentContainer.IsExpanded = isExpanded;
                                    continue;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static bool ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            foreach (object obj2 in (IEnumerable)parentContainer.Items)
            {
                TreeViewItem item = parentContainer.ItemContainerGenerator.ContainerFromItem(obj2) as TreeViewItem;
                if ((obj2 == itemToSelect) && (item != null))
                {
                    item.BringIntoView();
                    return true;
                }
            }
            foreach (object obj3 in (IEnumerable)parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(obj3) as TreeViewItem;
                if ((currentContainer != null) && (currentContainer.Items.Count > 0))
                {
                    bool isExpanded = currentContainer.IsExpanded;
                    currentContainer.IsExpanded = true;
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        EventHandler eh = null;
                        eh = delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                if (!ExpandAndSelectItem(currentContainer, itemToSelect))
                                {
                                    currentContainer.IsExpanded = false;
                                }
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        };
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else
                    {
                        if (!ExpandAndSelectItem(currentContainer, itemToSelect))
                        {
                            currentContainer.IsExpanded = isExpanded;
                            continue;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private static void ExpandSubContainers(ItemsControl parentContainer)
        {
            foreach (object obj2 in (IEnumerable)parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(obj2) as TreeViewItem;
                if ((currentContainer != null) && (currentContainer.Items.Count > 0))
                {
                    currentContainer.IsExpanded = true;
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        EventHandler eh = null;
                        eh = delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                ExpandSubContainers(currentContainer);
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        };
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else
                    {
                        ExpandSubContainers(currentContainer);
                    }
                }
            }
        }

        public static void SetItemSelectedAndExpand(VicTreeView treeList, string fieldName, string fieldValue)
        {
            if (treeList != null)
            {
                treeList.SetItemSelected(fieldName, fieldValue);
                treeList.SetItemExpanded(fieldName, fieldValue);
                treeList.BringSelectItemIntoView(treeList.SelectedItem);
            }
        }
    }
}
