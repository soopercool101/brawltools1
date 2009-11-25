﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using System.ComponentModel;
using System.Drawing;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public unsafe class ModelPanel : GLPanel
    {
        private bool _grabbing = false;
        private int _lastX, _lastY;

        //private int _rotX, _rotY;
        private float _rotFactor = 0.1f;
        public float RotationScale { get { return _rotFactor; } set { _rotFactor = value; } }

        //private int _transX, _transY;
        private float _transFactor = 0.05f;
        public float TranslationScale { get { return _transFactor; } set { _transFactor = value; } }

        //private int _zoom;
        private float _zoomFactor = 2.5f;
        public float ZoomScale { get { return _zoomFactor; } set { _zoomFactor = value; } }

        private int _zoomInit = -5;
        public int InitialZoomFactor { get { return _zoomInit; } set { _zoomInit = value; } }

        private int _yInit = -100;
        public int InitialYFactor { get { return _yInit; } set { _yInit = value; } }

        private Vector3 _eyePoint;
        private Vector3 _viewPoint, _viewRot;
        private float _viewDistance = 5.0f;
        private Matrix43 _viewMatrix = Matrix43.Identity;
        //private Matrix _vMatrix = Matrix.Identity;

        private List<ResourceNode> _resourceList = new List<ResourceNode>();

        private Matrix43 _transform = Matrix43.Identity;

        private MDL0Node _model;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;

                if (_model != null)
                    _model.Unbind(_context);

                if (_context != null)
                    _context.Unbind();

                _resourceList.Clear();
                if ((_model = value) != null)
                {
                    _resourceList.Add(_model);
                    if (_context != null)
                        _context._states["_Node_Refs"] = _resourceList;
                }

                ResetCamera();
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

        public ModelPanel()
        {
            ColorChanged += OnBackColorChanged;
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
            _viewPoint = new Vector3();
            _viewRot = new Vector3();

            CalcMatrices();

            Invalidate();
        }

        public void AddReference(ResourceNode node)
        {
            if (!_resourceList.Contains(node))
                _resourceList.Add(node);
            if (_model != null)
                _model.ResetTextures();
            Invalidate();
        }
        public void RemoveReference(ResourceNode node)
        {
            if (_resourceList.Contains(node))
                _resourceList.Remove(node);
            if (_model != null)
                _model.ResetTextures();
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int z = e.Delta / 120;

            Translate(0.0f, 0.0f, -z * _zoomFactor);

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _grabbing = true;

            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _grabbing = false;

            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int xDiff = e.X - _lastX;
            int yDiff = _lastY - e.Y;
            _lastX = e.X;
            _lastY = e.Y;

            lock(_context)
            {

                if (_grabbing)
                {
                    if (ModifierKeys == Keys.Control)
                        Rotate(yDiff * _rotFactor, -xDiff * _rotFactor);
                    else
                        Translate(-xDiff * _transFactor, -yDiff * _transFactor, 0.0f);
                }
            }

            base.OnMouseMove(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            keyData &= (Keys)0xFFFF;
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;

                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad8:
                case Keys.Up:
                    {
                        if (e.Control)
                            Rotate(-_rotFactor * 4, 0.0f);
                        else
                            Translate(0.0f, _transFactor * 8, 0.0f);
                        break;
                    }
                case Keys.NumPad2:
                case Keys.Down:
                    {
                        if (e.Control)
                            Rotate(_rotFactor * 4, 0.0f);
                        else
                            Translate(0.0f, -_transFactor * 8, 0.0f);
                        break;
                    }
                case Keys.NumPad6:
                case Keys.Right:
                    {
                        if (e.Control)
                            Rotate(0.0f, _rotFactor * 4);
                        else
                            Translate(_transFactor * 8, 0.0f, 0.0f);
                        break;
                    }
                case Keys.NumPad4:
                case Keys.Left:
                    {
                        if (e.Control)
                            Rotate(0.0f, -_rotFactor * 4);
                        else
                            Translate(-_transFactor * 8, 0.0f, 0.0f);
                        break;
                    }
                case Keys.Add:
                case Keys.Oemplus:
                    {
                        Translate(0.0f, 0.0f, -_zoomFactor);
                        break;
                    }
                case Keys.Subtract:
                case Keys.OemMinus:
                    {
                        Translate(0.0f, 0.0f, _zoomFactor);
                        break;
                    }
            }
            base.OnKeyDown(e);
        }

        private void Translate(float x, float y, float z)
        {
            _viewPoint = _transform.Multiply(new Vector3(x, y, z));

            CalcMatrices();
            this.Invalidate();
        }
        private void Rotate(float x, float y)
        {
            _viewRot._x = Math.Max(Math.Min(89.0f, _viewRot._x + x), -89.0f);
            _viewRot._y += y;

            CalcMatrices();
            this.Invalidate();
        }

        private void CalcMatrices()
        {
            _transform = Matrix43.TransformationMatrix(new Vector3(1.0f), _viewRot, _viewPoint);

            _eyePoint = _transform.Multiply(new Vector3(0.0f, 0.0f, _viewDistance));
        }

        private int _updateCounter = 0;
        public void BeginUpdate()
        {
            _updateCounter++;
        }
        public void EndUpdate()
        {
            if ((_updateCounter = Math.Max(_updateCounter - 1, 0)) == 0)
                Invalidate();
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
            _context.CheckErrors();

            //Set client states
            _context._states["_Node_Refs"] = _resourceList;

            OnResized();
        }

        protected internal override void OnRender()
        {
            if (_updateCounter > 0)
                return;

            _context.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);


            if (_model != null)
            {
                _context.glMatrixMode(GLMatrixMode.ModelView);
                _context.glLoadIdentity();

                _context.gluLookAt(_eyePoint._x, _eyePoint._y, _eyePoint._z, _viewPoint._x, _viewPoint._y, _viewPoint._z, 0.0, 1.0, 0.0);

                _model.Render(_context);
            }
        }
    }
}
