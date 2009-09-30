using System;

namespace System.Windows.Forms
{
    public abstract class GLControl : Control
    {
        public static GLControl Create()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT: { return new wGLControl(); }
            }
            return null;
        }
    }

    internal class wGLControl : GLControl
    {
        internal wGLControl() { }
    }
}
