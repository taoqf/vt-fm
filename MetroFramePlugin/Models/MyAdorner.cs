﻿using System;
using System.Collections.Generic;
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
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MetroFramePlugin.Models
{
    /// <summary>
    /// 移动控件
    /// </summary>
    public class MyCanvasAdorner : Adorner
    {
        const double THUMB_SIZE = 6;
        const double MINIMAL_SIZE = 10;
        const double MOVE_OFFSET = 10;
        Thumb tl, tr, bl, br, ct, cl, cr, cb;
        VisualCollection visCollec;
        FrameworkElement mDraggedElement = null;

        #region 改变事件
        public delegate void UIElementSizeChangedDelegate(object sender);

        public event UIElementSizeChangedDelegate CurrentUElementSizeChanged;

        internal void UIElementSizeChanged(object sender)
        {
            if (this.CurrentUElementSizeChanged != null)
            {
                this.CurrentUElementSizeChanged(sender);
            }
        }
        #endregion

        public MyCanvasAdorner(UIElement adorned, bool flag) : base(adorned)
        {           
            if (flag)
            {
                visCollec = new VisualCollection(this);
                visCollec.Add(tl = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Left, VerticalAlignment.Top));
                visCollec.Add(tr = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Right, VerticalAlignment.Top));
                visCollec.Add(bl = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Left, VerticalAlignment.Bottom));
                visCollec.Add(br = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Right, VerticalAlignment.Bottom));
                visCollec.Add(ct = GetResizeThumb(Cursors.SizeNS, HorizontalAlignment.Center, VerticalAlignment.Top));
                visCollec.Add(cl = GetResizeThumb(Cursors.SizeWE, HorizontalAlignment.Left, VerticalAlignment.Center));
                visCollec.Add(cr = GetResizeThumb(Cursors.SizeWE, HorizontalAlignment.Right, VerticalAlignment.Center));
                visCollec.Add(cb = GetResizeThumb(Cursors.SizeNS, HorizontalAlignment.Center, VerticalAlignment.Bottom));
            }  
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (mDraggedElement != null)
            {
                Win32.POINT screenPos = new Win32.POINT();
                if (Win32.GetCursorPos(ref screenPos))
                {
                    Point pos = PointFromScreen(new Point(screenPos.X, screenPos.Y));
                    Rect rect = new Rect(pos.X, pos.Y, mDraggedElement.ActualWidth, mDraggedElement.ActualHeight);
                    drawingContext.PushOpacity(1.0);
                    Brush highlight = mDraggedElement.TryFindResource(SystemColors.HighlightBrushKey) as Brush;
                    if (highlight != null)
                        drawingContext.DrawRectangle(highlight, new Pen(Brushes.Transparent, 0), rect);
                    drawingContext.DrawRectangle(new VisualBrush(mDraggedElement),
                        new Pen(Brushes.Transparent, 0), rect);
                    drawingContext.Pop();
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offset = THUMB_SIZE / 2;
            Size sz = new Size(THUMB_SIZE, THUMB_SIZE);
            if (tl != null)
            {
                tl.Arrange(new Rect(new Point(-offset, -offset), sz));
                tr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                bl.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                br.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
                ct.Arrange(new Rect(new Point((AdornedElement.RenderSize.Width - 2 * offset) / 2, -offset), sz));
                cl.Arrange(new Rect(new Point(-offset, (AdornedElement.RenderSize.Height - 2 * offset) / 2), sz));
                cb.Arrange(new Rect(new Point((AdornedElement.RenderSize.Width - 2 * offset) / 2, AdornedElement.RenderSize.Height - offset), sz));
                cr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, (AdornedElement.RenderSize.Height - 2 * offset) / 2), sz));
            }
           
            return finalSize;
        }

        private void Resize(FrameworkElement ff)
        {
        
            if (Double.IsNaN(ff.Width))
            {
                if (ff.RenderSize.Width > (ff.Parent as Canvas).ActualWidth)
                {
                    ff.Width = (ff.Parent as Canvas).ActualWidth - 500;
                }
                else
                {
                    if (Canvas.GetLeft(ff) + ff.Width > (ff.Parent as Canvas).ActualWidth)
                    {
                        ff.Width = (ff.Parent as Canvas).ActualWidth - Canvas.GetLeft(ff) - 5;
                    }
                    else
                        ff.Width = ff.RenderSize.Width;
                }                   
            }

            if (Double.IsNaN(ff.Height))
            {
                if (ff.Height > (ff.Parent as Canvas).ActualHeight)
                {
                    ff.Height = (ff.Parent as Canvas).ActualHeight - 500;
                }
                else
                {
                    if (Canvas.GetTop(ff) + ff.Height > (ff.Parent as Canvas).ActualHeight)
                    {
                        ff.Height = (ff.Parent as Canvas).ActualHeight - Canvas.GetTop(ff) - 5;
                    }
                    else ff.Height = ff.RenderSize.Height;
                }
            }
            ff.MouseLeave += ff_MouseMove;
        }

        private void ff_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                this.UIElementSizeChanged(sender);
            }
        }

     

        private Thumb GetResizeThumb(Cursor cur, HorizontalAlignment hor, VerticalAlignment ver)
        {
            var thumb = new Thumb()
            {
                Background = Brushes.Red,
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                HorizontalAlignment = hor,
                VerticalAlignment = ver,
                Cursor = cur,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.Green))
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null)
                    return;

                Resize(element);

                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        if (element.Height + e.VerticalChange > MINIMAL_SIZE)
                        {
                            element.Height += e.VerticalChange;
                        }
                        break;
                    case VerticalAlignment.Top:
                        if (element.Height - e.VerticalChange > MINIMAL_SIZE)
                        {
                            element.Height -= e.VerticalChange;
                            Canvas.SetTop(element, Canvas.GetTop(element) + e.VerticalChange);
                        }
                        break;
                }
                switch (thumb.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        if (element.Width - e.HorizontalChange > MINIMAL_SIZE)
                        {
                            element.Width -= e.HorizontalChange;
                            Canvas.SetLeft(element, Canvas.GetLeft(element) + e.HorizontalChange);
                        }
                        break;
                    case HorizontalAlignment.Right:
                        if (element.Width + e.HorizontalChange > MINIMAL_SIZE)
                        {
                            element.Width += e.HorizontalChange;
                        }
                        break;
                }

                e.Handled = true;
            };
            return thumb;
        }

        private Brush GetMoveEllipseBack()
        {
            string lan = "M 0,5 h 10 M 5,0 v 10";
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            var geometry = (Geometry)converter.ConvertFrom(lan);
            TileBrush bsh = new DrawingBrush(new GeometryDrawing(Brushes.Transparent, new Pen(Brushes.Black, 2), geometry));
            bsh.Stretch = Stretch.Fill;
            return bsh;
        }

        private FrameworkElementFactory GetFactory(Brush back)
        {
            back.Opacity = 0.6;
            var fef = new FrameworkElementFactory(typeof(Ellipse));
            fef.SetValue(Ellipse.FillProperty, back);
            fef.SetValue(Ellipse.StrokeProperty, Brushes.White);
            fef.SetValue(Ellipse.StrokeThicknessProperty, (double)1);
            return fef;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (visCollec != null)
            {
                return visCollec[index];
            }
            else return null;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                if (visCollec != null)
                    return visCollec.Count;
                else
                    return 0;
            }
        }

    }

    public static class Win32
    {
        public struct POINT { public Int32 X; public Int32 Y; }

        // During drag-and-drop operations, the position of the mouse cannot be 
        // reliably determined through GetPosition. This is because control of 
        // the mouse (possibly including capture) is held by the originating 
        // element of the drag until the drop is completed, with much of the 
        // behavior controlled by underlying Win32 calls. As a workaround, you 
        // might need to use Win32 externals such as GetCursorPos.
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT point);
    }
}
