using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
namespace Victop.Wpf.Controls
{

    internal static class VisualStateHelper
    {
        public static void GoToState(Control control, string stateName, bool useTransitions)
        {
            try
            {
                VisualStateManager.GoToState(control, stateName, useTransitions);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Cannot go to state {0} in {1}.\nPlease verify you have this state in the Style and all the storyboards reference existing elements in the template.", new object[] { stateName, control.GetType().Name }), exception);
            }
        }
    }
}

