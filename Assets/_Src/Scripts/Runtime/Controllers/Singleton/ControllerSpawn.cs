using System.Collections.Generic;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Game.Core;
using Game.UI;

namespace Game.Runtime
{
    public class ControllerSpawner : Singleton<ControllerSpawner>
    {
        [SerializeField] private GameObject prefabGirlEntity;
        
        private readonly List<GameObject> _pool = new();

        public GameObject Spawn(string key, bool isPool = true, bool isAutoDisable = false, Transform parent = null)
        {
            GameObject instance = GetPool(key);
            if (instance == null)
            {
                GameObject go = AnR.Get<GameObject>(key);
                if (go != null)
                {
                    instance = Instantiate(go, parent ?? transform);
                    instance.name = key;

                    if (isPool)
                    {
                        _pool.Add(instance);
                    }

                    if (isAutoDisable)
                    {
                        instance.AddComponent<AutoDisableByParticleSystem>();
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError($"[ControllerSpawner] Spawn: {key} is not found");
                }
            }
            else
                instance.transform.SetParent(parent);
            
            
            instance.transform.localScale=Vector3.one;
            
            return instance;
        }
        
        public async UniTask<GameObject> SpawnAsync(string key, bool isPool = true, bool isAutoDisable = false, Transform parent = null)
        {
            var instance = GetPool(key);
            if (instance == null)
            {
                var go = AnR.Get<GameObject>(key);
                if (go == null) go = await AnR.LoadAddressable<GameObject>(key);

                instance = Instantiate(go, parent != null ? parent : transform);
                instance.name = key;

                if (isPool)
                {
                    _pool.Add(instance);
                }

                if (isAutoDisable)
                {
                    instance.AddComponent<AutoDisableByParticleSystem>();
                }
            }

            return instance;
        }

        public void PrePool(string key, int count, Transform parent = null)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn(key,parent: parent);
            }
        }

        GameObject GetPool(string key)
        {
            foreach (GameObject item in _pool)
            {
                if (item==null)
                    continue;
                
                if ((item.name == key || item.name == key) && !item.activeSelf)
                {
                    return item;
                }
            }
            return null;
        }

        public void Return(GameObject instance)
        {
            instance.SetActive(false);
        }

        public void ReleaseAll()
        {
            foreach (var item in _pool)
            {
                DestroyImmediate(item, true);
            }
            _pool.Clear();
        }

        public Entity SpawnGirl(int girlID,Transform parent)
        {
            var girlEntity = Instantiate(prefabGirlEntity).GetComponent<Entity>();
            // girlEntity.Init(girlID);
            girlEntity.transform.SetParent(parent);
            girlEntity.transform.localScale = Vector3.one;
            girlEntity.transform.localPosition = Vector3.zero;
            girlEntity.gameObject.SetActive(true);
            return girlEntity;
        }
        
        public async UniTask<Texture> SpawnBgSpecialGirl(string key)
        {
            var bg = AnR.Get<Texture>(key);
            if (bg == null)
                bg = await AnR.LoadAddressable<Texture>(key);
            return bg;
        }
        
        public async UniTask<Texture> SpawnBackground(string key)
        {
            var bg = AnR.Get<Texture>(key);
            if (bg == null)
                bg = await AnR.LoadAddressable<Texture>(key);

            return bg;
        }


        public void SpawnEffectTap(TypeGirlReact type,Vector3 pos,Transform parent)
        {
            var effect = Instance.Spawn(AnR.GetKey(AnR.CommonKey.VfxTapGirl), true, false, parent);
            if (effect == null) return;
        
            var rectTransform = effect.GetComponent<RectTransform>();
            rectTransform.localPosition = pos;
            effect.SetActive(true);
            effect.GetComponent<TapGirlVfxControl>().PlayAnim(type);
        }
    }
}