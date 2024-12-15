// Author: ad   -
// Created: 22/09/2024  : : 02:09
// DateUpdate: 22/09/2024

using System;
using Game.Extensions;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public abstract class AItemCheckStartGame : MonoBehaviour
    {
        protected bool IsGameStart;
        
        private bool _isInitialized = false;

        protected virtual void Awake()
        {
            this.RegisterEvent(TypeGameEvent.GameStart, OnGameStart);
        }

        protected virtual void OnDestroy()
        {
            this.RemoveEvent(TypeGameEvent.GameStart, OnGameStart);
        }

        private void OnEnable()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                return;
            }
            OnEnabled();
        }

        private void OnDisable()
        {
            OnDisabled();
        }

        protected virtual void OnEnabled()
        {

        }

        protected virtual void OnDisabled()
        {

        }

        private void OnGameStart(object data)
        {
            OnInit();
            IsGameStart = true;
        }

        protected abstract void OnInit();
    }
}