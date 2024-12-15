using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSeeWaifuPicture : MonoBehaviour
{
    [SerializeField] private Image imgPic;

    public void SetData(Sprite spr)
    {
        imgPic.sprite = spr;
    }
}
