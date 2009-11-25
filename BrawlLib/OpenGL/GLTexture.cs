using System;

namespace BrawlLib.OpenGL
{
    public class GLTexture
    {
        internal GLContext _context;
        internal uint _id;
        internal int _width, _height;

        public uint Id { get { return _id; } }
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        //public GLTexture() { }
        public unsafe GLTexture(GLContext ctx, int width, int height)
        {
            uint id;
            ctx.glGenTextures(1, &id);
            _id = id;

            _context = ctx;
            _width = width;
            _height = height;
        }

        public void Bind()
        {
            if (_context != null)
                _context.glBindTexture(GLTextureTarget.Texture2D, _id);
        }

        public unsafe void Delete()
        {
            if ((_context != null) && (_id != 0))
            {
                uint id = _id;
                _context.glDeleteTextures(1, &id);
                _id = 0;
                _context = null;
            }
        }
    }
}
