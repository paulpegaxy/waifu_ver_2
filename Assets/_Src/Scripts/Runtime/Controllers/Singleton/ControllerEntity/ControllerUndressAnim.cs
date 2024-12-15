using System;
using System.Collections;
using System.Collections.Generic;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

public class ControllerUndressAnim : MonoBehaviour
{
    [SerializeField] private Animation anim;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void PlayAnim()
    {
        anim.Play();
    }

    public void TriggerTransitionFade()
    {
        this.PostEvent(TypeGameEvent.UndressTransitionFade);
    }
}
