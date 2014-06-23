namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public sealed class NullableDateTimeConverter : TypeConverter
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
                return (DateTime?) value;
            }
            string str = value as string;
            if ((str != null) || (value == null))
            {
                return (!string.IsNullOrEmpty(str) ? new DateTime?(DateTime.Parse(str, CultureInfo.InvariantCulture)) : null);
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return base.ConvertFrom(value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            DateTime? nullable = (DateTime?) value;
            if (((destinationType == typeof(string)) && nullable.HasValue) && nullable.HasValue)
            {
                return nullable.Value.ToString("d", CultureInfo.InvariantCulture);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

