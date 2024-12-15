using System.Collections;
using System.Collections.Generic;
using Game.Model;
using Game.Runtime;
using Game.UI;
using UnityEngine;

public class ItemNotiMail : ItemNotificationGeneric<List<ModelApiMailData>>
{
    protected override void OnEnabled()
    {
        SetData(FactoryApi.Get<ApiMail>().Data.Mails);
        ModelApiMail.OnChanged += OnMailChanged;
    }

    protected override void OnDisabled()
    {
        ModelApiMail.OnChanged -= OnMailChanged;
    }
    
    private void OnMailChanged(ModelApiMail data)
    {
        SetData(data.Mails);
    }
        
    protected override bool IsValid()
    {
        if (_data == null) return false;
        
        return _data.Exists(x => !x.is_read);
    }
}
