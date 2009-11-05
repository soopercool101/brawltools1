using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class NumericInputBox : TextBox
    {
        public float _value;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;
                UpdateText();
            }
        }

        public event EventHandler ValueChanged;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Left:
                case Keys.Right:
                case Keys.Back:
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    if ((this.SelectionStart != 0) || (Text.IndexOf('-') != -1))
                        e.SuppressKeyPress = true;
                    break;

                case Keys.Decimal:
                case Keys.OemPeriod:
                    if (Text.IndexOf('.') != -1)
                        e.SuppressKeyPress = true;
                    break;

                case Keys.Escape:
                    UpdateText();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Enter:
                    Apply();
                    e.SuppressKeyPress = true;
                    break;

                default:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        private void UpdateText()
        {
            if (_value == float.NaN)
                Text = "";
            else
                Text = _value.ToString();
        }

        private void Apply()
        {
            if (float.TryParse(Text, out _value))
            {
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            UpdateText();
        }
    }
}
