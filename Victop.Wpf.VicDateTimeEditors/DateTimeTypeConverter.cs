namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public sealed class DateTimeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }
            if (!(!(sourceType != typeof(string)) || typeof(DateTime).IsAssignableFrom(sourceType)))
            {
                return (sourceType == typeof(DateTime));
            }
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is DateTime)
            {
                return (DateTime) value;
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            string str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                return DateTime.Parse(str, CultureInfo.InvariantCulture);
            }
            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            DateTime? nullable = (DateTime?) value;
            if (!(destinationType == typeof(string)))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            if (nullable.HasValue && nullable.HasValue)
            {
                return nullable.Value.ToString("d", CultureInfo.InvariantCulture);
            }
            return "";
        }
    }
}

