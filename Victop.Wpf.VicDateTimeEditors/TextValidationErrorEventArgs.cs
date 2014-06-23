namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.Runtime.CompilerServices;

    public class TextValidationErrorEventArgs : EventArgs
    {
        private bool _throwException;

        public TextValidationErrorEventArgs(System.Exception exception, string text)
        {
            this.Text = text;
            this.Exception = exception;
        }

        public System.Exception Exception { get; private set; }

        public string Text { get; private set; }

        public bool ThrowException
        {
            get
            {
                return this._throwException;
            }
            set
            {
                if (value && (this.Exception == null))
                {
                    throw new ArgumentException("Cannot Throw Null Exception");
                }
                this._throwException = value;
            }
        }
    }
}

