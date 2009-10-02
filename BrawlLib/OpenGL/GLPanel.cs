using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.OpenGL
{
    public abstract unsafe class GLPanel : UserControl
    {
        internal protected GLContext _gl;
        protected override void Dispose(bool disposing)
        {
            if (_gl != null)
            {
                _gl.Dispose();
                _gl = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            _gl = GLContext.Attach(this);

            _gl.Capture();
            OnInit();
            _gl.Release();

            base.OnLoad(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_gl != null)
            {
                _gl.Capture();
                OnRender();
                _gl.Release();
            }
            else
                base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (_gl != null)
            {
                _gl.Capture();
                OnResized();
                _gl.Release();
            }
            else
                base.OnResize(e);
        }

        internal void OnInit()
        {
            _gl.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            _gl.glClearDepth(1.0f);
        }
        internal void OnResized()
        {
            _gl.glViewport(0, 0, Width, Height);
        }
        internal void OnRender()
        {
            _gl.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);
            _gl.glLoadIdentity();

            _gl.glBegin(GLBeginMode.Triangles);
            _gl.glVertex(0.0f, 1.0f, 0.0f);
            _gl.glVertex(0.0f, 0.0f, 1.0f);
            _gl.glVertex(1.0f, 0.0f, 0.0f);
            _gl.glEnd();

            _gl.glFlush();
        }
    }
}
