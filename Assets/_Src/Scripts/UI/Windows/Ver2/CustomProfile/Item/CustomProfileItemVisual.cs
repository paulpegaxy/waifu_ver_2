using System.Collections;
using System.Collections.Generic;
using _Src.Scripts.Data.DBM.Configs;
using UnityEngine;
using UnityEngine.UI;

public class CustomProfileItemVisual : MonoBehaviour
{
    [SerializeField] private Image imgAvatar;

    public void SetData(DataItemCustomProfileVisual data)
    {
        imgAvatar.sprite = data.avatar;
    }

    public void SetData(int index)
    {
        imgAvatar.LoadSpriteAsync("ava_" + index);
    }
}
