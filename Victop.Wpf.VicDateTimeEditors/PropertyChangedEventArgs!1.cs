namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.Runtime.CompilerServices;

    public class PropertyChangedEventArgs<T> : EventArgs
    {
        public T NewValue { get; set; }

        public T OldValue { get; set; }
    }
}

