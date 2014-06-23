using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Victop.Wpf.Controls
{
    public static class VicMessageBox
    {
        internal static Dictionary<VicMessageBoxButton, MessageBoxResult> defaultResult;
        internal const string LinkedResourcePath = "Resources/";

        static VicMessageBox()
        {
            Dictionary<VicMessageBoxButton, MessageBoxResult> dictionary = new Dictionary<VicMessageBoxButton, MessageBoxResult>();
            dictionary.Add(VicMessageBoxButton.OK, MessageBoxResult.OK);
            dictionary.Add(VicMessageBoxButton.OKCancel, MessageBoxResult.Cancel);
            dictionary.Add(VicMessageBoxButton.YesNo, MessageBoxResult.No);
            dictionary.Add(VicMessageBoxButton.YesNoCancel, MessageBoxResult.Cancel);
            defaultResult = dictionary;
        }

        public static void Show(string message)
        {
            Show(message, string.Empty, VicMessageBoxButton.OK, VicMessageBoxIcon.None, delegate(MessageBoxResult r)
            {
            });
        }

        public static void Show(string message, string caption)
        {
            Show(message, caption, VicMessageBoxButton.OK, VicMessageBoxIcon.None, delegate (MessageBoxResult r) {
            });
        }

        public static void Show(string message, Action<MessageBoxResult> callback)
        {
            Show(message, string.Empty, VicMessageBoxButton.OK, VicMessageBoxIcon.None, callback);
        }

        public static void Show(string message, string caption, VicMessageBoxButton button)
        {
            Show(message, caption, button, VicMessageBoxIcon.None, delegate(MessageBoxResult r)
            {
            });
        }

        public static void Show(string message, string caption, VicMessageBoxIcon icon)
        {
            Show(message, caption, VicMessageBoxButton.OK, icon, delegate(MessageBoxResult r)
            {
            });
        }

        public static void Show(string message, string caption, Action<MessageBoxResult> callback)
        {
            Show(message, caption, VicMessageBoxButton.OK, VicMessageBoxIcon.None, callback);
        }

        public static void Show(string message, string caption, VicMessageBoxButton button, VicMessageBoxIcon icon)
        {
            Show(message, caption, button, icon, delegate (MessageBoxResult r) {
            });
        }

        public static void Show(string message, string caption, VicMessageBoxButton button, Action<MessageBoxResult> callback)
        {
            Show(message, caption, button, VicMessageBoxIcon.None, callback);
        }

        public static void Show(string message, string caption, VicMessageBoxIcon icon, Action<MessageBoxResult> callback)
        {
            Show(message, caption, VicMessageBoxButton.OK, icon, callback);
        }

        public static void Show(string message, string caption, VicMessageBoxButton button, VicMessageBoxIcon icon, Action<MessageBoxResult> callback)
        {
            EventHandler handler2 = null;
            EventHandler handler = null;
            VicMessageBoxContent content = new VicMessageBoxContent
            {
                Message = message,
                Button = button,
                Icon = icon
            };
            if (ContentStyle != null)
            {
                content.Style = ContentStyle;
            }
            Window window = new Window {
                Title = caption,
                Content = content,
                ResizeMode=ResizeMode.NoResize
            };
            if (WindowStyle != null)
            {
                window.Style = WindowStyle;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            MessageBoxResult result = defaultResult[button];
            if (callback != null)
            {
                if (handler == null)
                {
                    if (handler2 == null)
                    {
                        handler2 = (s, e) => callback(result);
                    }
                    handler = handler2;
                }
                window.Closed += handler;
            }
            content.ButtonClick += delegate (MessageBoxResult r) {
                result = r;
                window.Close();
            };
            content.Loaded += (s, e) => content.Focus();
            content.KeyDown += delegate (object s, KeyEventArgs e) {
                if (e.Key == Key.Escape)
                {
                    window.Close();
                }
            };
        }

        public static Style ContentStyle
        {
            get;
            set;
        }

        public static Style WindowStyle
        {
            get;
            set;
        }
    }
}
