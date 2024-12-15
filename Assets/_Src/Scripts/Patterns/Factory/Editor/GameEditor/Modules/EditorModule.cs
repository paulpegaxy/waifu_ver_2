using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sirenix.OdinInspector.Editor;
using Game.Runtime;
using Game.Model;

namespace Game.GameTools
{
	public interface IEditorModule
	{
		void Init(OdinMenuTree tree);
		void Create();
		void Save();
		void CleanUp();
		void LoadGGSheetData();
		void Delete(OdinMenuItem item);
		void SelectCategoryToNewDataParam();
	}

	public abstract class EditorModule<TModel> : IEditorModule
	{
		protected string _path = "Assets/_Src/Resources/Collections";
		protected CollectionType _type;
		protected OdinMenuTree _tree;
		protected TModel _data;
		protected List<TModel> _dataAll;

		protected abstract void AddMenu(TModel item);
		protected abstract bool ValidateCreate();
		protected abstract void DeleteItem(TModel item);
		protected abstract void Sort();
		protected virtual void AfterCreate(TModel item) { }

		protected virtual void InitializeParamData()
		{

		}

		public void Init(OdinMenuTree tree)
		{
			// Debug.Log($"Key {_path}/{GetKey()}");
			TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{_path}/{GetKey()}.{"json"}");
			if (textAsset == null)
			{
				UnityEngine.Debug.LogError($"File not found: {_path}");
				return;
			}

			_dataAll = JsonConvert.DeserializeObject<List<TModel>>(textAsset.text, new ModelConverter());
			_tree = tree;

			CreateDefaultModel();
			Sort();
			foreach (TModel item in _dataAll)
			{
				AddMenu(item);
			}

		}

		public void SelectCategoryToNewDataParam()
		{
			InitializeParamData();
		}

		public void Create()
		{
			if (!ValidateCreate())
			{
				return;
			}

			_dataAll.Add(_data);
			AddMenu(_data);
			AfterCreate(_data);
			Sort();
			Save();
			CreateDefaultModel();
			_tree.UpdateMenuTree();
		}

		public void Delete(OdinMenuItem item)
		{
			DeleteModel(item);
			DeleteItem(_data);
			Sort();
			Save();
		}

		public void Save()
		{
			StringEnumConverter enumConverter = new StringEnumConverter();
			string json = JsonConvert.SerializeObject(_dataAll, Formatting.Indented, enumConverter);
			var path = string.Format("{0}/{1}.{2}", _path, GetKey(), "json");
			File.WriteAllText(path, json);
			AssetDatabase.Refresh();
		}

		public void CleanUp()
		{
			_dataAll.Clear();
		}

		public virtual void LoadGGSheetData()
		{
		}

		private string GetKey()
		{
			// Debug.Log("type: "+_type);
			return $"collection_{_type.ToString()}".PascalToSnake();
		}

		private void CreateDefaultModel()
		{
			string name = _type.ToString().PascalToSpace();
			OdinMenuItem item = _tree.GetMenuItem(name);

			_data = (TModel)Activator.CreateInstance(typeof(TModel));
			if (item == null)
			{
				_tree.Add(name, _data);
			}
			else
			{
				item.Value = _data;
			}
		}

		private void DeleteModel(OdinMenuItem item)
		{
			string name = _type.ToString().PascalToSpace();
			OdinMenuItem itemParent = _tree.GetMenuItem(name);
			itemParent.ChildMenuItems.Remove(item);
			item.Deselect();
			itemParent.Select();
		}
	}

	public class FactoryEditorModule : Factory<CollectionType, IEditorModule>
	{
		public FactoryEditorModule()
		{

		}
	}
}