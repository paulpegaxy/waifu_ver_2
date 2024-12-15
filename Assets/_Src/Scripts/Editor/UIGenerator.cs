using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Doozy.Runtime.UIManager.ScriptableObjects;
using Doozy.Editor.UIManager.ScriptableObjects;
using Doozy.Runtime.Common;

namespace Game.GameTools
{
	public class UIGenerator
	{
	
		private static Dictionary<string, Dictionary<string, List<string>>> _database = new();

		[MenuItem("Assets/Game Tools/Generate/UIId", false, 1)]
		static void GenerateId()
		{
			_database.Clear();

			GenerateUIView();
			GenerateUIButton();
			GenerateUIToggle();

			string data = "";
			data += $"public partial class UIId\n{{\n";
			foreach (KeyValuePair<string, Dictionary<string, List<string>>> pairType in _database)
			{
				List<string> names = new List<string>();
				data += $"\tpublic enum {pairType.Key}Category\n\t{{\n";
				foreach (KeyValuePair<string, List<string>> pairCategory in pairType.Value)
				{
					data += $"\t\t{pairCategory.Key},\n";
					foreach (string name in pairCategory.Value)
					{
						if (!names.Contains(name))
						{
							names.Add(name);
						}
					}
				}
				data += "\t}\n\n";

				data += $"\tpublic enum {pairType.Key}Name\n\t{{\n";
				foreach (string name in names)
				{
					data += $"\t\t{name},\n";
				}
				data += "\t}\n\n";
			}

			data += GenerateUIPopup();

			string path = Path.Combine(GameConsts.PATH_AUTO_GEN_UI, "UIId.cs");
			File.WriteAllText(path, data);

			AssetDatabase.Refresh();
		}

		static void GenerateUIView()
		{
			var items = UIViewIdDatabase.instance.database.items;
			foreach (var item in items)
			{
				AddToDatabase("UIView", item);
			}
		}

		static void GenerateUIButton()
		{
			var items = UIButtonIdDatabase.instance.database.items;
			foreach (var item in items)
			{
				AddToDatabase("UIButton", item);
			}
		}

		static void GenerateUIToggle()
		{
			var items = UIToggleIdDatabase.instance.database.items;
			foreach (var item in items)
			{
				AddToDatabase("UIToggle", item);
			}
		}

		static string GenerateUIPopup()
		{
			var items = UIPopupDatabase.instance.database;
			var data = "";

			data += "\tpublic enum UIPopupName\n\t{\n";
			data += "\t\tNone,\n";
			foreach (var item in items)
			{
				data += $"\t\t{item.prefabName},\n";
			}
			data += "\t}\n\n";
			data += "}";

			return data;
		}

		static void AddToDatabase(string key, CategoryNameItem item)
		{
			if (!_database.ContainsKey(key))
			{
				_database.Add(key, new Dictionary<string, List<string>>());
			}

			if (!_database[key].ContainsKey(item.category))
			{
				_database[key].Add(item.category, new List<string>());
			}

			if (!_database[key][item.category].Contains(item.name))
			{
				_database[key][item.category].Add(item.name);
			}
		}
	}
}