using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Codice.Client.Common;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Game.Runtime;
using Game.Model;
using Sirenix.OdinInspector;

namespace Game.GameTools
{
	public class GameEditorWindow : OdinMenuEditorWindow
	{

		[MenuItem("Window/Game Data Editor")]
		public static void Show()
		{
			((EditorWindow)GetWindow<GameEditorWindow>()).Show();
		}

		// [MenuItem("Assets/Game Tools/Game Editor #&g", false, 100)]
		// static void OpenWindow()
		// {
		// 	GetWindow<GameEditorWindow>().Show();
		// }

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			tree.Config.DrawSearchToolbar = true;
			MenuWidth = 250;

			InitFactory();

			var count = Enum.GetNames(typeof(CollectionType)).Length;
			for (int i = 0; i < count; i++)
			{
				CollectionType type = (CollectionType)i;
				AddModule(type, tree);
			}

			return tree;
		}

		protected override void OnBeginDrawEditors()
		{
			OdinMenuItem itemSelected = MenuTree.Selection.FirstOrDefault();
			var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

			SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
			{
				if (itemSelected != null)
				{
					GUILayout.Label(itemSelected.GetFullPath().ToUpper());
				}

				var name = itemSelected.GetFullPath().Split('/').First();
				var type = (CollectionType)Enum.Parse(typeof(CollectionType), name.Replace(" ", ""));/**/

				if (itemSelected.Parent == null)
				{
					SelectCategory(type);
					if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create")))
					{
						Create(type);
					}
				}
				else
				{
					if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete")))
					{
						Delete(type, itemSelected);
					}
				}

				if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save All " + name)))
				{
					Save(type);
				}

				if (SirenixEditorGUI.ToolbarButton(new GUIContent($"Load {name.ToUpper()} from GGSheet")))
				{
					LoadGGSheetData(type);
				}
			}
			SirenixEditorGUI.EndHorizontalToolbar();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			int count = Enum.GetNames(typeof(CollectionType)).Length;
			for (int i = 0; i < count; i++)
			{
				var type = (CollectionType)i;
				var module = FactoryEditorModule.Get<IEditorModule>(type);
				module.CleanUp();
			}

			FactoryEditorModule.CleanUp();
			FactoryModelRarityParam.CleanUp();
			FactoryModelGameConfigParam.CleanUp();
			FactoryModelTrigger.CleanUp();
		}

		void InitFactory()
		{
			var types = new List<Type>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				Type[] assemblyTypes = assembly.GetTypes();
				foreach (Type type in assemblyTypes)
				{
					if (type.IsDefined(typeof(FactoryAttribute)))
					{
						types.Add(type);
					}
				}
			}

			FactoryEditorModule.Setup(types);
			FactoryModelRarityParam.Setup(types);
			FactoryModelGameConfigParam.Setup(types);
			FactoryModelTrigger.Setup(types);
		}

		void AddModule(CollectionType type, OdinMenuTree tree)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.Init(tree);
		}

		void SelectCategory(CollectionType type)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.SelectCategoryToNewDataParam();
		}

		void Create(CollectionType type)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.Create();
		}

		void Delete(CollectionType type, OdinMenuItem item)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.Delete(item);
		}

		void LoadGGSheetData(CollectionType type)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.LoadGGSheetData();
		}

		void Save(CollectionType type)
		{
			IEditorModule module = FactoryEditorModule.Get<IEditorModule>(type);
			module?.Save();
		}

		// [Button]
		// void Save()
		// {
		// 	string json = JsonConvert.SerializeObject(_companionList);
		// 	string path = Path.Join("Assets/MainGame/Addressable/Collections", "collection_companions.json");
		// 	File.WriteAllText(path, json);

		// 	AssetDatabase.Refresh();
		// }
	}
}
