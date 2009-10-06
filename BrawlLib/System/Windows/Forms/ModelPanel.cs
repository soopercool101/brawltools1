using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class ModelPanel : GLPanel
    {
        private bool _grabbing = false;
        private int _lastX, _lastY;

        private int _rotX, _rotY;
        private float _rotFactor = 0.4f;

        private int _transX, _transY;
        private float _transFactor = 0.05f;

        private int _zoom;
        private float _zoomFactor = 2.5f;

        private GLModel _model;
        [Browsable(false)]
        public GLModel TargetModel
        {
            get { return _model; }
            set { _model = value; InitModel(); }
        }

        private void InitModel()
        {
            if (_model == null)
                return;

            _rotX = _rotY = _transX = _transY = 0;
            _zoom = -5;

            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int z = Math.Min(-1, _zoom + (e.Delta / 120));
            if (z != _zoom)
            {
                _zoom = z;
                this.Invalidate();
            }

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _grabbing = true;

            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _grabbing = false;

            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int xDiff = e.X - _lastX;
            int yDiff = _lastY - e.Y;
            _lastX = e.X;
            _lastY = e.Y;

            if (_grabbing)
            {
                if (ModifierKeys == Keys.Control)
                {
                    _transX += xDiff;
                    _transY += yDiff;
                }
                else
                {
                    _rotX -= yDiff;
                    _rotY += xDiff;
                }
                this.Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected internal override void OnInit()
        {
            //Vector3 v = (Vector3)BackColor;
            _context.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            _context.glClearDepth(1.0f);
            //_context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
            _context.glEnable(GLEnableCap.DepthTest);
            _context.glEnable(GLEnableCap.Texture2D);
            _context.glEnable(GLEnableCap.Blend);
            //_context.glEnable(GLEnableCap.Lighting);

            OnResized();
        }

        protected internal override void OnRender()
        {
            _context.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);


            if (_model != null)
            {
                _context.glMatrixMode(GLMatrixMode.ModelView);
                _context.glLoadIdentity();

                _context.glTranslate(_transX * _transFactor, _transY * _transFactor, _zoom * _zoomFactor);

                if (_rotX != 0)
                    _context.glRotate(_rotX * _rotFactor, 1.0f, 0.0f, 0.0f);
                if (_rotY != 0)
                    _context.glRotate(_rotY * _rotFactor, 0.0f, 1.0f, 0.0f);


                _model.Render(_context);
            }

            _context.glFlush();
        }
    }
}
