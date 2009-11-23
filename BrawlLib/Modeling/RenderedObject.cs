using System;
using System.Collections.Generic;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public abstract class RenderedObject
    {
        internal string _name;
        public string Name { get { return _name; } set { _name = value; } }

        internal bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (_parent != null)
                        _parent._isDirty = true;
                }
            }
        }

        internal RenderedObject _parent;
        public RenderedObject Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                Remove();
                _parent = value;
                _isDirty = true;
                if (_parent != null)
                {
                    _parent._children.Add(this);
                    _parent._isDirty = true;
                }
            }
        }

        internal bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        internal uint _displayList = 0;

        internal List<RenderedObject> _children = new List<RenderedObject>();
        public List<RenderedObject> Children { get { return _children; } }

        internal FrameState _baseTransform = FrameState.Neutral;
        internal FrameState _frameTransform;

        internal Matrix _bindMatrix = Matrix.Identity, _iBindMatrix = Matrix.Identity;
        internal Matrix _frameMatrix, _iFrameMatrix;

        public FrameState BaseTransform
        {
            get { return _baseTransform; }
            set { _baseTransform = value; CalcBindMatrix(); }
        }
        public FrameState CurrentTransform
        {
            get { return _frameTransform; }
            set { _frameTransform = value; CalcFrameMatrix(); }
        }

        public void Remove()
        {
            if (_parent != null)
            {
                _isDirty = true;
                _parent._isDirty = true;
                _parent._children.Remove(this);
                _parent = null;
            }
        }

        private void CalcBindMatrix()
        {
            if (_parent != null)
            {
                _bindMatrix = _parent._bindMatrix * _baseTransform._transform;
                //_iBindMatrix = _parent._iBindMatrix * _baseTransform._iTransform;
            }
            else
            {
                _bindMatrix = _baseTransform._transform;
                //_iBindMatrix = _baseTransform._iTransform;
            }

            foreach (RenderedObject o in _children)
                o.CalcBindMatrix();
        }
        private void CalcFrameMatrix()
        {
            if (_parent != null)
            {
                _frameMatrix = _parent._frameMatrix * _frameTransform._transform;
                //_iFrameMatrix = _parent._iFrameMatrix * _frameTransform._iTransform;
            }
            else
            {
                _frameMatrix = _frameTransform._transform;
                //_iFrameMatrix = _frameTransform._iTransform;
            }

            foreach (RenderedObject o in _children)
                o.CalcFrameMatrix();
        }

        public virtual void Prepare(GLContext ctx)
        {
            //For objects that will not use a display list, do not set _isDirty.
            //This will cause the followng code to be ignored, as well as display lists.
            if (_isDirty)
            {
                if ((_displayList == 0) && (_displayList = ctx.glGenLists(1)) == 0)
                    return;

                OnPrepare(ctx);
                ctx.glNewList(_displayList, GLListMode.COMPILE);

                OnRender(ctx);

                ctx.glEndList();

                _isDirty = false;
            }
        }

        public abstract void OnRender(GLContext ctx);
        public virtual void OnPrepare(GLContext ctx) { }

        public virtual unsafe void Render(GLContext ctx)
        {
            if (!_enabled)
                return;

            //Prepare
            Prepare(ctx);

            //Transform
            fixed (Matrix* p = &_frameTransform._transform)
                ctx.glMultMatrix((float*)p);

            //Render
            if (_displayList != 0)
                ctx.glCallList(_displayList);
            else
                OnRender(ctx);

            //Children
            foreach (RenderedObject o in _children)
                o.Render(ctx);
        }

        public virtual void Unbind(GLContext ctx)
        {
            foreach (RenderedObject o in _children)
                o.Unbind(ctx);
            ClearList(ctx);
        }
        public void ClearList(GLContext ctx)
        {
            if (_displayList != 0)
            {
                ctx.glDeleteLists(_displayList, 1);
                _displayList = 0;
            }
        }

        public virtual void SetFrame(object state, int index)
        {
            if ((state == null) || (index == 0))
            {
                _frameTransform = _baseTransform;
                _frameMatrix = _bindMatrix;
                _iFrameMatrix = _iBindMatrix;
            }

            foreach (RenderedObject o in _children)
                o.SetFrame(state, index);
        }
    }
}
