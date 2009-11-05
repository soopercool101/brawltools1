using System;

namespace BrawlLib
{
    public static class ExportFilters
    {
        public static string TEX0 =
            "All Image Formats (*.png,*.tga,*.tif,*.tiff,*.bmp,*.jpg,*.jpeg,*.gif,*.tex0)|*.png;*.tga;*.tif;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif;*.tex0|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tif, *.tiff)|*.tif;*.tiff|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif|" +
            "TEX0 Raw Texture (*.tex0)|*.tex0";

        public static string MDL0 =
            "MDL0 Raw Model (*.mdl0)|*.mdl0";

        public static string CHR0 =
            "CHR0 Raw Animation (*.chr0)|*.chr0";

        public static string PLT0 =
            "PLT0 Raw Palette (*.plt0)|*.plt0";

        public static string MSBin =
            "MSBin Message List (*.msbin)|*.msbin";
    }
}
