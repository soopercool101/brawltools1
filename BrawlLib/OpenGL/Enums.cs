using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.OpenGL
{
    public enum GLBlendFactor : uint
    {
        ZERO = 0,
        ONE = 1,
        SRC_COLOR = 0x0300,
        ONE_MINUS_SRC_COLOR = 0x0301,
        SRC_ALPHA = 0x0302,
        ONE_MINUS_SRC_ALPHA = 0x0303,
        DST_ALPHA = 0x0304,
        ONE_MINUS_DST_ALPHA = 0x0305,
        DST_COLOR = 0x0306,
        ONE_MINUS_DST_COLOR = 0x0307,
        SRC_ALPHA_SATURATE = 0x0308
    }

    public enum GLShadingModel : uint
    {
        FLAT = 0x1D00,
        SMOOTH = 0x1D01
    }

    public enum GLFunction : uint
    {
        NEVER = 0x0200,
        LESS = 0x0201,
        EQUAL = 0x0202,
        LEQUAL = 0x0203,
        GREATER = 0x0204,
        NOTEQUAL = 0x0205,
        GEQUAL = 0x0206,
        ALWAYS = 0x0207
    }

    public enum GLHintTarget : int
    {
        PERSPECTIVE_CORRECTION_HINT = 0x0C50,
        POINT_SMOOTH_HINT = 0x0C51,
        LINE_SMOOTH_HINT = 0x0C52,
        POLYGON_SMOOTH_HINT = 0x0C53,
        FOG_HINT = 0x0C54
    }

    public enum GLHintMode : uint
    {
        DONT_CARE = 0x1100,
        FASTEST = 0x1101,
        NICEST = 0x1102
    }

    public enum GLAccumOp : uint
    {
        Accum = 0x0100,
        Load = 0x0101,
        Return = 0x0102,
        Mult = 0x0103,
        Add = 0x0104
    }

    public enum GLAlphaFunc : uint
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207
    }

    public enum GLPrimitiveType : uint
    {
        Points = 0x0000,
        Lines = 0x0001,
        LineLoop = 0x0002,
        LineStrip = 0x0003,
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        Quads = 0x0007,
        QuadStrip = 0x0008,
        Polygon = 0x0009
    }

    public enum GLClearMask : uint
    {
        DepthBuffer = 0x00000100,
        StencilBuffer = 0x00000400,
        ColorBuffer = 0x00004000
    }

    public enum GLMatrixMode : uint
    {
        ModelView = 0x1700,
        Projection = 0x1701,
        Texture = 0x1702
    }

    public enum GLFace : uint
    {
        Front = 0x404,
        Back = 0x405,
        FrontAndBack = 0x408
    }

    public enum GLPolygonMode : uint
    {
        Point = 0x1B00,
        Line = 0x1B01,
        Fill = 0x1B02
    }

    public enum GLTextureTarget : uint
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        Texture3D = 0x806F,
        TextureCubeMap = 0x8513
    }

    public enum GLTexImageTarget : uint
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        ProxyTexture1D = 0x8063,
        ProxyTexture2D = 0x8064,

        Texture3D = 0x806F,
        TextureCubeMap = 0x8513
    }

    public enum GLTextureParameter : uint
    {
        MagFilter = 0x2800,
        MinFilter = 0x2801,
        WrapS = 0x2802,
        WrapT = 0x2803,
        MinLOD = 0x813A,
        MaxLOD = 0x813B,
        BaseLevel = 0x813C,
        MaxLevel = 0x813D
    }

    public enum GLTextureWrapMode : uint
    {
        CLAMP = 0x2900,
        REPEAT = 0x2901,
        CLAMP_TO_EDGE = 0x812F,
        CLAMP_TO_BORDER = 0x812D
    }

    public enum GLTextureFilterMode : uint
    {
        NEAREST						= 0x2600,
	LINEAR						= 0x2601
    }

    public enum GLInternalPixelFormat : uint
    {
        R3_G3_B2 = 0x2A10,
        GL_ALPHA = 0x1906,
        ALPHA4 = 0x803B,
        ALPHA8 = 0x803C,
        ALPHA12 = 0x803D,
        ALPHA16 = 0x803E,
        LUMINANCE4 = 0x803F,
        LUMINANCE8 = 0x8040,
        LUMINANCE12 = 0x8041,
        LUMINANCE16 = 0x8042,
        LUMINANCE4_ALPHA4 = 0x8043,
        LUMINANCE6_ALPHA2 = 0x8044,
        LUMINANCE8_ALPHA8 = 0x8045,
        LUMINANCE12_ALPHA4 = 0x8046,
        LUMINANCE12_ALPHA12 = 0x8047,
        LUMINANCE16_ALPHA16 = 0x8048,
        INTENSITY = 0x8049,
        INTENSITY4 = 0x804A,
        INTENSITY8 = 0x804B,
        INTENSITY12 = 0x804C,
        INTENSITY16 = 0x804D,
        RGB = 0x1906,
        RGB4 = 0x804F,
        RGB5 = 0x8050,
        RGB8 = 0x8051,
        RGB10 = 0x8052,
        RGB12 = 0x8053,
        RGB16 = 0x8054,
        RGBA2 = 0x8055,
        RGBA4 = 0x8056,
        RGB5_A1 = 0x8057,
        RGBA8 = 0x8058,
        RGB10_A2 = 0x8059,
        RGBA = 0x1907,
        RGBA12 = 0x805A,
        RGBA16 = 0x805B
    }

    public enum GLErrorCode : uint
    {
        NO_ERROR = 0,
        INVALID_ENUM = 0x0500,
        INVALID_VALUE = 0x0501,
        INVALID_OPERATION = 0x0502,
        STACK_OVERFLOW = 0x0503,
        STACK_UNDERFLOW = 0x0504,
        OUT_OF_MEMORY = 0x0505
    }

    public enum GLPixelDataFormat : uint
    {
        COLOR_INDEX = 0x1900,
        STENCIL_INDEX = 0x1901,
        DEPTH_COMPONENT = 0x1902,
        RED = 0x1903,
        GREEN = 0x1904,
        BLUE = 0x1905,
        ALPHA = 0x1906,
        RGB = 0x1907,
        RGBA = 0x1908,
        LUMINANCE = 0x1909,
        LUMINANCE_ALPHA = 0x190A,
        BGR = 0x80E0,
        BGRA = 0x80E1
    }

    public enum GLPixelDataType : uint
    {
        BITMAP = 0x1A00,

        BYTE = 0x1400,
        UNSIGNED_BYTE = 0x1401,
        SHORT = 0x1402,
        UNSIGNED_SHORT = 0x1403,
        INT = 0x1404,
        UNSIGNED_INT = 0x1405,
        FLOAT = 0x1406,

        UNSIGNED_BYTE_3_3_2 = 0x8032,
        UNSIGNED_SHORT_4_4_4_4 = 0x8033,
        UNSIGNED_SHORT_5_5_5_1 = 0x8034,
        UNSIGNED_INT_8_8_8_8 = 0x8035,
        UNSIGNED_INT_10_10_10_2 = 0x8036,
        UNSIGNED_BYTE_2_3_3_REV = 0x8362,
        UNSIGNED_SHORT_5_6_5 = 0x8363,
        UNSIGNED_SHORT_5_6_5_REV = 0x8364,
        UNSIGNED_SHORT_4_4_4_4_REV = 0x8365,
        UNSIGNED_SHORT_1_5_5_5_REV = 0x8366,
        UNSIGNED_INT_8_8_8_8_REV = 0x8367,
        UNSIGNED_INT_2_10_10_10_REV = 0x8368
    }

    public enum GLTexEnvTarget : uint
    {
        TextureEnvironment = 0x2300,
        FilterControl = 0x8500,
        PointSprite = 0x8861
    }

    public enum GLTexEnvParam : uint
    {
        TEXTURE_ENV_MODE = 0x2200,
        TEXTURE_ENV_COLOR = 0x2201,
        TEXTURE_LOD_BIAS = 0x8501,
        COMBINE_RGB = 0x8571,
        COMBINE_ALPHA = 0x8572
    }

    public enum GLTexEnvMode : uint
    {
        MODULATE = 0x2100,
        DECAL = 0x2101
    }

    public enum GLGetMode : uint
    {
        MATRIX_MODE = 0x0BA0,
        NORMALIZE = 0x0BA1,
        VIEWPORT = 0x0BA2,
        MODELVIEW_STACK_DEPTH = 0x0BA3,
        PROJECTION_STACK_DEPTH = 0x0BA4,
        TEXTURE_STACK_DEPTH = 0x0BA5,
        MODELVIEW_MATRIX = 0x0BA6,
        PROJECTION_MATRIX = 0x0BA7,
        TEXTURE_MATRIX = 0x0BA8
    }

    public enum GLEnableCap : uint
    {
        Fog = 0x0B60,
        Lighting = 0x0B50,
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        LineStipple = 0x0B24,
        PolygonStipple = 0x0B42,
        CullFace = 0x0B44,
        AlphaTest = 0x0BC0,
        Blend = 0x0BE2,
        IndexLogicOp = 0x0BF1,
        ColorLogicOp = 0x0BF2,
        Dither = 0x0BD0,
        StencilTest = 0x0B90,
        DepthTest = 0x0B71,
        ClipPlane0 = 0x3000,
        ClipPlane1 = 0x3001,
        ClipPlane2 = 0x3002,
        ClipPlane3 = 0x3003,
        ClipPlane4 = 0x3004,
        ClipPlane5 = 0x3005,
        Light0 = 0x4000,
        Light1 = 0x4001,
        Light2 = 0x4002,
        Light3 = 0x4003,
        Light4 = 0x4004,
        Light5 = 0x4005,
        Light6 = 0x4006,
        Light7 = 0x4007

        //use GetPName TEXTURE_GEN_S
        //use GetPName TEXTURE_GEN_T
        //use GetPName TEXTURE_GEN_R
        //use GetPName TEXTURE_GEN_Q
        //use GetPName MAP1_VERTEX_3
        //use GetPName MAP1_VERTEX_4
        //use GetPName MAP1_COLOR_4
        //use GetPName MAP1_INDEX
        //use GetPName MAP1_NORMAL
        //use GetPName MAP1_TEXTURE_COORD_1
        //use GetPName MAP1_TEXTURE_COORD_2
        //use GetPName MAP1_TEXTURE_COORD_3
        //use GetPName MAP1_TEXTURE_COORD_4
        //use GetPName MAP2_VERTEX_3
        //use GetPName MAP2_VERTEX_4
        //use GetPName MAP2_COLOR_4
        //use GetPName MAP2_INDEX
        //use GetPName MAP2_NORMAL
        //use GetPName MAP2_TEXTURE_COORD_1
        //use GetPName MAP2_TEXTURE_COORD_2
        //use GetPName MAP2_TEXTURE_COORD_3
        //use GetPName MAP2_TEXTURE_COORD_4
        //use GetPName POINT_SMOOTH
        //use GetPName LINE_SMOOTH
        //use GetPName POLYGON_SMOOTH
        //use GetPName SCISSOR_TEST
        //use GetPName COLOR_MATERIAL
        //use GetPName NORMALIZE
        //use GetPName AUTO_NORMAL
        //use GetPName POLYGON_OFFSET_POINT
        //use GetPName POLYGON_OFFSET_LINE
        //use GetPName POLYGON_OFFSET_FILL
        //use GetPName VERTEX_ARRAY
        //use GetPName NORMAL_ARRAY
        //use GetPName COLOR_ARRAY
        //use GetPName INDEX_ARRAY
        //use GetPName TEXTURE_COORD_ARRAY
        //use GetPName EDGE_FLAG_ARRAY

    }
}
