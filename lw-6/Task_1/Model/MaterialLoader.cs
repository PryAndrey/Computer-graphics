using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Assimp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

public class MaterialLoader
{
    private Dictionary<int, int> materialIndexToTextureId = new Dictionary<int, int>();
    private Dictionary<string, int> filePathToTextureId = new Dictionary<string, int>();

    public int GetTextureId(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return 0;
        }

        if (filePathToTextureId.TryGetValue(filePath, out int existingTexId))
        {
            return existingTexId;
        }

        if (!File.Exists(filePath))
        {
            filePathToTextureId[filePath] = 0;
            return 0;
        }

        int texId = 0;
        try
        {
            texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            using (Bitmap bitmap = new Bitmap(filePath))
            {
                BitmapData data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    data.Width, data.Height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte,
                    data.Scan0);
                bitmap.UnlockBits(data);
            }

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int)OpenTK.Graphics.ES20.TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int)OpenTK.Graphics.ES20.TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            filePathToTextureId[filePath] = texId;

            return texId;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error loading texture file '{filePath}': {ex.Message}");

            filePathToTextureId[filePath] = 0;

            if (texId != 0 && GL.IsTexture(texId)) GL.DeleteTexture(texId);
            return 0;
        }
        finally
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
    
    public void LoadMaterialTextures(Scene scene, int materialIndex)
    {
        if (materialIndex < 0 || materialIndex >= scene.MaterialCount) return;

        if (materialIndexToTextureId.ContainsKey(materialIndex))
        {
            return;
        }

        Material material = scene.Materials[materialIndex];
        int texIdForMaterial = 0;

        if (material.GetMaterialTextureCount(TextureType.Diffuse) > 0)
        {
            if (material.GetMaterialTexture(TextureType.Diffuse, 0, out TextureSlot textureSlot))
            {
                string filePath = textureSlot.FilePath;
                texIdForMaterial = GetTextureId(filePath);
            }
            else
            {
                System.Console.WriteLine($"Warning: Material {materialIndex} reports diffuse textures but slot 0 is unavailable.");
            }
        }

        materialIndexToTextureId[materialIndex] = texIdForMaterial;
    }
    
    public void ApplyMaterial(Material material, int materialIndex)
    {
        Color4 ambientColor = ConvertColor(material.ColorAmbient);
        Color4 diffuseColor = ConvertColor(material.ColorDiffuse);
        Color4 specularColor = ConvertColor(material.ColorSpecular);
        float shininess = material.Shininess;

        GL.Color4(diffuseColor);

        GL.Material(MaterialFace.Front, MaterialParameter.Ambient, ambientColor);
        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, diffuseColor);
        GL.Material(MaterialFace.Front, MaterialParameter.Specular, specularColor);
        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, shininess);

        if (materialIndexToTextureId.TryGetValue(materialIndex, out int texId) && texId != 0)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texId);
        }
        else
        {
            GL.Disable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }

    private Color4 ConvertColor(Color4D color4d)
    {
        return new Color4(color4d.R, color4d.G, color4d.B, color4d.A);
    }
}