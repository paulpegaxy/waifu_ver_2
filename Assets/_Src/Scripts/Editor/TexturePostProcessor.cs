
using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{
	void OnPostprocessTexture(Texture2D texture)
	{
		if (!(assetPath.StartsWith("Assets/_GameAssets") && assetPath.Contains("Sprite"))) return;
		var importer = assetImporter as TextureImporter;
		importer.textureType = TextureImporterType.Sprite;
		importer.alphaSource = TextureImporterAlphaSource.FromInput;
		importer.alphaIsTransparency = true;
		importer.mipmapEnabled = false;
		TextureImporterSettings textureSettings = new TextureImporterSettings();
		importer.ReadTextureSettings(textureSettings);
		// textureSettings.spriteMeshType = SpriteMeshType.Tight;
		textureSettings.spriteExtrude = 0;
		textureSettings.spriteGenerateFallbackPhysicsShape = false;
		importer.SetTextureSettings(textureSettings);
		Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
		if (asset) {
			EditorUtility.SetDirty(asset);
		}
	}
}