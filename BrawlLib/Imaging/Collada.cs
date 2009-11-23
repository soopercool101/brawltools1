using System;
using System.IO;
using System.Xml;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Imaging
{
    public static class Collada
    {
        static XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace };
        public static void Serialize(object[] assets, string outFile)
        {
            using (FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.SequentialScan))
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("COLLADA", "http://www.collada.org/2008/03/COLLADASchema");
                writer.WriteAttributeString("version", "1.4.1");

                writer.WriteStartElement("asset");
                writer.WriteElementString("up_axis", "Y_UP");
                writer.WriteEndElement();

                //Define materials
                writer.WriteStartElement("library_materials");
                writer.WriteStartElement("material");
                writer.WriteAttributeString("id", "lambert1");
                writer.WriteAttributeString("name", "lambert1");
                writer.WriteStartElement("instance_effect");
                writer.WriteAttributeString("url", "#lambert1_fx");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                //Define effects
                writer.WriteStartElement("library_effects");
                writer.WriteStartElement("effect");
                writer.WriteAttributeString("id", "lambert1_fx");
                writer.WriteStartElement("profile_COMMON");
                writer.WriteStartElement("technique");
                writer.WriteAttributeString("sid", "common");
                writer.WriteStartElement("lambert");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                //Define geometry
                writer.WriteStartElement("library_geometries");
                foreach (object o in assets)
                {
                    if (o is GLModel)
                    {
                        WriteModel((GLModel)o, writer);
                    }
                }
                writer.WriteEndElement();

                //Define scenes
                writer.WriteStartElement("library_visual_scenes");
                writer.WriteStartElement("visual_scene");
                writer.WriteAttributeString("id", "VisualSceneNode");
                writer.WriteAttributeString("name", "VisualSceneNode");

                //Define nodes
                foreach (object o in assets)
                {
                    if (o is GLModel)
                    {
                        WriteNodes((GLModel)o, writer);
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteStartElement("scene");
                writer.WriteStartElement("instance_visual_scene");
                writer.WriteAttributeString("url", "#VisualSceneNode");
                writer.WriteEndElement();


                writer.WriteEndElement();
                writer.Close();
            }
        }

        private static unsafe void WriteModel(GLModel model, XmlWriter writer)
        {
            foreach (GLPolygon poly in model._polygons)
            {
                //write header
                writer.WriteStartElement("geometry");
                writer.WriteAttributeString("id", poly._name);
                writer.WriteAttributeString("name", poly._name);

                //write mesh
                writer.WriteStartElement("mesh");
                //write source data

                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly._name + "_positions");
                writer.WriteAttributeString("name", poly._name + "positions");

                writer.WriteStartElement("float_array");
                writer.WriteAttributeString("id", poly._name + "_array");
                writer.WriteAttributeString("count", (poly._vertices.Vertices * 3).ToString());

                Vector3* vPtr = (Vector3*)poly._vertices.Address;

                writer.WriteString(String.Format("{0} {1} {2}", vPtr->_x, vPtr->_y, vPtr->_z));
                vPtr++;
                for (int i = 1; i < poly._vertices.Vertices; i++, vPtr++)
                    writer.WriteString(String.Format(" {0} {1} {2}", vPtr->_x, vPtr->_y, vPtr->_z));

                writer.WriteEndElement();

                //Write technique
                writer.WriteStartElement("technique_common");

                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", "#" + poly._name + "_array");
                writer.WriteAttributeString("count", poly._vertices.Vertices.ToString());
                writer.WriteAttributeString("stride", "3");

                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "X");
                writer.WriteAttributeString("type", "float");
                writer.WriteEndElement();
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Y");
                writer.WriteAttributeString("type", "float");
                writer.WriteEndElement();
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Z");
                writer.WriteAttributeString("type", "float");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteStartElement("vertices");
                writer.WriteAttributeString("id", poly._name + "_vertices");
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "POSITION");
                writer.WriteAttributeString("source", "#" + poly._name + "_positions");
                writer.WriteEndElement();
                writer.WriteEndElement();

                //Write primitives
                WritePrimitiveType(poly, GLPrimitiveType.Lines, writer);
                WritePrimitiveType(poly, GLPrimitiveType.LineStrip, writer);
                WritePrimitiveType(poly, GLPrimitiveType.Triangles, writer);
                WritePrimitiveType(poly, GLPrimitiveType.TriangleFan, writer);
                WritePrimitiveType(poly, GLPrimitiveType.TriangleStrip, writer);

                writer.WriteEndElement(); //mesh
                writer.WriteEndElement(); //geometry
            }
        }

        private static unsafe void WritePrimitiveType(GLPolygon poly, GLPrimitiveType type, XmlWriter writer)
        {
            int count = 0;
            foreach (GLPrimitive prim in poly._primitives)
                if (prim._type == type)
                    switch (type)
                    {
                        case GLPrimitiveType.Lines:
                            count += prim._elements / 2;
                            break;

                        case GLPrimitiveType.LineStrip:
                            count += prim._elements - 1;
                            break;

                        case GLPrimitiveType.Triangles:
                            count += prim._elements / 3;
                            break;

                        case GLPrimitiveType.TriangleFan:
                            count += prim._elements - 2;
                            break;

                        case GLPrimitiveType.TriangleStrip:
                            count += prim._elements - 2;
                            break;
                    }
        

            if (count != 0)
            {
                switch (type)
                {
                    case GLPrimitiveType.Lines:
                        writer.WriteStartElement("lines");
                        break;

                    case GLPrimitiveType.LineStrip:
                        writer.WriteStartElement("linestrips");
                        break;

                    case GLPrimitiveType.Triangles:
                        writer.WriteStartElement("triangles");
                        break;

                    case GLPrimitiveType.TriangleFan:
                        writer.WriteStartElement("trifans");
                        break;

                    case GLPrimitiveType.TriangleStrip:
                        writer.WriteStartElement("tristrips");
                        break;
                }
                writer.WriteAttributeString("count", count.ToString());

                writer.WriteAttributeString("material", "initialShadingGroup");

                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "VERTEX");
                writer.WriteAttributeString("source", "#" + poly._name + "_vertices");
                writer.WriteAttributeString("offset", "0");
                writer.WriteEndElement();

                foreach (GLPrimitive prim in poly._primitives)
                    if (prim._type == type)
                    {
                        writer.WriteStartElement("p");

                        writer.WriteString(prim._vertexIndices[0].ToString());
                        for (int i = 1; i < prim._elements; i++)
                            writer.WriteString(" " + prim._vertexIndices[i].ToString());

                        writer.WriteEndElement();
                    }

                writer.WriteEndElement();
            }
        }

        private static unsafe void WriteNodes(GLModel model, XmlWriter writer)
        {
            foreach (GLPolygon poly in model._polygons)
            {
                writer.WriteStartElement("node");
                writer.WriteAttributeString("id", poly._name + "_surface");
                writer.WriteAttributeString("name", poly._name + "_surface");
                writer.WriteAttributeString("type", "NODE");

                writer.WriteStartElement("instance_geometry");
                writer.WriteAttributeString("url", "#" + poly._name);
                writer.WriteStartElement("bind_material");
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("instance_material");
                writer.WriteAttributeString("symbol", "initialShadingGroup");
                writer.WriteAttributeString("target", "#lambert1");

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        //private static void WriteVertices()
        //{
        //}
    }
}
