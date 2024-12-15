using UnityEngine;

namespace Game.Runtime
{
    public class ServiceGirlSfx : IServiceGirlSfx
    {
        public void CheckGirlSfx(int tapCount)
        {
            var tapCheck = tapCount % GameConsts.MAX_LEVEL_PER_CHAR;
            if (tapCheck == 0)
            {
                bool isUseSfx = FactoryStorage.Get<StorageUserInfo>().Get().isActiveSfwMode;
                if (isUseSfx)
                    return;
                
                if (ValidateQueueAudio())
                {
                    ControllerAudio.Instance.PlayGirlAudio();
                    return;
                }

                ReFillQueueAudioGirl();
                ControllerAudio.Instance.PlayGirlAudio();
            }
        }

        private bool ValidateQueueAudio()
        {
            if (ControllerAudio.Instance.CountAudioGirl <= 0)
            {
                return false;
            }

            return true;
        }

        private void ReFillQueueAudioGirl()
        {
            var listAudioGirl = AnR.GetAllAssetsByLabel<AudioClip>("audiogirl");
            GameUtils.ShuffleList(ref listAudioGirl);
            ControllerAudio.Instance.ProcessQueueAudioGirl(listAudioGirl);
        }
    }
}