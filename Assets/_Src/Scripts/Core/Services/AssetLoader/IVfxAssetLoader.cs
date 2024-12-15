using System.Threading.Tasks;
using UnityEngine;

public interface IVfxAssetLoader
{
    Task<GameObject> LoadVfx(string vfxPath);
}