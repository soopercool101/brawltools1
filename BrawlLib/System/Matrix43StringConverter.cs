using System;
using System.ComponentModel;
using System.Globalization;

namespace System
{
    public unsafe class Matrix43StringConverter : TypeConverter
    {
        private static char[] delims = new char[] { ',', '(', ')', ' ' };

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { return destinationType == typeof(Matrix43); }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return value.ToString(); }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { return sourceType == typeof(string); }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Matrix43 m = new Matrix43();

            string s = value.ToString();
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 12)
            {
                float.TryParse(arr[0], out m._data[0]);
                float.TryParse(arr[1], out m._data[1]);
                float.TryParse(arr[2], out m._data[2]);
                float.TryParse(arr[3], out m._data[3]);
                float.TryParse(arr[4], out m._data[4]);
                float.TryParse(arr[5], out m._data[5]);
                float.TryParse(arr[6], out m._data[6]);
                float.TryParse(arr[7], out m._data[7]);
                float.TryParse(arr[8], out m._data[8]);
                float.TryParse(arr[9], out m._data[9]);
                float.TryParse(arr[10], out m._data[10]);
                float.TryParse(arr[11], out m._data[11]);
            }
            return m;
        }
    }
}
