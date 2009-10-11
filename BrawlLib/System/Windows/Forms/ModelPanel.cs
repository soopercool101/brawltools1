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
        public float RotationScale { get { return _rotFactor; } set { _rotFactor = value; } }

        private int _transX, _transY;
        private float _transFactor = 0.05f;
        public float TranslationScale { get { return _transFactor; } set { _transFactor = value; } }

        private int _zoom;
        private float _zoomFactor = 2.5f;
        public float ZoomScale { get { return _zoomFactor; } set { _zoomFactor = value; } }

        private int _zoomInit = -5;
        public int InitialZoomFactor { get { return _zoomInit; } set { _zoomInit = value; } }

        private int _yInit = -100;
        public int InitialYFactor { get { return _yInit; } set { _yInit = value; } }

        private GLModel _model;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GLModel TargetModel
        {
            get { return _model; }
            set 
            {
                if (_model == value)
                    return;

                if (_model != null)
                    _model.Unbind(_context);

                _model = value;
                InitModel();
                Invalidate();
            }
        }

        private void InitModel()
        {
            if (_model == null)
                return;

            _rotX = _rotY = _transX =  0;
            _transY = _yInit;
            _zoom = _zoomInit;
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
            _context.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            _context.glClearDepth(1.0f);
            _context.glPolygonMode(GLFace.Back, GLPolygonMode.Fill);

            _context.glEnable(GLEnableCap.DepthTest);
            _context.glDepthFunc(GLFunction.LEQUAL);
            _context.glShadeModel(GLShadingModel.SMOOTH);
            _context.glHint(GLHintTarget.PERSPECTIVE_CORRECTION_HINT, GLHintMode.NICEST);

            //_context.glBlendFunc(GLBlendFactor.SRC_ALPHA, GLBlendFactor.SRC_COLOR);
            //_context.glEnable(GLEnableCap.Blend);

            _context.glTexEnv(GLTexEnvTarget.TextureEnvironment, GLTexEnvParam.TEXTURE_ENV_MODE, (int)GLTexEnvMode.DECAL);
            //_context.glEnable(GLEnableCap.Lighting);
            _context.CheckErrors();

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

                _context.CheckErrors();


                _model.Render(_context);
            }

            _context.glFlush();
        }
    }
}
