using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using JetBrains.Annotations;
using System.IO;
using UnityEditor.Sprites;

public class SpriteToTexture : EditorWindow
{
    string folderName;
    Texture2D currentTexture;
    DefaultAsset targetFolder = null;

    [MenuItem("CustomTools/Sprite to texture")]
    static void Init()
    {
        SpriteToTexture window =
            (SpriteToTexture)GetWindow(typeof(SpriteToTexture));
    }

    void OnGUI()
    {

        currentTexture = (Texture2D)EditorGUILayout.ObjectField(currentTexture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

        folderName = EditorGUILayout.TextField("File Name:", folderName);
        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Select Folder",targetFolder,typeof(DefaultAsset),false);

        if (GUILayout.Button("Sprite to texture"))
        {
            SpriteToTexture();
        }

        if(GUILayout.Button("Clear folder"))
        {
            Debug.Log("So far so good1");
            ClearFolder();
            
        }

        void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
        {
            byte[] _bytes = _texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(_fullPath, _bytes);
            Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
        }

        void ClearFolder()
        {
            string targetFolderPath = AssetDatabase.GetAssetPath(targetFolder);
            string[] assetsToClear = Directory.GetFiles(targetFolderPath);
            Debug.Log("So far so good2");
            for (int i = 0; i< assetsToClear.Length; i++)
            {
                Debug.Log("So far so good3");
                string assetPathToDelete = assetsToClear[i];
                Debug.Log(assetPathToDelete + " deleted");
                AssetDatabase.DeleteAsset(assetPathToDelete);
            }
        }

        void SpriteToTexture()
        {
            string spriteSheet = AssetDatabase.GetAssetPath(currentTexture);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

            //Sprite[] sprites = Resources.LoadAll<Sprite>(currentTexture.name);
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite currentSprite = sprites[i];
                //Texture2D croppedTexture = SpriteUtility.GetSpriteTexture(currentSprite,true);
                Texture2D croppedTexture = ConvertSpriteToTexture(currentSprite);

                SaveTextureAsPNG(croppedTexture, AssetDatabase.GetAssetPath(targetFolder) + "/" + currentSprite.name + ".png");
            }
        }


        Texture2D ConvertSpriteToTexture(Sprite sprite)
        {
            try
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    int textureWidth = (int)System.Math.Ceiling(sprite.textureRect.width);
                    int textureHeight = (int)System.Math.Ceiling(sprite.textureRect.height);
                    Texture2D newText = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
                    
                    Color[] colors = newText.GetPixels();
                    Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.y),
                                                                 textureWidth,
                                                                 textureHeight,
                                                                 0);
                    Debug.Log(textureHeight * textureWidth);
                    Debug.Log(colors.Length + "_" + newColors.Length);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    return newText;
                }
                else
                    return sprite.texture;
            }
            catch
            {
                return sprite.texture;
            }
        }





    }
}
