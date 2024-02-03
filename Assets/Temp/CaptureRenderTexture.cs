using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ImageFormat
{
    png,tiff
}

public class CaptureRenderTexture : MonoBehaviour
{
    public RenderTexture texture;
    public TextureFormat format = TextureFormat.RGBAFloat;
    public ImageFormat imageFormat = ImageFormat.tiff;
    public bool isLinear = true;

    [ContextMenu("capture")]
    private void capture()
    {
        Texture2D t = new Texture2D(texture.width, texture.height, format, false, isLinear);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = texture;
        t.ReadPixels(new Rect(0, 0, t.width, t.height), 0, 0);
        t.Apply();
        RenderTexture.active = previous;
        switch (imageFormat)
        {
            case ImageFormat.png:
                byte[] pixels = t.EncodeToPNG();
                string filePath = UnityEditor.EditorUtility.SaveFilePanel("save","", "texture", "png");
                File.WriteAllBytes(filePath, pixels);
                break;

            case ImageFormat.tiff:
                
                //byte[] pixels = t.Enc;
                //string filePath = UnityEditor.EditorUtility.SaveFilePanelInProject("save", "texture", "png", "saving");
                //File.WriteAllBytes(filePath, pixels);
                break;

        }
    }
}
