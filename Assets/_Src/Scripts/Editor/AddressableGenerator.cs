using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using Game.Runtime;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace Game.GameTools
{
	public class AddressableGenerator
	{
		private static readonly string _addressablePath = "Assets/_Src/ResourceData";
		private static readonly string _resourcePath = "Assets/_Src/Resources";


		[MenuItem("Assets/Game Tools/Generate/Addressable", false, 3)]
		static void Generate()
		{
			var folders = AssetDatabase.GetSubFolders(_addressablePath);
			var ignoreGroup = new List<string>()
			{
				"Built In Data"
			};

			var settings = AddressableAssetSettingsDefaultObject.Settings;
			var groups = settings.groups.FindAll(x => !ignoreGroup.Contains(x.Name));

			foreach (var group in groups)
			{
				settings.RemoveGroup(group);
			}

			foreach (var folder in folders)
			{
				string group = Path.GetFileName(folder);
				GenerateGroup(group, folder);
			}

			GenerateAnRId();
		}

		[MenuItem("Assets/Game Tools/Generate/AssetId", false, 2)]
		static void GenerateAnRId()
		{
			var ids = new Dictionary<string, List<string>>();
			GenerateId(_addressablePath, ref ids);
			GenerateId(_resourcePath, ref ids);

			var content = "public partial class AnR\n{\n";
			foreach (var group in ids)
			{
				content += string.Format($"\tpublic enum {group.Key}\n\t") + "{\n";
				foreach (var id in group.Value)
				{
					content += string.Format($"\t\t{id.SnakeToPascal()},\n");
				}
				content += "\t}\n\n";
			}
			content += "}";

			var path = Path.Combine("Assets/_Src/Scripts/Defines/AutoGenerate", "AssetId.cs");
			File.WriteAllText(path, content);

			AssetDatabase.Refresh();
		}

		static void GenerateId(string path, ref Dictionary<string, List<string>> ids)
		{
			var folders = AssetDatabase.GetSubFolders(path);
			foreach (var folder in folders)
			{
				var group = Path.GetFileName(folder) + "Key";
				var assets = AssetDatabase.FindAssets("", new string[] { folder });

				ids.Add(group, new List<string>());
				foreach (var asset in assets)
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(asset);
					if (!Directory.Exists(assetPath))
					{
						var name = Path.GetFileNameWithoutExtension(assetPath);
						var data = name.Split('_');
						var isNumber = int.TryParse(data[0], out _);

						if (isNumber)
						{
							name = name.Replace($"{data[0]}_", "");
						}

						if (!ids[group].Contains(name))
						{
							ids[group].Add(name);
						}
					}
				}
			}
		}

		static void GenerateGroup(string groupName, string folder)
		{
			var assets = AssetDatabase.FindAssets("", new string[] { folder });
			var assetGroup = GetGroupByName(groupName);
			var settings = AddressableAssetSettingsDefaultObject.Settings;


			if (groupName.Equals("BgGirl") || groupName.Equals("EntitySpine") || groupName.Equals("Sprite") || groupName.Equals("Vfx")
			    || groupName.Equals("MediaPicture"))
			{
				assetGroup.GetSchema<BundledAssetGroupSchema>().BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
			}

			foreach (var asset in assets)
			{
				var path = AssetDatabase.GUIDToAssetPath(asset);
				if (!Directory.Exists(path))
				{
					var name = Path.GetFileNameWithoutExtension(path).PascalToSnake();
					var entry = settings.CreateOrMoveEntry(asset, assetGroup);
					entry.SetAddress(name);
					var data = name.Split('_');
					var label = data[0];
					switch (groupName)
					{
						case "BgGirl":
							case "MediaPicture":
						case "EntitySpine":
							case "Vfx":
							entry.SetLabel(label, true, true);
							break;
						default:
							entry.SetLabel(groupName.ToLowerCase(), true, true);
							break;
					}
				}
			}
		}

		static AddressableAssetGroup GetGroupByName(string groupName)
		{
			var settings = AddressableAssetSettingsDefaultObject.Settings;

			if (groupName.Equals("Pet"))
			{
				groupName = "MainGame";
			}

			var assetGroup = settings.FindGroup(groupName);
			if (assetGroup == null)
			{
				assetGroup = settings.CreateGroup(groupName, false, false, true, settings.DefaultGroup.Schemas);
			}

			return assetGroup;
		}
	}
}