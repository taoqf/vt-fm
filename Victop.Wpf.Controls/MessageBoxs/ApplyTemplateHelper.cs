namespace Victop.Wpf.Controls
{
    using System;
    using System.Globalization;

    internal static class ApplyTemplateHelper
    {
        internal static void VerifyTemplateChild(Type type, string childType, string childName, object child, bool required, ref string errors)
        {
            if (child == null)
            {
                if (required)
                {
                    errors = errors + string.Format(CultureInfo.InvariantCulture, "\nThe {0} {1} is null!", new object[] { childType, childName });
                }
            }
            else if (!type.IsInstanceOfType(child))
            {
                errors = errors + string.Format(CultureInfo.InvariantCulture, "The {0} {1} isn't an instance of {2}!", new object[] { childType, childName, type.Name });
            }
        }
    }
}

