using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Game.Core;
using Template.Utils;

public class BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
	private const string rootPath = "Assets/_Src/Resources/Collections";
	private const string backupPath = "Assets/Backup";

	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		string[] assets = AssetDatabase.FindAssets("", new string[] { rootPath });
		Directory.CreateDirectory(backupPath);

		foreach (var asset in assets)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(asset);
			if (!Directory.Exists(assetPath))
			{
				string extension = Path.GetExtension(assetPath);
				if (extension != ".json") continue;

				byte[] data = File.ReadAllBytes(assetPath);
				byte[] encrypted = Crypto.Encrypt(data);

				string fileName = Path.GetFileName(assetPath);
				AssetDatabase.CopyAsset(assetPath, $"{backupPath}/{fileName}");
				AssetDatabase.DeleteAsset(assetPath);

				string encryptedFileName = $"{Path.GetFileNameWithoutExtension(assetPath)}.bytes";
				File.WriteAllBytes(Path.Join(rootPath, encryptedFileName), encrypted);
			}
		}
	}

	public void OnPostprocessBuild(BuildReport report)
	{
		string[] assets = AssetDatabase.FindAssets("", new string[] { backupPath });

		foreach (var asset in assets)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(asset);
			if (!Directory.Exists(assetPath))
			{
				string fileName = Path.GetFileName(assetPath);
				AssetDatabase.CopyAsset(assetPath, $"{rootPath}/{fileName}");
				AssetDatabase.DeleteAsset(assetPath);
			}
		}

		assets = AssetDatabase.FindAssets("", new string[] { rootPath });
		foreach (var asset in assets)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(asset);
			if (!Directory.Exists(assetPath))
			{
				string extension = Path.GetExtension(assetPath);
				if (extension != ".bytes") continue;

				AssetDatabase.DeleteAsset(assetPath);
			}
		}

		AssetDatabase.DeleteAsset(backupPath);
	}

}
