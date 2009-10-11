using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.OpenGL
{
    public unsafe abstract class GLContext : IDisposable
    {
        public virtual void Dispose() { }

        public static GLContext Attach(Control target)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT: return new wGlContext(target);
            }
            return null;
        }

        public void CheckErrors()
        {
            GLErrorCode code;
            if((code = glGetError()) == GLErrorCode.NO_ERROR)
                return;

            throw new Exception(code.ToString());
        }

        public virtual void Capture() { }
        public virtual void Release() { }

        internal abstract void glAccum(GLAccumOp op, float value);
        internal abstract void glAlphaFunc(GLAlphaFunc func, float refValue);
        internal abstract bool glAreTexturesResident(int num, uint* textures, bool* residences);
        internal abstract void glArrayElement(int index);
        internal abstract void glBegin(GLPrimitiveType mode);
        internal abstract void glBindTexture(GLTextureTarget target, uint texture);
        internal abstract void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap);
        internal abstract void glBlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor);
        internal abstract void glCallList(uint list);
        internal abstract void glCallLists(int n, uint type, void* lists);
        internal abstract void glClear(GLClearMask mask);
        internal abstract void glClearAccum(float red, float green, float blue, float alpha);
        internal abstract void glClearColor(float red, float green, float blue, float alpha);
        internal abstract void glClearDepth(double depth);
        internal abstract void glClearIndex(float c);
        internal abstract void glClearStencil(int s);
        internal abstract void glClipPlane(uint plane, double* equation);

        #region glColor

        internal abstract void glColor(sbyte red, sbyte green, sbyte blue);
        internal abstract void glColor(double red, double green, double blue);
        internal abstract void glColor(float red, float green, float blue);
        internal abstract void glColor(int red, int green, int blue);
        internal abstract void glColor(short red, short green, short blue);
        internal abstract void glColor(byte red, byte green, byte blue);
        internal abstract void glColor(uint red, uint green, uint blue);
        internal abstract void glColor(ushort red, ushort green, ushort blue);

        internal abstract void glColor(sbyte red, sbyte green, sbyte blue, sbyte alpha);
        internal abstract void glColor(double red, double green, double blue, double alpha);
        internal abstract void glColor(float red, float green, float blue, float alpha);
        internal abstract void glColor(int red, int green, int blue, int alpha);
        internal abstract void glColor(short red, short green, short blue, short alpha);
        internal abstract void glColor(byte red, byte green, byte blue, byte alpha);
        internal abstract void glColor(uint red, uint green, uint blue, uint alpha);
        internal abstract void glColor(ushort red, ushort green, ushort blue, ushort alpha);

        internal abstract void glColor3(sbyte* v);
        internal abstract void glColor3(double* v);
        internal abstract void glColor3(float* v);
        internal abstract void glColor3(int* v);
        internal abstract void glColor3(short* v);
        internal abstract void glColor3(byte* v);
        internal abstract void glColor3(uint* v);
        internal abstract void glColor3(ushort* v);

        internal abstract void glColor4(sbyte* v);
        internal abstract void glColor4(double* v);
        internal abstract void glColor4(float* v);
        internal abstract void glColor4(int* v);
        internal abstract void glColor4(short* v);
        internal abstract void glColor4(byte* v);
        internal abstract void glColor4(uint* v);
        internal abstract void glColor4(ushort* v);

        #endregion

        internal abstract void glColorMask(bool red, bool green, bool blue, bool alpha);
        internal abstract void glColorMaterial(uint face, uint mode);
        internal abstract void glColorPointer(int size, uint type, int stride, void* pointer);
        internal abstract void glCopyPixels(int x, int y, int width, int height, uint type);

        #region CopyTex

        internal abstract void glCopyTexImage1D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int border);
        internal abstract void glCopyTexImage2D(GLTextureTarget target, int level, GLInternalPixelFormat internalFormat, int x, int y, int width, int height, int border);
        internal abstract void glCopyTexSubImage1D(GLTextureTarget target, int level, int xOffset, int x, int y, int width);
        internal abstract void glCopyTexSubImage2D(GLTextureTarget target, int level, int xOffset, int yOffset, int x, int y, int width, int height);

        #endregion

        internal abstract void glCullFace(GLFace mode);
        /*
        [DllImport("opengl32.dll")]
        internal abstract ?? glDebugEntry(??);
         * */
        internal abstract void glDeleteLists(uint list, int range);
        internal abstract void glDeleteTextures(int num, uint* textures);
        internal abstract void glDepthFunc(GLFunction func);
        internal abstract void glDepthMask(bool flag);
        internal abstract void glDepthRange(double near, double far);
        internal abstract void glDisable(uint cap);
        internal abstract void glDisableClientState(uint cap);
        internal abstract void glDrawArrays(uint mode, int first, int count);
        internal abstract void glDrawBuffer(uint mode);
        internal abstract void glDrawElements(uint mode, int count, uint type, void* indices);
        internal abstract void glDrawPixels(int width, int height, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        internal abstract void glEdgeFlag(bool flag);
        internal abstract void glEdgeFlagPointer(int stride, bool* pointer);
        internal abstract void glEdgeFlagv(bool* flag);
        internal abstract void glEnable(GLEnableCap cap);
        internal abstract void glEnableClientState(uint cap);
        internal abstract void glEnd();
        internal abstract void glEndList();

        #region glEvalCoord

        internal abstract void glEvalCoord(double u);
        internal abstract void glEvalCoord(float u);
        internal abstract void glEvalCoord(double u, double v);
        internal abstract void glEvalCoord(float u, float v);
        internal abstract void glEvalCoord1(double* u);
        internal abstract void glEvalCoord1(float* u);
        internal abstract void glEvalCoord2(double* u);
        internal abstract void glEvalCoord2(float* u);

        #endregion

        internal abstract void glEvalMesh(uint mode, int i1, int i2);
        internal abstract void glEvalMesh(uint mode, int i1, int i2, int j1, int j2);

        internal abstract void glEvalPoint(int i);
        internal abstract void glEvalPoint(int i, int j);

        internal abstract void glFeedbackBuffer(int size, uint type, out float* buffer);
        internal abstract void glFinish();
        internal abstract void glFlush();

        #region glFog

        internal abstract void glFog(uint pname, float param);
        internal abstract void glFog(uint pname, int param);
        internal abstract void glFog(uint pname, float* param);
        internal abstract void glFog(uint pname, int* param);

        #endregion

        internal abstract void glFrontFace(uint mode);
        internal abstract void glFrustum(double left, double right, double bottom, double top, double near, double far);
        internal abstract uint glGenLists(int range);
        internal abstract void glGenTextures(int num, uint* textures);

        #region glGet

        internal abstract void glGet(GLGetMode pname, bool* param);
        internal abstract void glGet(GLGetMode pname, double* param);
        internal abstract void glGet(GLGetMode pname, float* param);
        internal abstract void glGet(GLGetMode pname, int* param);

        #endregion

        internal abstract void glGetClipPlane(uint plane, double* equation);
        internal abstract GLErrorCode glGetError();

        internal abstract void glGetLight(uint light, uint pname, float* param);
        internal abstract void glGetLight(uint light, uint pname, int* param);

        internal abstract void glGetMap(uint target, uint query, double* v);
        internal abstract void glGetMap(uint target, uint query, float* v);
        internal abstract void glGetMap(uint target, uint query, int* v);

        internal abstract void glGetMaterial(uint face, uint pname, float* param);
        internal abstract void glGetMaterial(uint face, uint pname, int* param);

        internal abstract void glGetPixelMap(uint map, float* values);
        internal abstract void glGetPixelMap(uint map, uint* values);
        internal abstract void glGetPixelMap(uint map, ushort* values);

        internal abstract void glGetPointer(uint name, void* values);
        internal abstract void glGetPolygonStipple(out byte* mask);
        internal abstract byte* glGetString(uint name);

        internal abstract void glGetTexEnv(uint target, uint pname, out float* param);
        internal abstract void glGetTexEnv(uint target, uint pname, out int* param);

        internal abstract void glGetTexGen(uint coord, uint pname, out double* param);
        internal abstract void glGetTexGen(uint coord, uint pname, out float* param);
        internal abstract void glGetTexGen(uint coord, uint pname, out int* param);

        internal abstract void glGetTexImage(uint target, uint format, uint type, out void* pixels);

        internal abstract void glGetTexLevelParameter(uint target, int level, uint pname, out float* param);
        internal abstract void glGetTexLevelParameter(uint target, int level, uint pname, out int* param);

        internal abstract void glGetTexParameter(uint target, uint pname, out float* param);
        internal abstract void glGetTexParameter(uint target, uint pname, out int* param);

        internal abstract void glHint(GLHintTarget target, GLHintMode mode);

        #region glIndex

        internal abstract void glIndex(double c);
        internal abstract void glIndex(float c);
        internal abstract void glIndex(int c);
        internal abstract void glIndex(short c);

        internal abstract void glIndex(double* c);
        internal abstract void glIndex(float* c);
        internal abstract void glIndex(int* c);
        internal abstract void glIndex(short* c);

        #endregion

        internal abstract void glIndexMask(uint mask);
        internal abstract void glIndexPointer(uint type, int stride, void* ptr);
        internal abstract void glInitNames();
        internal abstract void glInterleavedArrays(uint format, int stride, void* pointer);
        internal abstract bool glIsEnabled(uint cap);
        internal abstract bool glIsList(uint list);
        internal abstract bool glIsTexture(uint texture);

        #region glLight

        internal abstract bool glLight(uint light, uint pname, float param);
        internal abstract bool glLight(uint light, uint pname, int param);
        internal abstract bool glLight(uint light, uint pname, float* param);
        internal abstract bool glLight(uint light, uint pname, int* param);

        #endregion

        #region glLightModel

        internal abstract void glLightModel(uint pname, float param);
        internal abstract void glLightModel(uint pname, int param);
        internal abstract void glLightModel(uint pname, float* param);
        internal abstract void glLightModel(uint pname, int* param);

        #endregion

        internal abstract void glLineStipple(int factor, ushort pattern);
        internal abstract void glLineWidth(float width);
        internal abstract void glListBase(uint b);
        internal abstract void glLoadIdentity();
        internal abstract void glLoadMatrix(double* m);
        internal abstract void glLoadMatrix(float* m);
        internal abstract void glLoadName(uint name);
        internal abstract void glLogicOp(uint opcode);

        #region glMap

        internal abstract void glMap(uint target, double u1, double u2, int stride, int order, double* points);
        internal abstract void glMap(uint target, float u1, float u2, int stride, int order, float* points);
        internal abstract void glMap(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points);
        internal abstract void glMap(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points);

        #endregion

        #region glMapGrid

        internal abstract void glMapGrid(int un, double u1, double u2);
        internal abstract void glMapGrid(int un, float u1, float u2);
        internal abstract void glMapGrid(int un, double u1, double u2, int vn, double v1, double v2);
        internal abstract void glMapGrid(int un, float u1, float u2, int vn, float v1, float v2);

        #endregion

        #region glMaterial

        internal abstract void glMaterial(uint face, uint pname, float param);
        internal abstract void glMaterial(uint face, uint pname, int param);
        internal abstract void glMaterial(uint face, uint pname, float* param);
        internal abstract void glMaterial(uint face, uint pname, int* param);

        #endregion

        internal abstract void glMatrixMode(GLMatrixMode mode);

        internal abstract void glMultMatrix(double* m);
        internal abstract void glMultMatrix(float* m);

        internal abstract void glNewList(uint list, uint mode);

        #region glNormal

        internal abstract void glNormal(sbyte nx, sbyte ny, sbyte nz);
        internal abstract void glNormal(double nx, double ny, double nz);
        internal abstract void glNormal(float nx, float ny, float nz);
        internal abstract void glNormal(int nx, int ny, int nz);
        internal abstract void glNormal(short nx, short ny, short nz);

        internal abstract void glNormal(sbyte* v);
        internal abstract void glNormal(double* v);
        internal abstract void glNormal(float* v);
        internal abstract void glNormal(int* v);
        internal abstract void glNormal(short* v);

        #endregion

        internal abstract void glNormalPointer(uint type, int stride, void* pointer);

        internal abstract void glOrtho(double left, double right, double bottom, double top, double near, double far);
        internal abstract void glPassThrough(float token);

        internal abstract void glPixelMap(uint map, int mapsize, float* v);
        internal abstract void glPixelMap(uint map, int mapsize, uint* v);
        internal abstract void glPixelMap(uint map, int mapsize, ushort* v);

        internal abstract void glPixelStore(uint pname, float param);
        internal abstract void glPixelStore(uint pname, int param);

        internal abstract void glPixelTransfer(uint pname, float param);
        internal abstract void glPixelTransfer(uint pname, int param);

        internal abstract void glPixelZoom(float xfactor, float yfactor);
        internal abstract void glPointSize(float size);
        internal abstract void glPolygonMode(GLFace face, GLPolygonMode mode);
        internal abstract void glPolygonOffset(float factor, float units);
        internal abstract void glPolygonStipple(byte* mask);

        internal abstract void glPopAttrib(uint mask);
        internal abstract void glPopClientAttrib(uint mask);
        internal abstract void glPopMatrix();
        internal abstract void glPopName();

        internal abstract void glPrioritizeTextures(int num, uint* textures, float* priorities);

        internal abstract void glPushAttrib(uint mask);
        internal abstract void glPushClientAttrib(uint mask);
        internal abstract void glPushMatrix();
        internal abstract void glPushName(uint name);

        #region glRasterPos

        internal abstract void glRasterPos(double x, double y);
        internal abstract void glRasterPos(float x, float y);
        internal abstract void glRasterPos(int x, int y);
        internal abstract void glRasterPos(short x, short y);

        internal abstract void glRasterPos(double x, double y, double z);
        internal abstract void glRasterPos(float x, float y, float z);
        internal abstract void glRasterPos(int x, int y, int z);
        internal abstract void glRasterPos(short x, short y, short z);

        internal abstract void glRasterPos(double x, double y, double z, double w);
        internal abstract void glRasterPos(float x, float y, float z, float w);
        internal abstract void glRasterPos(int x, int y, int z, int w);
        internal abstract void glRasterPos(short x, short y, short z, short w);

        internal abstract void glRasterPos2(double* v);
        internal abstract void glRasterPos2(float* v);
        internal abstract void glRasterPos2(int* v);
        internal abstract void glRasterPos2(short* v);

        internal abstract void glRasterPos3(double* v);
        internal abstract void glRasterPos3(float* v);
        internal abstract void glRasterPos3(int* v);
        internal abstract void glRasterPos3(short* v);

        internal abstract void glRasterPos4(double* v);
        internal abstract void glRasterPos4(float* v);
        internal abstract void glRasterPos4(int* v);
        internal abstract void glRasterPos4(short* v);

        #endregion

        internal abstract void glReadBuffer(uint mode);
        internal abstract void glReadPixels(int x, int y, int width, int height, uint format, uint type, out void* pixels);

        #region glRect

        internal abstract void glRect(double x1, double y1, double x2, double y2);
        internal abstract void glRect(float x1, float y1, float x2, float y2);
        internal abstract void glRect(int x1, int y1, int x2, int y2);
        internal abstract void glRect(short x1, short y1, short x2, short y2);

        internal abstract void glRect(double* v1, double* v2);
        internal abstract void glRect(float* v1, float* v2);
        internal abstract void glRect(int* v1, int* v2);
        internal abstract void glRect(short* v1, short* v2);

        #endregion

        internal abstract int glRenderMode(uint mode);

        internal abstract void glRotate(double angle, double x, double y, double z);
        internal abstract void glRotate(float angle, float x, float y, float z);

        internal abstract void glScale(double x, double y, double z);
        internal abstract void glScale(float x, float y, float z);

        internal abstract void glScissor(int x, int y, int width, int height);
        internal abstract void glSelectBuffer(int size, out uint* buffer);
        internal abstract void glShadeModel(GLShadingModel mode);
        internal abstract void glStencilFunc(uint func, int refval, uint mask);
        internal abstract void glStencilMask(uint mask);
        internal abstract void glStencilOp(uint fail, uint zfail, uint zpass);

        #region glTexCoord

        internal abstract void glTexCoord(double s);
        internal abstract void glTexCoord(float s);
        internal abstract void glTexCoord(int s);
        internal abstract void glTexCoord(short s);

        internal abstract void glTexCoord(double s, double t);
        internal abstract void glTexCoord(float s, float t);
        internal abstract void glTexCoord(int s, int t);
        internal abstract void glTexCoord(short s, short t);

        internal abstract void glTexCoord(double s, double t, double r);
        internal abstract void glTexCoord(float s, float t, float r);
        internal abstract void glTexCoord(int s, int t, int r);
        internal abstract void glTexCoord(short s, short t, short r);

        internal abstract void glTexCoord(double s, double t, double r, double q);
        internal abstract void glTexCoord(float s, float t, float r, float q);
        internal abstract void glTexCoord(int s, int t, int r, int q);
        internal abstract void glTexCoord(short s, short t, short r, short q);

        internal abstract void glTexCoord1(double* v);
        internal abstract void glTexCoord1(float* v);
        internal abstract void glTexCoord1(int* v);
        internal abstract void glTexCoord1(short* v);

        internal abstract void glTexCoord2(double* v);
        internal abstract void glTexCoord2(float* v);
        internal abstract void glTexCoord2(int* v);
        internal abstract void glTexCoord2(short* v);

        internal abstract void glTexCoord3(double* v);
        internal abstract void glTexCoord3(float* v);
        internal abstract void glTexCoord3(int* v);
        internal abstract void glTexCoord3(short* v);

        internal abstract void glTexCoord4(double* v);
        internal abstract void glTexCoord4(float* v);
        internal abstract void glTexCoord4(int* v);
        internal abstract void glTexCoord4(short* v);

        #endregion

        internal abstract void glTexCoordPointer(int size, uint type, int stride, void* pointer);

        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, float* param);
        internal abstract void glTexEnv(GLTexEnvTarget target, GLTexEnvParam pname, int* param);

        #region glTexGen

        internal abstract void glTexGen(uint coord, uint pname, double param);
        internal abstract void glTexGen(uint coord, uint pname, float param);
        internal abstract void glTexGen(uint coord, uint pname, int param);
        internal abstract void glTexGen(uint coord, uint pname, double* param);
        internal abstract void glTexGen(uint coord, uint pname, float* param);
        internal abstract void glTexGen(uint coord, uint pname, int* param);

        #endregion

        internal abstract void glTexImage1D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);
        internal abstract void glTexImage2D(GLTexImageTarget target, int level, GLInternalPixelFormat internalFormat, int width, int height, int border, GLPixelDataFormat format, GLPixelDataType type, void* pixels);

        #region glTexParameter

        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, float* param);
        internal abstract void glTexParameter(GLTextureTarget target, GLTextureParameter pname, int* param);

        #endregion

        internal abstract void glTexSubImage1D(uint target, int level, int xOffset, int width, uint format, uint type, void* pixels);
        internal abstract void glTexSubImage2D(uint target, int level, int xOffset, int yOffset, int width, int height, uint format, uint type, void* pixels);

        internal abstract void glTranslate(double x, double y, double z);
        internal abstract void glTranslate(float x, float y, float z);

        #region glVertex

        internal abstract void glVertex(double x, double y);
        internal abstract void glVertex(float x, float y);
        internal abstract void glVertex(int x, int y);
        internal abstract void glVertex(short x, short y);

        internal abstract void glVertex(double x, double y, double z);
        internal abstract void glVertex(float x, float y, float z);
        internal abstract void glVertex(int x, int y, int z);
        internal abstract void glVertex(short x, short y, short z);

        internal abstract void glVertex(double x, double y, double z, double w);
        internal abstract void glVertex(float x, float y, float z, float w);
        internal abstract void glVertex(int x, int y, int z, int w);
        internal abstract void glVertex(short x, short y, short z, short w);

        internal abstract void glVertex2v(double* v);
        internal abstract void glVertex2v(float* v);
        internal abstract void glVertex2v(int* v);
        internal abstract void glVertex2v(short* v);

        internal abstract void glVertex3v(double* v);
        internal abstract void glVertex3v(float* v);
        internal abstract void glVertex3v(int* v);
        internal abstract void glVertex3v(short* v);

        internal abstract void glVertex4v(double* v);
        internal abstract void glVertex4v(float* v);
        internal abstract void glVertex4v(int* v);
        internal abstract void glVertex4v(short* v);

        #endregion

        internal abstract void glVertexPointer(int size, uint type, int stride, void* pointer);
        internal abstract void glViewport(int x, int y, int width, int height);

        internal abstract void gluPerspective(double fovy, double aspect, double zNear, double zFar);
    }
}
