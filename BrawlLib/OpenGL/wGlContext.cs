﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BrawlLib.OpenGL
{
    internal unsafe class wGlContext : GLContext
    {
        VoidPtr _hwnd, _hdc, _hglrc;

        public override void Dispose()
        {
            if (_hglrc)
            {
                wGL.wglMakeCurrent(null, null);
                wGL.wglDeleteContext(_hglrc);
                _hglrc = null;
            }
            if (_hdc)
            {
                Win32.ReleaseDC(_hwnd, _hdc);
                _hdc = null;
            }
            base.Dispose();
        }

        public wGlContext(Control target)
        {
            wGL.wglMakeCurrent(null, null);

            _hwnd = target.Handle;
            if (!(_hdc = Win32.GetDC(_hwnd)))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            PixelFormatDescriptor pfd = new PixelFormatDescriptor(32, 16);

            int format = wGL.ChoosePixelFormat(_hdc, &pfd);
            if (format == 0)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            wGL.SetPixelFormat(_hdc, format, &pfd);
            //if (wGL.SetPixelFormat(_hdc, format, &pfd) == 0)
            //    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            if (!(_hglrc = wGL.wglCreateContext(_hdc)))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public override void Capture()
        {
            if (_hglrc)
                wGL.wglMakeCurrent(_hdc, _hglrc);
        }
        public override void Release()
        {
            if (_hglrc)
            {
                wGL.wglSwapBuffers(_hdc);
                wGL.wglMakeCurrent(null, null);
            }
        }

        internal override void glAccum(GLAccumOp op, float value) { wGL.glAccum(op, value); }
        internal override void glAlphaFunc(GLAlphaFunc func, float refValue) { wGL.glAlphaFunc(func, refValue); }
        internal override bool glAreTexturesResident(int num, uint* textures, bool* residences) { return wGL.glAreTexturesResident(num, textures, residences); }
        internal override void glArrayElement(int index) { wGL.glArrayElement(index); }
        internal override void glBegin(GLBeginMode mode) { wGL.glBegin(mode); }
        internal override void glBindTexture(uint target, uint texture) { wGL.glBindTexture(target, texture); }
        internal override void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap) { wGL.glBitmap(width, height, xorig, yorig, xmove, ymove, bitmap); }
        internal override void glBlendFunc(uint sfactor, uint dfactor) { wGL.glBlendFunc(sfactor, dfactor); }

        internal override void glCallList(uint list) { wGL.glCallList(list); }
        internal override void glCallLists(int n, uint type, void* lists) { wGL.glCallLists(n, type, lists); }

        internal override void glClear(GLClearMask mask) { wGL.glClear(mask); }
        internal override void glClearAccum(float red, float green, float blue, float alpha) { wGL.glClearAccum(red, green, blue, alpha); }
        internal override void glClearColor(float red, float green, float blue, float alpha) { wGL.glClearColor(red, green, blue, alpha); }
        internal override void glClearDepth(double depth) { wGL.glClearDepth(depth); }
        internal override void glClearIndex(float c) { wGL.glClearIndex(c); }
        internal override void glClearStencil(int s) { wGL.glClearStencil(s); }
        internal override void glClipPlane(uint plane, double* equation) { wGL.glClipPlane(plane, equation); }

        internal override void glColor(sbyte red, sbyte green, sbyte blue) { wGL.glColor3b(red, green, blue); }
        internal override void glColor(double red, double green, double blue) { wGL.glColor3d(red, green, blue); }
        internal override void glColor(float red, float green, float blue) { wGL.glColor3f(red, green, blue); }
        internal override void glColor(int red, int green, int blue) { wGL.glColor3i(red, green, blue); }
        internal override void glColor(short red, short green, short blue) { wGL.glColor3s(red, green, blue); }
        internal override void glColor(byte red, byte green, byte blue) { wGL.glColor3ub(red, green, blue); }
        internal override void glColor(uint red, uint green, uint blue) { wGL.glColor3ui(red, green, blue); }
        internal override void glColor(ushort red, ushort green, ushort blue) { wGL.glColor3us(red, green, blue); }
        internal override void glColor(sbyte red, sbyte green, sbyte blue, sbyte alpha) { wGL.glColor4b(red, green, blue, alpha); }
        internal override void glColor(double red, double green, double blue, double alpha) { wGL.glColor4d(red, green, blue, alpha); }
        internal override void glColor(float red, float green, float blue, float alpha) { wGL.glColor4f(red, green, blue, alpha); }
        internal override void glColor(int red, int green, int blue, int alpha) { wGL.glColor4i(red, green, blue, alpha); }
        internal override void glColor(short red, short green, short blue, short alpha) { wGL.glColor4s(red, green, blue, alpha); }
        internal override void glColor(byte red, byte green, byte blue, byte alpha) { wGL.glColor4ub(red, green, blue, alpha); }
        internal override void glColor(uint red, uint green, uint blue, uint alpha) { wGL.glColor4ui(red, green, blue, alpha); }
        internal override void glColor(ushort red, ushort green, ushort blue, ushort alpha) { wGL.glColor4us(red, green, blue, alpha); }
        internal override void glColor3(sbyte* v) { wGL.glColor3bv(v); }
        internal override void glColor3(double* v) { wGL.glColor3dv(v); }
        internal override void glColor3(float* v) { wGL.glColor3fv(v); }
        internal override void glColor3(int* v) { wGL.glColor3iv(v); }
        internal override void glColor3(short* v) { wGL.glColor3sv(v); }
        internal override void glColor3(byte* v) { wGL.glColor3ubv(v); }
        internal override void glColor3(uint* v) { wGL.glColor3uiv(v); }
        internal override void glColor3(ushort* v) { wGL.glColor3usv(v); }
        internal override void glColor4(sbyte* v) { wGL.glColor4bv(v); }
        internal override void glColor4(double* v) { wGL.glColor4dv(v); }
        internal override void glColor4(float* v) { wGL.glColor4fv(v); }
        internal override void glColor4(int* v) { wGL.glColor4iv(v); }
        internal override void glColor4(short* v) { wGL.glColor4sv(v); }
        internal override void glColor4(byte* v) { wGL.glColor4ubv(v); }
        internal override void glColor4(uint* v) { wGL.glColor4uiv(v); }
        internal override void glColor4(ushort* v) { wGL.glColor4usv(v); }

        internal override void glColorMask(bool red, bool green, bool blue, bool alpha) { wGL.glColorMask(red, green, blue, alpha); }
        internal override void glColorMaterial(uint face, uint mode) { wGL.glColorMaterial(face, mode); }
        internal override void glColorPointer(int size, uint type, int stride, void* pointer) { wGL.glColorPointer(size, type, stride, pointer); }

        internal override void glCopyPixels(int x, int y, int width, int height, uint type) { wGL.glCopyPixels(x, y, width, height, type); }
        internal override void glCopyTexImage1D(uint target, int level, uint internalFormat, int x, int y, int width, int border) { wGL.glCopyTexImage1D(target, level, internalFormat, x, y, width, border); }
        internal override void glCopyTexImage2D(uint target, int level, uint internalFormat, int x, int y, int width, int height, int border) { wGL.glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border); }
        internal override void glCopyTexSubImage1D(uint target, int level, int xOffset, int x, int y, int width) { wGL.glCopyTexSubImage1D(target, level, xOffset, x, y, width); }
        internal override void glCopyTexSubImage2D(uint target, int level, int xOffset, int yOffset, int x, int y, int width, int height) { wGL.glCopyTexSubImage2D(target, level, xOffset, yOffset, x, y, width, height); }

        internal override void glCullFace(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glDeleteLists(uint list, int range)
        {
            throw new NotImplementedException();
        }

        internal override void glDeleteTextures(int num, uint* textures)
        {
            throw new NotImplementedException();
        }

        internal override void glDepthFunc(uint func)
        {
            throw new NotImplementedException();
        }

        internal override void glDepthMask(bool flag)
        {
            throw new NotImplementedException();
        }

        internal override void glDepthRange(double near, double far)
        {
            throw new NotImplementedException();
        }

        internal override void glDisable(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override void glDisableClientState(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override void glDrawArrays(uint mode, int first, int count)
        {
            throw new NotImplementedException();
        }

        internal override void glDrawBuffer(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glDrawElements(uint mode, int count, uint type, void* indices)
        {
            throw new NotImplementedException();
        }

        internal override void glDrawPixels(int width, int height, uint format, uint type, void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glEdgeFlag(bool flag)
        {
            throw new NotImplementedException();
        }

        internal override void glEdgeFlagPointer(int stride, bool* pointer)
        {
            throw new NotImplementedException();
        }

        internal override void glEdgeFlagv(bool* flag)
        {
            throw new NotImplementedException();
        }

        internal override void glEnable(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override void glEnableClientState(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override void glEnd() { wGL.glEnd(); }

        internal override void glEndList()
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(double u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(float u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(double u, double v)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord(float u, float v)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord1(double* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord1(float* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord2(double* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalCoord2(float* u)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalMesh(uint mode, int i1, int i2)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalMesh(uint mode, int i1, int i2, int j1, int j2)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalPoint(int i)
        {
            throw new NotImplementedException();
        }

        internal override void glEvalPoint(int i, int j)
        {
            throw new NotImplementedException();
        }

        internal override void glFeedbackBuffer(int size, uint type, out float* buffer)
        {
            throw new NotImplementedException();
        }

        internal override void glFinish()        {            wGL.glFinish();        }

        internal override void glFlush()        {            wGL.glFlush();        }

        internal override void glFog(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glFog(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glFog(uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glFog(uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glFrontFace(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glFrustum(double left, double right, double bottom, double top, double near, double far)
        {
            throw new NotImplementedException();
        }

        internal override uint glGenLists(int range)
        {
            throw new NotImplementedException();
        }

        internal override void glGenTextures(int num, uint* textures)
        {
            throw new NotImplementedException();
        }

        internal override void glGetBoolean(uint pname, bool* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetDouble(uint pname, double* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetFloat(uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetInteger(uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetClipPlane(uint plane, double* equation)
        {
            throw new NotImplementedException();
        }

        internal override uint glGetError()
        {
            throw new NotImplementedException();
        }

        internal override void glGetLight(uint light, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetLight(uint light, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMap(uint target, uint query, int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMaterial(uint face, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetMaterial(uint face, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, float* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, uint* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPixelMap(uint map, ushort* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPointer(uint name, void* values)
        {
            throw new NotImplementedException();
        }

        internal override void glGetPolygonStipple(out byte* mask)
        {
            throw new NotImplementedException();
        }

        internal override byte* glGetString(uint name)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexEnv(uint target, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexEnv(uint target, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out double* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexGen(uint coord, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexImage(uint target, uint format, uint type, out void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexLevelParameter(uint target, int level, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexLevelParameter(uint target, int level, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexParameter(uint target, uint pname, out float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glGetTexParameter(uint target, uint pname, out int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glHint(uint target, uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(double c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(float c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(int c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(short c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(double* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(float* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(int* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndex(short* c)
        {
            throw new NotImplementedException();
        }

        internal override void glIndexMask(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glIndexPointer(uint type, int stride, void* ptr)
        {
            throw new NotImplementedException();
        }

        internal override void glInitNames()
        {
            throw new NotImplementedException();
        }

        internal override void glInterleavedArrays(uint format, int stride, void* pointer)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsEnabled(uint cap)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsList(uint list)
        {
            throw new NotImplementedException();
        }

        internal override bool glIsTexture(uint texture)
        {
            throw new NotImplementedException();
        }

        internal override bool glLight(uint light, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override bool glLight(uint light, uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override bool glLight(uint light, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override bool glLight(uint light, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glLightModel(uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glLineStipple(int factor, ushort pattern)        {            wGL.glLineStipple(factor, pattern);        }

        internal override void glLineWidth(float width)        {            wGL.glLineWidth(width);        }

        internal override void glListBase(uint b)        {            wGL.glListBase(b);        }

        internal override void glLoadIdentity()        {            wGL.glLoadIdentity();        }

        internal override void glLoadMatrix(double* m)        {            wGL.glLoadMatrixd(m);        }

        internal override void glLoadMatrix(float* m)        {            wGL.glLoadMatrixf(m);        }

        internal override void glLoadName(uint name)        {            wGL.glLoadName(name);        }

        internal override void glLogicOp(uint opcode)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, double u1, double u2, int stride, int order, double* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, float u1, float u2, int stride, int order, float* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMap(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, double u1, double u2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, float u1, float u2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, double u1, double u2, int vn, double v1, double v2)
        {
            throw new NotImplementedException();
        }

        internal override void glMapGrid(int un, float u1, float u2, int vn, float v1, float v2)
        {
            throw new NotImplementedException();
        }

        internal override void glMaterial(uint face, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glMaterial(uint face, uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glMaterial(uint face, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glMaterial(uint face, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glMatrixMode(GLMatrixMode mode) { wGL.glMatrixMode(mode); }

        internal override void glMultMatrix(double* m) { wGL.glMultMatrixd(m); }

        internal override void glMultMatrix(float* m) { wGL.glMultMatrixf(m); }

        internal override void glNewList(uint list, uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(sbyte nx, sbyte ny, sbyte nz)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(double nx, double ny, double nz)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(float nx, float ny, float nz)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(int nx, int ny, int nz)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(short nx, short ny, short nz)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(sbyte* v)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glNormal(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glNormalPointer(uint type, int stride, void* pointer)
        {
            throw new NotImplementedException();
        }

        internal override void glOrtho(double left, double right, double bottom, double top, double near, double far) { wGL.glOrtho(left, right, bottom, top, near, far); }

        internal override void glPassThrough(float token)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, uint* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelMap(uint map, int mapsize, ushort* v)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelStore(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelStore(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelTransfer(uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelTransfer(uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glPixelZoom(float xfactor, float yfactor)
        {
            throw new NotImplementedException();
        }

        internal override void glPointSize(float size)
        {
            throw new NotImplementedException();
        }

        internal override void glPolygonMode(uint face, uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glPolygonOffset(float factor, float units)
        {
            throw new NotImplementedException();
        }

        internal override void glPolygonStipple(byte* mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopClientAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPopMatrix()
        {
            throw new NotImplementedException();
        }

        internal override void glPopName()
        {
            throw new NotImplementedException();
        }

        internal override void glPrioritizeTextures(int num, uint* textures, float* priorities)
        {
            throw new NotImplementedException();
        }

        internal override void glPushAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPushClientAttrib(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glPushMatrix()
        {
            throw new NotImplementedException();
        }

        internal override void glPushName(uint name)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y, short z)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(double x, double y, double z, double w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(float x, float y, float z, float w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(int x, int y, int z, int w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos(short x, short y, short z, short w)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos2(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos3(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glRasterPos4(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glReadBuffer(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glReadPixels(int x, int y, int width, int height, uint format, uint type, out void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(double x1, double y1, double x2, double y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(int x1, int y1, int x2, int y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(short x1, short y1, short x2, short y2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(double* v1, double* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(float* v1, float* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(int* v1, int* v2)
        {
            throw new NotImplementedException();
        }

        internal override void glRect(short* v1, short* v2)
        {
            throw new NotImplementedException();
        }

        internal override int glRenderMode(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glRotated(double angle, double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        internal override void glRotatef(float angle, float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        internal override void glScaled(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        internal override void glScalef(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        internal override void glScissor(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        internal override void glSelectBuffer(int size, out uint* buffer)
        {
            throw new NotImplementedException();
        }

        internal override void glShadeModel(uint mode)
        {
            throw new NotImplementedException();
        }

        internal override void glStencilFunc(uint func, int refval, uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glStencilMask(uint mask)
        {
            throw new NotImplementedException();
        }

        internal override void glStencilOp(uint fail, uint zfail, uint zpass)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(double s)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(float s)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(int s)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(short s)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(double s, double t)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(float s, float t)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(int s, int t)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(short s, short t)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(double s, double t, double r)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(float s, float t, float r)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(int s, int t, int r)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(short s, short t, short r)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(double s, double t, double r, double q)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(float s, float t, float r, float q)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(int s, int t, int r, int q)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord(short s, short t, short r, short q)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord1(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord1(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord1(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord1(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord2(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord2(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord2(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord2(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord3(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord3(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord3(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord3(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord4(double* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord4(float* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord4(int* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoord4(short* v)
        {
            throw new NotImplementedException();
        }

        internal override void glTexCoordPointer(int size, uint type, int stride, void* pointer)
        {
            throw new NotImplementedException();
        }

        internal override void glTexEnv(uint target, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexEnv(uint target, uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexEnv(uint target, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexEnv(uint target, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, double param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, double* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexGen(uint coord, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glTexImage2D(uint target, int level, int components, int width, int height, int border, uint format, uint type, void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glTexParameter(uint target, uint pname, float param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexParameter(uint target, uint pname, int param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexParameter(uint target, uint pname, float* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexParameter(uint target, uint pname, int* param)
        {
            throw new NotImplementedException();
        }

        internal override void glTexSubImage1D(uint target, int level, int xOffset, int width, uint format, uint type, void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glTexSubImage2D(uint target, int level, int xOffset, int yOffset, int width, int height, uint format, uint type, void* pixels)
        {
            throw new NotImplementedException();
        }

        internal override void glTranslate(double x, double y, double z) { wGL.glTranslated(x, y, z); }
        internal override void glTranslate(float x, float y, float z) { wGL.glTranslatef(x, y, z); }

        internal override void glVertex(double x, double y) { wGL.glVertex2d(x, y); }
        internal override void glVertex(float x, float y) { wGL.glVertex2f(x, y); }
        internal override void glVertex(int x, int y) { wGL.glVertex2i(x, y); }
        internal override void glVertex(short x, short y) { wGL.glVertex2s(x, y); }
        internal override void glVertex(double x, double y, double z) { wGL.glVertex3d(x, y, z); }
        internal override void glVertex(float x, float y, float z) { wGL.glVertex3f(x, y, z); }
        internal override void glVertex(int x, int y, int z) { wGL.glVertex3i(x, y, z); }
        internal override void glVertex(short x, short y, short z) { wGL.glVertex3s(x, y, z); }
        internal override void glVertex(double x, double y, double z, double w) { wGL.glVertex4d(x, y, z, w); }
        internal override void glVertex(float x, float y, float z, float w) { wGL.glVertex4f(x, y, z, w); }
        internal override void glVertex(int x, int y, int z, int w) { wGL.glVertex4i(x, y, z, w); }
        internal override void glVertex(short x, short y, short z, short w) { wGL.glVertex4s(x, y, z, w); }
        internal override void glVertex2v(double* v) { wGL.glVertex2dv(v); }
        internal override void glVertex2v(float* v) { wGL.glVertex2fv(v); }
        internal override void glVertex2v(int* v) { wGL.glVertex2iv(v); }
        internal override void glVertex2v(short* v) { wGL.glVertex2sv(v); }
        internal override void glVertex3v(double* v) { wGL.glVertex3dv(v); }
        internal override void glVertex3v(float* v) { wGL.glVertex3fv(v); }
        internal override void glVertex3v(int* v) { wGL.glVertex3iv(v); }
        internal override void glVertex3v(short* v) { wGL.glVertex3sv(v); }
        internal override void glVertex4v(double* v) { wGL.glVertex4dv(v); }
        internal override void glVertex4v(float* v) { wGL.glVertex4fv(v); }
        internal override void glVertex4v(int* v) { wGL.glVertex4iv(v); }
        internal override void glVertex4v(short* v) { wGL.glVertex4sv(v); }

        internal override void glVertexPointer(int size, uint type, int stride, void* pointer) { wGL.glVertexPointer(size, type, stride, pointer); }

        internal override void glViewport(int x, int y, int width, int height) { wGL.glViewport(x, y, width, height); }
    }
}
