using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Game.Core;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class LoadingWindow : UIWindow
	{
		protected override void OnEnabled()
		{
			Load().Forget();
		}

		protected override void OnDisabled()
		{

		}

		private async UniTask Load()
		{
			// var type = EventManager.GetData<GameEventLoadType>(TypeGameEvent.Load);

			var type = this.GetEventData<TypeGameEvent, GameEventLoadType>(TypeGameEvent.Load);
			
			switch (type)
			{
				case GameEventLoadType.LoadToGame:
					await LoadToPuzzle();
					break;

				case GameEventLoadType.LoadToMain:
					await LoadToMain();
					break;
			}

			Signal.Send(StreamId.Game.Loaded);
		}

		private async UniTask LoadToPuzzle()
		{
			// await AnR.LoadAddressableByLabels<GameObject>(new List<string> { "common", "tile" }, false);
			await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

			SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
			CameraManager.Instance.SetCamera(CameraManager.CameraType.GameCamera);
		}

		private async UniTask LoadToMain()
		{
			await SceneManager.UnloadSceneAsync("Game");
			ControllerSpawner.Instance.ReleaseAll();
			AnR.ReleaseAll();

			CameraManager.Instance.RemoveCamera(CameraManager.CameraType.GameCamera);
			CameraManager.Instance.SetCamera(CameraManager.CameraType.MainCamera);
		}
	}
}