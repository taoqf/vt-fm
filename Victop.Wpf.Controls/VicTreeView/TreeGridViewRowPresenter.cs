using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    public class TreeGridViewRowPresenter : GridViewRowPresenter
    {
        private static PropertyInfo ActualIndexProperty = typeof(GridViewColumn).GetProperty("ActualIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        private UIElementCollection childs;
        private static PropertyInfo DesiredWidthProperty = typeof(GridViewColumn).GetProperty("DesiredWidth", BindingFlags.NonPublic | BindingFlags.Instance);
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty ExpanderProperty = DependencyProperty.Register("Expander", typeof(UIElement), typeof(TreeGridViewRowPresenter), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(TreeGridViewRowPresenter.OnExpanderChanged)));
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static DependencyProperty FirstColumnIndentProperty = DependencyProperty.Register("FirstColumnIndent", typeof(double), typeof(TreeGridViewRowPresenter), new PropertyMetadata(0.0));

        public TreeGridViewRowPresenter()
        {
            this.childs = new UIElementCollection(this, this);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Size size = base.ArrangeOverride(arrangeSize);
            if ((base.Columns != null) && (base.Columns.Count != 0))
            {
                UIElement expander = this.Expander;
                double x = 0.0;
                double width = arrangeSize.Width;
                for (int i = 0; i < base.Columns.Count; i++)
                {
                    GridViewColumn column = base.Columns[i];
                    UIElement visualChild = (UIElement)base.GetVisualChild((int)ActualIndexProperty.GetValue(column, null));
                    double num4 = Math.Min(width, double.IsNaN(column.Width) ? ((double)DesiredWidthProperty.GetValue(column, null)) : column.Width);
                    if ((i == 0) && (expander != null))
                    {
                        double num5 = this.FirstColumnIndent + expander.DesiredSize.Width;
                        visualChild.Arrange(new Rect(x + num5, 0.0, ((num4 - num5) < 0.0) ? 0.0 : (num4 - num5), arrangeSize.Height));
                    }
                    else
                    {
                        visualChild.Arrange(new Rect(x, 0.0, num4, arrangeSize.Height));
                    }
                    width -= num4;
                    x += num4;
                }
                if (expander != null)
                {
                    expander.Arrange(new Rect(this.FirstColumnIndent, 0.0, expander.DesiredSize.Width, expander.DesiredSize.Height));
                }
            }
            return size;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < base.VisualChildrenCount)
            {
                return base.GetVisualChild(index);
            }
            return this.Expander;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = base.MeasureOverride(constraint);
            UIElement expander = this.Expander;
            if (expander != null)
            {
                expander.Measure(constraint);
                size.Width = Math.Max(size.Width, expander.DesiredSize.Width);
                size.Height = Math.Max(size.Height, expander.DesiredSize.Height);
            }
            return size;
        }

        private static void OnExpanderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeGridViewRowPresenter presenter = (TreeGridViewRowPresenter)d;
            presenter.childs.Remove(e.OldValue as UIElement);
            presenter.childs.Add((UIElement)e.NewValue);
        }

        public UIElement Expander
        {
            get
            {
                return (UIElement)base.GetValue(ExpanderProperty);
            }
            set
            {
                base.SetValue(ExpanderProperty, value);
            }
        }

        public double FirstColumnIndent
        {
            get
            {
                return (double)base.GetValue(FirstColumnIndentProperty);
            }
            set
            {
                base.SetValue(FirstColumnIndentProperty, value);
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                if (this.Expander != null)
                {
                    return (base.VisualChildrenCount + 1);
                }
                return base.VisualChildrenCount;
            }
        }
    }
}
