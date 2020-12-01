using System;
using System.IO;
using UnityEngine;

namespace SharpNoise.Utilities.Imaging
{
    /// <summary>
    /// Implements an image, a 2-dimensional array of color values.
    /// </summary>
    /// <remarks>
    /// An image can be used to store a color texture.
    ///
    /// These color values are of type <see cref="Color"/>.
    /// 
    /// The size (width and height) of the image can be specified during
    /// object construction or at any other time.
    ///
    /// The <see cref="GetValue"/> method returns the border value if the specified
    /// position lies outside of the image.
    /// </remarks>
    public class Image : Map<Color>
    {
        public override int UsedMemory
        {
            get
            {
                unsafe
                {
                    return values.Length * sizeof(Color);
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Image() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Image(int width, int height)
            : base(width, height)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Image(Image other)
            : base(other)
        {

        }

        /// <summary>
        /// Converts the Image to a System.Drawing.Bitmap
        /// </summary>
        /// <returns>Returns the created Bitmap</returns>
        /// <remarks>
        /// This isn't exactly optimised for speed. Will be slow for large Images.
        /// </remarks>
        public Texture2D ToTexture2D(TextureFormat imageFormat = TextureFormat.RGB24)
        {
            Texture2D tex = new Texture2D(Width, Height, imageFormat, false, true);
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    tex.SetPixel(x, y, GetValue(x, y).ToGdiColor());
                }
            }
            return tex;
        }

        /// <summary>
        /// Convert the image to a System.Drawing.Bitmap and save it to a file
        /// </summary>
        /// <param name="filename">The file to save the image to</param>
        /// <param name="imageFormat">The ImageFormat to use</param>
        public void SavePNG(string filename, TextureFormat imageFormat = TextureFormat.RGB24)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            try
            {
                var bytes = ToTexture2D(imageFormat).EncodeToPNG();
                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }

                //File.WriteAllBytes(filename, bytes);

                FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
                using (stream)
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                stream.Close();
            }
            catch (IOException exc)
            {
                throw new IOException(String.Format("Cannot write to given file '{0}'", filename), exc);
            }
        }
    }
}
