using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Runtime;
using Template.Service;
using Template.VfxManager;

public class GameDependencies
{
    private static bool isInitialized;

    public static async UniTask Initialize()
    {
        if (isInitialized) { return; }

        ServiceLocator.Register<AssetLoader>(new ResourceAssetLoader());
        ServiceLocator.Register<ServiceGameContent>(new FactoryContentManager());

        ServiceLocator.Register<IEntityManager>(new EntityManager());

        ServiceLocator.Register(new VfxController(ServiceLocator.GetService<AssetLoader>(), new ResourceVfxCreator(), 
            ServiceLocator.GetService<ServiceGameContent>()));

        ServiceLocator.Register<IServiceValidate>(new ServiceValidate());
        ServiceLocator.Register<IServiceSyncData>(new ServiceSyncData());
        ServiceLocator.Register<IServiceGirlSfx>(new ServiceGirlSfx());
        ServiceLocator.Register<IServiceTracking>(new ServiceTracking());
        ServiceLocator.Register<IServiceGirlVfx>(new ServiceGirlVfx());

        isInitialized = true;
    }
}