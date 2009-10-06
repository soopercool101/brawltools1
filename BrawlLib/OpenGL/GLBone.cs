using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.OpenGL
{
    public class GLBone
    {
        public List<GLBone> _children = new List<GLBone>();
        public List<GLPolygon> _polygons = new List<GLPolygon>();

        public int _id;
        public float _rotAngle;
        public Vector3 _rotation, _translation, _scale;
        public bool _enabled = true;

        public virtual void Render(GLContext ctx)
        {
            if (!_enabled || ((_children.Count == 0) && (_polygons.Count == 0)))
                return;

            ctx.glPushMatrix();

            //ctx.glScale(_scale._x, _scale._y, _scale._z);
            //ctx.glTranslate(_translation._x, _translation._y, _translation._z);
            //ctx.glRotate(_rotAngle, _rotation._x, _rotation._y, _rotation._z);

            foreach (GLPolygon poly in _polygons)
                poly.Render(ctx);

            foreach (GLBone b in _children)
                b.Render(ctx);

            ctx.glPopMatrix();
        }

        public int _index;
    }
}
