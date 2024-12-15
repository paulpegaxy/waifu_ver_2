
using BreakInfinity;
using Game.UI;
using Template.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Runtime
{
    public class ServiceGirlVfx : IServiceGirlVfx
    {
        private ParticleSystem _effActivePoint;
        private ParticleSystem[] _arrEffContainScPoint;

        public void Init(ParticleSystem eff, ParticleSystem[] effContainScPoint)
        {
            _effActivePoint = eff;
            _arrEffContainScPoint = effContainScPoint;
        }

        public void ActiveTapGeneral(TypeGirlReact type, Vector3 position, Transform parent)
        {
            ActiveTapEffect(type, position, parent);
            ActiveTapFloatingEffect(position, parent);
        }

        private void ActiveScCurrencyEffect(Sprite sprite, bool isSpecial, BigDouble valueAdditive, Vector3 startPos)
        {
            int count = int.Parse(valueAdditive.ToString());
            float randomSize = Random.Range(0.4f, 1.1f);
            ControllerUI.Instance.SpawnScDynamic(sprite, isSpecial, startPos, count, randomSize,
                () =>
                {
                    // callback?.Invoke(count);
                    ActivePoint(count);
                });
        }

        private void ActiveTapEffect(TypeGirlReact type, Vector3 pos, Transform parent)
        {
            var storage = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storage.Get();
            var config = DBM.Config.tapEffectConfig;
            if (userInfo.isActiveSfwMode)
            {
                var effect = ControllerSpawner.Instance.Spawn(config.tapSfwEffect.pathAssetName, true, false, parent);
                if (effect == null) return;

                var rectTransform = effect.GetComponent<RectTransform>();
                rectTransform.localPosition = pos;
                effect.SetActive(true);
            }
            else
            {
                if (userInfo.selectedTapEffectId == 0)
                {
                    userInfo.selectedTapEffectId = config.tapDefaultEffect.id;
                    storage.Save();
                }

                var effectConfig = config.GetTapEffect(userInfo.selectedTapEffectId.ToString());

                var effect = ControllerSpawner.Instance.Spawn(effectConfig.pathAssetName, true, false, parent);
                if (effect == null) return;

                var rectTransform = effect.GetComponent<RectTransform>();
                rectTransform.localPosition = pos;
                effect.SetActive(true);

                if (effectConfig.isDefault)
                    effect.GetComponent<TapGirlVfxControl>().PlayAnim(type);
            }
        }

        private void ActiveTapFloatingEffect(Vector3 position, Transform parent)
        {
            var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            var config = DBM.Config.tapEffectConfig;
            var effect =
                ControllerSpawner.Instance.Spawn(AnR.GetKey(AnR.CommonKey.VfxFloatingItem), true, false, parent);
            if (effect == null) return;

            var rectTransform = effect.GetComponent<RectTransform>();
            rectTransform.localPosition = position;
            effect.SetActive(true);
            var item = effect.GetComponent<ItemAnimFloatingText>();

            int id = 0;
            bool isSpecial = false;

            if (userInfo.isActiveSfwMode)
            {
                id = config.tapSfwEffect.resourceId;
            }
            else
            {
                var effectConfig = config.GetTapEffect(userInfo.selectedTapEffectId.ToString());
                id = effectConfig.resourceId;
                if (!effectConfig.isDefault)
                {
                    isSpecial = true;
                }
            }


            var spriteIcon = ControllerSprite.Instance.GetResourceIcon(id);
            for (var i = 0; i < _arrEffContainScPoint.Length; i++)
            {
                _arrEffContainScPoint[i].textureSheetAnimation.SetSprite(0, spriteIcon);
            }

            item.SetData(FactoryApi.Get<ApiGame>().Data.Info.GetFinalPointPerTap(), id);

            ActiveScCurrencyEffect(spriteIcon, isSpecial, FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse,
                effect.transform.position);
        }

        private void ActivePoint(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_effActivePoint == null)
                    break;

                // effActivePoint.time = 0;
                _effActivePoint.Simulate(0.0f, true, true);
                _effActivePoint.Play();
                // await UniTask.Delay(100);
            }
        }
    }
}