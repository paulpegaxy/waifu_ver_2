using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

namespace Game.GameTools
{
	public class ClassGeneratorWindow : OdinEditorWindow
	{
		[MenuItem("Assets/Game Tools/Generator #&c", false, 100)]
		static void OpenWindow()
		{
			GetWindow<ClassGeneratorWindow>().Show();
		}

		[ShowInInspector]
		public GeneratorScroller ScrollerGenerator;
	}
}