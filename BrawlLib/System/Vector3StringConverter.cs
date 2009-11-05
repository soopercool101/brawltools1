using System;
using System.ComponentModel;
using System.Globalization;

namespace System
{
    public class Vector3StringConverter : TypeConverter
    {
        private static char[] delims = new char[] { ',', '(', ')', ' ' };

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) { return destinationType == typeof(Vector3); }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)        {            return value.ToString();        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) { return sourceType == typeof(string); }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Vector3 v = new Vector3();

            string s = value.ToString();
            string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 3)
            {
                float.TryParse(arr[0], out v._x);
                float.TryParse(arr[1], out v._y);
                float.TryParse(arr[2], out v._z);
            }

            return v;
        }
    }
}
