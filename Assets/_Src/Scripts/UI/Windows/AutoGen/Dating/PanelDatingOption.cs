using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDatingOption : MonoBehaviour
{
    [SerializeField] private Transform posContainOption;

    public void SetData(List<DataItemMessageOption> listMessage)
    {
        if (listMessage.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        posContainOption.FillData<DataItemMessageOption, DatingItemOption>(listMessage, (data, view, index) =>
        {
            view.SetData(data);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
