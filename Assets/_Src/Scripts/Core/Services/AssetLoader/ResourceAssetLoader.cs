using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class ResourceAssetLoader : AssetLoader
{
    public override async Task<Sprite> LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        return await Resources.LoadAsync<Sprite>(path) as Sprite;
    }

    public override async Task<GameObject> LoadVfx(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        return await Resources.LoadAsync<GameObject>(path) as GameObject;
    }
}
