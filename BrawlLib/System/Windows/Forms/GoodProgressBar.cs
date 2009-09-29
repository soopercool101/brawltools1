using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class GoodProgressBar : UserControl
    {
        private float _min = 0.0f, _max = 1.0f, _current = 0.0f;

        public float MinValue { get { return _min; } set { _min = value; } }
        public float MaxValue { get { return _max; } set { _max = value; } }
        public float CurrentValue { get { return _current; } set { _current = Math.Max(Math.Min(value, _max), _min); this.Invalidate(); } }
        public float Percent { get { return (_current - _min) / (_max - _min); } set { CurrentValue = (_max - _min) * value; } }

        public GoodProgressBar()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = this.ClientRectangle;

            float percent = this.Percent;

            bounds.Width = (int)(bounds.Width * percent);
            Point p1 = new Point(0, 0);
            Point p2 = new Point(bounds.Width, 0);
            Point p3 = new Point(bounds.Width, bounds.Height / 2);
            Point p4 = new Point(0, bounds.Height / 2);

            g.ResetClip();
            g.Clear(this.BackColor);

            if ((bounds.Width != 0) && (bounds.Height != 0))
                using (PathGradientBrush b = new PathGradientBrush(new Point[] { p1, p2, p3, p4 }, WrapMode.TileFlipY))
                {
                    b.CenterColor = Color.Gray;
                    b.SurroundColors = new Color[] { Color.Transparent, Color.Transparent, Color.Turquoise, Color.Turquoise };
                    g.FillRectangle(b, bounds);
                }

            g.Flush();
        }
    }
}
