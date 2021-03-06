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
    public delegate void GLRenderEventHandler(object sender, GLContext context);

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

        private int _zoomInit = 5;
        public int InitialZoomFactor { get { return _zoomInit; } set { _zoomInit = value; } }

        private int _yInit = 100;
        public int InitialYFactor { get { return _yInit; } set { _yInit = value; } }

        private Vector3 _eyePoint;
        private Vector3 _viewPoint, _viewRot;
        private float _viewDistance = 5.0f;
        private Matrix43 _viewMatrix = Matrix43.Identity;
        //private Matrix _vMatrix = Matrix.Identity;

        internal Vector3 _defaultTranslate;

        public event GLRenderEventHandler PreRender, PostRender;

        private List<ResourceNode> _resourceList = new List<ResourceNode>();

        private Matrix43 _transform = Matrix43.Identity;

        //private MDL0Node _model;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public MDL0Node TargetModel
        //{
        //    get { return _model; }
        //    set
        //    {
        //        if (_model == value)
        //            return;

        //        if (_model != null)
        //            _model.Unbind(_context);

        //        if (_context != null)
        //            _context.Unbind();

        //        _resourceList.Clear();
        //        if ((_model = value) != null)
        //        {
        //            _resourceList.Add(_model);
        //            if (_context != null)
        //                _context._states["_Node_Refs"] = _resourceList;
        //        }

        //        ResetCamera();
        //    }
        //}

        private List<IRenderedObject> _renderList = new List<IRenderedObject>();

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
            _camera = new GLCamera();
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
            _camera.Reset();
            _camera.Translate(_defaultTranslate._x, _defaultTranslate._y, _defaultTranslate._z);

            Invalidate();
        }

        public void ClearAll()
        {
            ClearTargets();
            ClearReferences();

            _context.Unbind();
            _context._states["_Node_Refs"] = _resourceList;
        }

        public void AddTarget(IRenderedObject target)
        {
            if (_renderList.Contains(target))
                return;

            _renderList.Add(target);

            if (target is ResourceNode)
                _resourceList.Add(target as ResourceNode);

            _context.Capture();
            target.Attach(_context);

            Invalidate();
        }
        public void RemoveTarget(IRenderedObject target)
        {
            if (!_renderList.Contains(target))
                return;

            _context.Capture();
            target.Detach(_context);

            if (target is ResourceNode)
                RemoveReference(target as ResourceNode);

            _renderList.Remove(target);
        }
        public void ClearTargets()
        {
            foreach (IRenderedObject o in _renderList)
                o.Detach(_context);
            _renderList.Clear();
        }

        public void AddReference(ResourceNode node)
        {
            if (_resourceList.Contains(node))
                return;

            _resourceList.Add(node);
            RefreshReferences();
        }
        public void RemoveReference(ResourceNode node)
        {
            if (!_resourceList.Contains(node))
                return;

            _resourceList.Remove(node);
            RefreshReferences();
        }
        public void ClearReferences()
        {
            _resourceList.Clear();
            RefreshReferences();
        }
        private void RefreshReferences()
        {
            _context.Capture();
            foreach (IRenderedObject o in _renderList)
                o.Refesh(_context);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float z = (float)e.Delta / 120;
            if (Control.ModifierKeys == Keys.Shift)
                z *= 32;

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

            if (Control.ModifierKeys == Keys.Shift)
            {
                xDiff *= 16;
                yDiff *= 16;
            }

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

        protected override bool ProcessKeyMessage(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                Keys mod = Control.ModifierKeys;
                bool ctrl = (mod & Keys.Control) != 0;
                bool shift = (mod & Keys.Shift) != 0;
                bool alt = (mod & Keys.Alt) != 0;
                switch ((Keys)m.WParam)
                {
                    case Keys.NumPad8:
                    case Keys.Up:
                        {
                            if (alt)
                                break;
                            if (ctrl)
                                Rotate(-_rotFactor * (shift ? 32 : 4), 0.0f);
                            else
                                Translate(0.0f, _transFactor * (shift ? 128 : 8), 0.0f);
                            return true;
                        }
                    case Keys.NumPad2:
                    case Keys.Down:
                        {
                            if (alt)
                                break;
                            if (ctrl)
                                Rotate(_rotFactor * (shift ? 32 : 4), 0.0f);
                            else
                                Translate(0.0f, -_transFactor * (shift ? 128 : 8), 0.0f);
                            return true;
                        }
                    case Keys.NumPad6:
                    case Keys.Right:
                        {
                            if (alt)
                                break;
                            if (ctrl)
                                Rotate(0.0f, _rotFactor * (shift ? 32 : 4));
                            else
                                Translate(_transFactor * (shift ? 128 : 8), 0.0f, 0.0f);
                            return true;
                        }
                    case Keys.NumPad4:
                    case Keys.Left:
                        {
                            if (alt)
                                break;
                            if (ctrl)
                                Rotate(0.0f, -_rotFactor * (shift ? 32 : 4));
                            else
                                Translate(-_transFactor * (shift ? 128 : 8), 0.0f, 0.0f);
                            return true;
                        }
                    case Keys.Add:
                    case Keys.Oemplus:
                        {
                            if (alt)
                                break;
                            Translate(0.0f, 0.0f, -_zoomFactor * (shift ? 32 : 2));
                            return true;
                        }
                    case Keys.Subtract:
                    case Keys.OemMinus:
                        {
                            if (alt)
                                break;
                            Translate(0.0f, 0.0f, _zoomFactor * (shift ? 32 : 2));
                            return true;
                        }
                }
            }
            return base.ProcessKeyMessage(ref m);
        }

        private void Translate(float x, float y, float z)
        {
            _camera.Translate(x, y, z);
            this.Invalidate();
        }
        private void Rotate(float x, float y)
        {
            _camera.Pivot(_viewDistance, x, y);
            this.Invalidate();
        }

        protected internal unsafe override void OnInit()
        {
            Vector3 v = (Vector3)BackColor;
            _context.glClearColor(v._x, v._y, v._z, 0.0f);
            _context.glClearDepth(1.0f);
            //_context.glFrontFace(GLFrontFaceDirection.CW);
            //_context.glCullFace(GLFace.Back);
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

            //OnResized();
        }

        protected internal override void OnRender()
        {
            _context.glClear(GLClearMask.ColorBuffer | GLClearMask.DepthBuffer);

            //if (_renderList.Count > 0)
            //{
                if (PreRender != null)
                    PreRender(this, _context);

                foreach (IRenderedObject o in _renderList)
                    o.Render(_context);

#if DEBUG
                _context.CheckErrors();
#endif

                if (PostRender != null)
                    PostRender(this, _context);
            //}
        }
    }
}
