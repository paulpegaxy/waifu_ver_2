using System.Threading.Tasks;
using UnityEngine;

public abstract class AssetLoader : IVfxAssetLoader
{
    public abstract Task<Sprite> LoadSprite(string path);
    public abstract Task<GameObject> LoadVfx(string path);
}
