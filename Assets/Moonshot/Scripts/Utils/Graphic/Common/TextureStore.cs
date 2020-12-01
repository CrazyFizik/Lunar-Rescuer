using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace Utils.Graphics
{
    public static class TextureStore
    {

        public static void WriteTextureToPlayerPrefs(string tag, Texture2D tex)
        {
            // if texture is png otherwise you can use tex.EncodeToJPG().
            byte[] texByte = tex.EncodeToPNG();

            // convert byte array to base64 string
            string base64Tex = System.Convert.ToBase64String(texByte);

            // write string to playerpref
            PlayerPrefs.SetString(tag, base64Tex);
            PlayerPrefs.Save();
        }

        public static Texture2D ReadTextureFromPlayerPrefs(string tag)
        {
            // load string from playerpref
            string base64Tex = PlayerPrefs.GetString(tag, null);

            if (!string.IsNullOrEmpty(base64Tex))
            {
                // convert it to byte array
                byte[] texByte = System.Convert.FromBase64String(base64Tex);
                Texture2D tex = new Texture2D(2, 2);

                //load texture from byte array
                if (tex.LoadImage(texByte))
                {
                    return tex;
                }
            }

            return null;
        }

        public static void SavePicture(Color[,] spr, string path)
        {
            Texture2D tex = new Texture2D(spr.GetLength(0), spr.GetLength(1), TextureFormat.RGBAFloat, true, true);
            for (int x = 0; x < spr.GetLength(0); x++)
            {
                for (int y = 0; y < spr.GetLength(1); y++)
                {
                    tex.SetPixel(x, y, spr[x, y]);
                }
            }
            tex.Apply();

            using (FileStream fs = File.Create(path))
            {
                //File.WriteAllBytes(sprite_sheet_path, sprite_tiles.EncodeToPNG());
                fs.Write(tex.EncodeToPNG(), 0, tex.EncodeToPNG().Length);
            }
            WaitForFile(path);
        }

        public static void SavePicture(Color[] spr, int w, int h, string path)
        {
            Texture2D tex = new Texture2D(w, h);
            tex.SetPixels(spr);
            tex.Apply();

            using (FileStream fs = File.Create(path))
            {
                //File.WriteAllBytes(sprite_sheet_path, sprite_tiles.EncodeToPNG());
                fs.Write(tex.EncodeToPNG(), 0, tex.EncodeToPNG().Length);
            }
            WaitForFile(path);
        }

        public static void SavePicture(Texture2D tex, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(tex.EncodeToPNG(), 0, tex.EncodeToPNG().Length);
            }
            //File.WriteAllBytes(path, tex.EncodeToPNG());
            WaitForFile(path);
        }

        public static void WaitForFile(string filename)
        {
            //This will lock the execution until the file is ready
            //TODO: Add some logic to make it async and cancelable
            while (!IsFileReady(filename)) { }
        }
        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return inputStream.Length > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Texture2D LoadPNG(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            return tex;
        }

    }
}