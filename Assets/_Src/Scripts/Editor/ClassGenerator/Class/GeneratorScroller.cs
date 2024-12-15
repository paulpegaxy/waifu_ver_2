using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Game.GameTools
{
	public class GeneratorScroller : Generator
	{
		[ShowInInspector, ReadOnly]
		public string Name => CellViewType == null ? "" : CellViewType.GetType().Name.Replace("CellViewType", "");

		[EnumToggleButtons]
		public ScrollerType ScrollerType;

		[ShowIf("ScrollerType", ScrollerType.Multiple), TypeFilter("GetCellViewTypes")]
		public Enum CellViewType;

		public IEnumerable<Type> GetCellViewTypes()
		{
			return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => t.IsEnum && t.Name.EndsWith("CellViewType"));
		}

		[Button]
		public void GenerateClass()
		{
			Generate(true);
			AssetDatabase.Refresh();
		}

		[Button]
		public void GeneratePrefab()
		{
			Generate(false);
			AssetDatabase.Refresh();
		}

		public override void Generate(bool isClass)
		{
			var root = "Assets/_Src";
			var model = $"{root}/Scripts/Patterns/Factory/AutoGenModel/{Name}";
			var window = $"{root}/Scripts/UI/Windows/AutoGen/{Name}";
			var prefab = $"{root}/_Prefabs/UI/Windows/AutoGen/{Name}";
			var values = Enum.GetValues(CellViewType.GetType());

			GenerateModel(model, CellViewType.GetType().Name, "");
			foreach (var value in values)
			{
				if (isClass)
				{
					GenerateModel(model, CellViewType.GetType().Name, value.ToString());
					GenerateCellview(window, value.ToString());
				}
				else
				{
					GenerateCellViewPrefab(prefab, value.ToString());
				}
			}

			if (isClass)
			{
				GenerateScroller(window, CellViewType.GetType().Name);
			}
			else
			{
				GenerateScrollerPrefab(prefab);
			}
		}

		private void GenerateModel(string path, string cellViewType, string name = "")
		{
			var abstractClass = $"Model{Name}CellView";
			var inheritClass = $"Model{Name}CellView{name}";
			var content = "";

			if (name == "")
			{
				content = $"using Game.UI;\nusing Game.Defines;\n\n";
				content += $"namespace Game.Model\n{{\n\tpublic abstract class {inheritClass} : IESModel<{cellViewType}>\n\t{{\n\t";
				content += $"\tpublic {cellViewType} Type {{ get; set; }}\n\t";
				content += $"}}\n}}";
			}
			else
			{
				content = $"using Game.Defines;\n\n";
				content += $"namespace Game.Model\n{{\n\tpublic class {inheritClass} : {abstractClass}\n\t{{\n\t";
				content += $"\tpublic {inheritClass}()\n\t\t{{\n\t\t\tType = {cellViewType}.{name};\n\t\t}}\n\t";
				content += $"}}\n}}";
			}

			Write(path, $"{inheritClass}.cs", content);
		}

		private void GenerateCellview(string path, string name = "")
		{
			var className = $"{Name}CellView{name}";
			var content = $"using Game.Model;\n\n";
			content += $"namespace Game.UI\n{{\n\tpublic class {className} : ESCellView<Model{Name}CellView>\n\t{{\n\t\t";
			content += $"public override void SetData(Model{Name}CellView model)\n\t\t{{\n\t\t\t" +
			           $"if (model is Model{Name}CellView{name} data)\n\t\t\t{{\n\n\t\t\t}}" +
			           $"\n\t\t}}\n\t}}\n}}";

			Write(path, $"{className}.cs", content);
		}

		private void GenerateScroller(string path, string cellViewType)
		{
			var className = $"{Name}Scroller";
			var content = $"using Game.Model;\nusing Game.Defines;\n\n";
			content += $"namespace Game.UI\n{{\n\tpublic class {className} : ESMulti<{cellViewType}, Model{Name}CellView, ESCellView<Model{Name}CellView>>\n\t{{\n\t}}\n}}";

			Write(path, $"{className}.cs", content);
		}

		private void GenerateCellViewPrefab(string path, string name)
		{
			var source = "Assets/_Src/Scripts/Editor/ClassGenerator/Prefab";
			var cellview = $"{source}/CellView.prefab";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var destination = $"{path}/{Name}CellView{name}.prefab";
			if (!File.Exists(destination))
			{
				FileUtil.CopyFileOrDirectory(cellview, destination);
				AssetDatabase.Refresh();

				var prefab = PrefabUtility.LoadPrefabContents(destination);
				var component = prefab.AddComponent(Type.GetType($"Game.UI.{Name}CellView{name},Assembly-CSharp"));

				SetField(component, "cellIdentifier", $"{Name}_Cell_View_{name}".ToLower());

				PrefabUtility.SaveAsPrefabAsset(prefab, destination);
			}
		}

		private void GenerateScrollerPrefab(string path)
		{
			var source = "Assets/_Src/Scripts/Editor/ClassGenerator/Prefab";
			var scroller = $"{source}/Scroller.prefab";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var destination = $"{path}/{Name}Scroller.prefab";
			if (!File.Exists(destination))
			{
				FileUtil.CopyFileOrDirectory(scroller, destination);
				AssetDatabase.Refresh();

				var prefab = PrefabUtility.LoadPrefabContents(destination);
				var component = prefab.AddComponent(Type.GetType($"Game.UI.{Name}Scroller,Assembly-CSharp"));

				PrefabUtility.SaveAsPrefabAsset(prefab, destination);
			}
		}

		private void SetField(object obj, string name, object value)
		{
			var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.Instance);
			field?.SetValue(obj, value);
		}

		private void Write(string path, string name, string content)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			var filename = Path.Combine(path, $"{name}");
			if (!File.Exists(filename))
			{
				File.WriteAllText(filename, content);
			}
		}
	}

	public enum ScrollerType
	{
		Single,
		Multiple
	}
}