using System.Threading.Tasks;
using Doozy.Runtime.Signals;
using Game.Model;
using Game.Runtime;
using Template.Utils;
using Template.Gameplay;
using UnityEngine;

public class GameController : MonoBehaviour
{


    private ModelGameConfigParamGameplay ConfigGamePlay =>
        FactoryCollection.Get<CollectionGameConfig>().Get<ModelGameConfigParamGameplay>(TypeGameConfig.GamePlayConfig);

    private async void Awake()
    {
        // await GameDependencies.Initialize();
    }

    private void Start()
    {
        StartGame();
    }

    public async void StartGame()
    {
        InitializeGameState();
        // Signal.Send(StreamId.Window.ActionPhase);
        await InitializeStartGame();
        BeginGame();
    }

    private void InitializeGameState()
    {

    }


    private async Task InitializeStartGame()
    {

        ServiceLocator.GetService<EntitiesCentralDataHandler>().Reset();
    }

    private void Update()
    {
    }

    private void BeginGame()
    {
    }
}
