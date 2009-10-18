using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ModelPanel : GLPanel
    {
        private IContainer components;
        private ModelContext _menu;

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
                if (_model != value) OnModelChanged(value);
               
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (base.BackColor != value)
                {
                    base.BackColor = value;
                    if (_context != null)
                    {
                        Vector3 v = (Vector3)value;
                        _context.Capture();
                        _context.glClearColor(v._x, v._y, v._z, 0.0f);
                        _context.Release();
                    }
                }
            }
        }

        private GLModel _currentModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GLModel CurrentModel
        {
            get { return _currentModel; }
            set { if (_currentModel != value) OnModelChanged(_currentModel = value); }
        }

        public ModelPanel()
        {
            components = new Container();
            ContextMenuStrip = _menu = new ModelContext(components);
            ColorChanged += OnBackColorChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnModelChanged(GLModel model)
        {
            if (_model != null)
                _model.Unbind(_context);

            _menu.Model = _model = model;

            ResetCamera();
        }
        private void OnBackColorChanged(Color c) { this.BackColor = c; }

        private delegate void ColorChangeEvent(Color c);
        private static event ColorChangeEvent ColorChanged;

        private static ColorDialog _colorDlg;
        public static void ChooseColor()
        {
            if (_colorDlg == null)
                _colorDlg = new ColorDialog();

            if (_colorDlg.ShowDialog() == DialogResult.OK)
            {
                if (ColorChanged != null)
                    ColorChanged(_colorDlg.Color);
            }
        }

        public void ResetCamera()
        {
            _rotX = _rotY = _transX =  0;
            _transY = _yInit;
            _zoom = _zoomInit;

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

        protected internal unsafe override void OnInit()
        {
            Vector3 v = (Vector3)BackColor;
            _context.glClearColor(v._x, v._y, v._z, 0.0f);
            _context.glClearDepth(1.0f);
            _context.glFrontFace(GLFrontFaceDirection.CW);
            _context.glCullFace(GLFace.Back);
            //_context.glEnable(GLEnableCap.CullFace);
            //_context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);

            _context.glEnable(GLEnableCap.DepthTest);
            _context.glDepthFunc(GLFunction.LEQUAL);
            _context.glShadeModel(GLShadingModel.SMOOTH);
            //_context.glEnable(GLEnableCap.POLYGON_SMOOTH);
            //_context.glEnable(GLEnableCap.LINE_SMOOTH);
            _context.glHint(GLHintTarget.PERSPECTIVE_CORRECTION_HINT, GLHintMode.NICEST);

            _context.glBlendFunc(GLBlendFactor.SRC_ALPHA, GLBlendFactor.ONE_MINUS_SRC_ALPHA);
            _context.glEnable(GLEnableCap.Blend);
            _context.glAlphaFunc(GLAlphaFunc.Greater, 0.1f);
            _context.glEnable(GLEnableCap.AlphaTest);

            float* pos = stackalloc float[4];
            pos[0] = pos[1] = pos[2] = 0.4f;
            pos[3] = 1.0f;
            _context.glLight(GLLightTarget.Light0, GLLightParameter.AMBIENT, pos);
            _context.glMaterial(GLFace.FrontAndBack, GLMaterialParameter.EMISSION, pos);
            pos[0] = 0.0f;
            pos[1] = 3.0f;
            pos[2] = 6.0f;
            pos[3] = 0.0f;
            _context.glLight(GLLightTarget.Light0, GLLightParameter.POSITION, pos);
            pos[0] = pos[1] = pos[2] = pos[3] = 1.0f;
            _context.glLight(GLLightTarget.Light0, GLLightParameter.DIFFUSE, pos);
            _context.glLight(GLLightTarget.Light0, GLLightParameter.SPECULAR, pos);
            _context.glEnable(GLEnableCap.Lighting);
            _context.glEnable(GLEnableCap.Light0);

            _context.glEnable(GLEnableCap.COLOR_MATERIAL);
            _context.glColorMaterial(GLFace.FrontAndBack, GLMaterialParameter.AMBIENT_AND_DIFFUSE);
            _context.glMaterial(GLFace.FrontAndBack, GLMaterialParameter.SPECULAR, pos);

            _context.glTexEnv(GLTexEnvTarget.TextureEnvironment, GLTexEnvParam.TEXTURE_ENV_MODE, (int)GLTexEnvMode.MODULATE);
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
        }
    }
}
