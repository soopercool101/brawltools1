using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BrawlLib.OpenGL
{
    public abstract unsafe class GLPanel : UserControl
    {
        internal protected GLContext _context;
        protected override void Dispose(bool disposing)
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            _context = GLContext.Attach(this);

            _context.Capture();
            OnInit();
            _context.Release();

            base.OnLoad(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_context != null)
            {
                if (Monitor.TryEnter(_context))
                {
                    try
                    {
                        _context.Capture();
                        OnRender();
                        _context.glFinish();
                        _context.Swap();
                        _context.Release();
                    }
                    finally { Monitor.Exit(_context); }
                }
            }
            else
                base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (_context != null)
            {
                _context.Capture();
                OnResized();
                _context.Release();
            }
            else
                base.OnResize(e);
        }

        internal protected virtual void OnInit()
        {
            _context.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            _context.glClearDepth(1.0f);
            OnResized();
        }
        internal protected virtual void OnResized()
        {
            _context.glViewport(0, 0, Width, Height);

            _context.glMatrixMode(GLMatrixMode.Projection);
            _context.glLoadIdentity();
            _context.gluPerspective(45.0f, (float)Width / (float)Height, 0.01f, 10000.0f);
        }
        internal protected virtual void OnRender()
        {
            _context.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);
        }
    }
}
