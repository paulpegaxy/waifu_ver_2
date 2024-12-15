using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Game.Runtime;
using I2.Loc;

namespace Game.GameTools
{
	public class LocalizeGenerator
	{
		[MenuItem("Assets/Game Tools/Generate/TextId", false, 1)]
		static void GenerateId()
		{
			var ids = LocalizationManager.GetTermsList();
			var data = "public enum TextId\n{\n";
			data += $"\tNone,\n";

			foreach (string id in ids)
			{
				string[] texts = id.Split('/');
				string textId = $"{texts[0].ToLower().SnakeToPascal()}_{texts[1].ToLower().SnakeToPascal()}";
				data += $"\t{textId},\n";
			}
			data += "}";

			string path = Path.Combine("Assets/_Src/Scripts/Defines/AutoGenerate", "TextId.cs");
			File.WriteAllText(path, data);

			AssetDatabase.Refresh();
		}
	}
}