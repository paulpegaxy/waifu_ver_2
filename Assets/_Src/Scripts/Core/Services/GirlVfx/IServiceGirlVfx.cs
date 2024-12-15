// Author: ad   -
// Created: 03/11/2024  : : 23:11
// DateUpdate: 03/11/2024

using System;
using BreakInfinity;
using UnityEngine;

namespace Game.Runtime
{
    public interface IServiceGirlVfx
    {
        void Init(ParticleSystem eff,ParticleSystem[] effContainScPoint);
        void ActiveTapGeneral(TypeGirlReact type, Vector3 position, Transform parent);
        // void ActiveTapEffect(TypeGirlReact type,Vector3 pos,Transform parent);
        // void ActiveTapFloatingEffect(TypeGirlReact type, Vector3 position,Transform parent);
        // void ActiveScCurrencyEffect(BigDouble valueAdditive, Vector3 startPos,Action<int> callback);
    }
}