// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using Spine.Unity;
using UnityEngine;

public abstract class ASpineView : MonoBehaviour,ISpineView
{
        
    public float Speed => _speed;

    private float _speed;
        
    public virtual void Init(float timeScale)
    {
        _speed = timeScale;
    }
    
  
}