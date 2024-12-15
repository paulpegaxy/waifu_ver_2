// Author: ad   -
// Created: 18/10/2024  : : 20:10
// DateUpdate: 18/10/2024

using System.Collections.Generic;
using Game.Model;

namespace Game.Runtime
{
    [Factory(StorageType.Shopping, true)]
    public class StorageShopping : Storage<ModelStorageShoping>
    {
        public StorageShopping()
        {
            _key = GetKey(StorageType.Shopping);
        }

        protected override void InitModel()
        {
            _model = new ModelStorageShoping()
            {
                lastTimeRequestBuyOfferFreerin = 0
            };
        }

        public override void Load()
        {
            base.Load();
            if (_model == null)
            {
                InitModel();
                Save();
            }
        }
    }
}