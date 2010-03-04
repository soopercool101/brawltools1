﻿using System;
using System.IO;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public static class Wavefront
    {
        public static void Serialize(string outPath, params object[] assets)
        {
            using (FileStream stream = new FileStream(outPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("#Wavefront OBJ file, generated by BrawlBox");
                writer.WriteLine();

                //foreach (object o in assets)
                //{
                //    if (o is MDL0VertexNode)
                //    {
                //        WriteVertexGroup(writer, o as MDL0VertexNode);
                //    }
                //    else if (o is MDL0PolygonNode)
                //    {
                //        WritePolygon(writer, o as MDL0PolygonNode);
                //    }
                //}

                writer.Flush();
            }
        }

        //private static void WriteVertexGroup(StreamWriter writer, MDL0VertexNode vert)
        //{
        //    writer.WriteLine();
        //    writer.WriteLine("#Vertices");
        //    foreach (Vector3 v in vert.Vertices)
        //        writer.WriteLine("v {0} {1} {2}", v._x, v._y, v._z);
        //}
        //private static void WriteNormalGroup(StreamWriter writer, MDL0NormalNode norm)
        //{
        //    writer.WriteLine();
        //    writer.WriteLine("#Normals");
        //    foreach (Vector3 v in norm.Normals)
        //        writer.WriteLine("vn {0} {1} {2}", v._x, v._y, v._z);
        //}
        //private static void WriteUVGroup(StreamWriter writer, MDL0UVNode uv)
        //{
        //    writer.WriteLine();
        //    writer.WriteLine("#UVs");
        //    foreach (Vector2 v in uv.Points)
        //        writer.WriteLine("vt {0} {1}", v._x, v._y);
        //}
        private static void WriteMaterial(StreamWriter writer, MDL0MaterialNode mat)
        {
            writer.WriteLine(String.Format("usemtl {0}", mat.Name));
        }
        private static void WritePolygon(StreamWriter writer, MDL0PolygonNode poly)
        {
            if (poly._vertexNode != null)
                WriteVertexGroup(writer, poly._vertexNode);
            if (poly._normalNode != null)
                WriteNormalGroup(writer, poly._normalNode);
            if (poly._uvSet[0] != null)
                WriteUVGroup(writer, poly._uvSet[0]);

            writer.WriteLine();
            writer.WriteLine(String.Format("g {0}", poly.Name));
            if (poly._material != null)
                WriteMaterial(writer, poly._material);
            foreach (Primitive p in poly.Primitives)
            {
                switch (p._type)
                {
                    //case GLPrimitiveType.TriangleFan: WriteTriFan(writer, p); break;
                    //case GLPrimitiveType.TriangleStrip: WriteTriStrip(writer, p); break;
                    case GLPrimitiveType.Triangles: WriteTriList(writer, p); break;
                    //case GLPrimitiveType.Quads: WriteQuadList(writer, p); break;
                }
            }
        }

        //private static void WriteTriFan(StreamWriter writer, Primitive p)
        //{
        //    if ((p._vertexIndices == null) || (p._normalIndices == null))
        //        return;

        //    int count = p._elementCount - 2;
        //    if (p._uvIndices[0] != null)
        //        for (int i = 0; i < count; i++)
        //            writer.WriteLine(String.Format("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
        //                p._vertexIndices[0] + 1, p._uvIndices[0][0] + 1, p._normalIndices[0] + 1,
        //                p._vertexIndices[i + 1] + 1, p._uvIndices[0][i + 1] + 1, p._normalIndices[i + 1] + 1,
        //                p._vertexIndices[i + 2] + 1, p._uvIndices[0][i + 2] + 1, p._normalIndices[i + 2] + 1));
        //    else
        //        for (int i = 0; i < count; i++)
        //            writer.WriteLine(String.Format("f {0}//{1} {2}//{3} {4}//{5}",
        //                    p._vertexIndices[0] + 1, p._normalIndices[0] + 1,
        //                    p._vertexIndices[i + 1] + 1, p._normalIndices[i + 1] + 1,
        //                    p._vertexIndices[i + 2] + 1, p._normalIndices[i + 2] + 1));
        //}
        //private static void WriteTriStrip(StreamWriter writer, Primitive p)
        //{
        //    if ((p._vertexIndices == null) || (p._normalIndices == null))
        //        return;

        //    int count = p._elementCount - 2;
        //    int l1 = 0, l2 = 2;
        //    if (p._uvIndices[0] != null)
        //        for (int i = 0; i < count; i++)
        //        {
        //            if ((i & 1) == 0)
        //            {
        //                l1 = i;
        //                l2 = i + 1;
        //            }
        //            else
        //            {
        //                l1 = i + 1;
        //                l2 = i;
        //            }
        //            writer.WriteLine(String.Format("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
        //                p._vertexIndices[l1] + 1, p._uvIndices[0][l1] + 1, p._normalIndices[l1] + 1,
        //                p._vertexIndices[l2] + 1, p._uvIndices[0][l2] + 1, p._normalIndices[l2] + 1,
        //                p._vertexIndices[i + 2] + 1, p._uvIndices[0][i + 2] + 1, p._normalIndices[i + 2] + 1));
        //        }
        //    else
        //        for (int i = 0; i < count; i++)
        //        {
        //            if ((i & 1) == 0)
        //            {
        //                l1 = i;
        //                l2 = i + 1;
        //            }
        //            else
        //            {
        //                l1 = i + 1;
        //                l2 = i;
        //            }
        //            writer.WriteLine(String.Format("f {0}//{1} {2}//{3} {4}//{5}",
        //                    p._vertexIndices[l1] + 1, p._normalIndices[l1] + 1,
        //                    p._vertexIndices[l2] + 1, p._normalIndices[l2] + 1,
        //                    p._vertexIndices[i + 2] + 1, p._normalIndices[i + 2] + 1));
        //        }
        //}
        private static void WriteTriList(StreamWriter writer, Primitive p)
        {
            //if ((p._vertexIndices == null) || (p._normalIndices == null))
            //    return;

            //int count = p._elementCount / 3;
            //if (p._uvIndices[0] != null)
            //    for (int i = 0, x = 0; i < count; i++, x += 3)
            //        writer.WriteLine(String.Format("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
            //            p._vertexIndices[x] + 1, p._uvIndices[0][x] + 1, p._normalIndices[x] + 1,
            //            p._vertexIndices[x + 1] + 1, p._uvIndices[0][x + 1] + 1, p._normalIndices[x + 1] + 1,
            //            p._vertexIndices[x + 2] + 1, p._uvIndices[0][x + 2] + 1, p._normalIndices[x + 2] + 1));
            //else
            //    for (int i = 0, x = 0; i < count; i++, x += 3)
            //        writer.WriteLine(String.Format("f {0}//{1} {2}//{3} {4}//{5}",
            //            p._vertexIndices[x] + 1, p._normalIndices[x] + 1,
            //            p._vertexIndices[x + 1] + 1, p._normalIndices[x + 1] + 1,
            //            p._vertexIndices[x + 2] + 1, p._normalIndices[x + 2] + 1));
        }
        //private static void WriteQuadList(StreamWriter writer, Primitive p)
        //{
        //    if ((p._vertexIndices == null) || (p._normalIndices == null))
        //        return;

        //    int count = p._elementCount / 4;
        //    if (p._uvIndices[0] != null)
        //        for (int i = 0, x = 0; i < count; i++, x += 4)
        //            writer.WriteLine(String.Format("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8} {9}/{10}/{11}",
        //                p._vertexIndices[x] + 1, p._uvIndices[0][x] + 1, p._normalIndices[x] + 1,
        //                p._vertexIndices[x + 1] + 1, p._uvIndices[0][x + 1] + 1, p._normalIndices[x + 1] + 1,
        //                p._vertexIndices[x + 2] + 1, p._uvIndices[0][x + 2] + 1, p._normalIndices[x + 2] + 1,
        //                p._vertexIndices[x + 3] + 1, p._uvIndices[0][x + 3] + 1, p._normalIndices[x + 3] + 1));
        //    else
        //        for (int i = 0, x = 0; i < count; i++, x += 4)
        //            writer.WriteLine(String.Format("f {0}//{1} {2}//{3} {4}//{5} {6}//{7}",
        //                p._vertexIndices[x] + 1, p._normalIndices[x] + 1,
        //                p._vertexIndices[x + 1] + 1, p._normalIndices[x + 1] + 1,
        //                p._vertexIndices[x + 2] + 1, p._normalIndices[x + 2] + 1,
        //                p._vertexIndices[x + 3] + 1, p._normalIndices[x + 3] + 1));
        //}
    }
}
