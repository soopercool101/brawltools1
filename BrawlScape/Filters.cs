using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlScape
{
    public static class Filters
    {
        public static string TextureReplaceFilter =
            "All Image Formats (*.png,*.tga,*.tiff,*.bmp,*.jpg,*.jpeg,*.gif)|*.png;*.tga;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tiff)|*.tiff|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif|" +
            "TEX0 Raw Texture (*.tex0)|*.tex0";

        public static string CostumeExportFilter =
            "Archive Pair (*.pcs + *.pac)|*.pac;*.pcs|" +
            "PAC Archive (*.pac)|*.pac|" +
            "Compressed PAC Archive (*.pcs)|*.pcs";

        public static string CostumeImportFilter =
            "PAC/PCS Archive (*.pac, *.pcs)|*.pac;*.pcs";
    }
}
