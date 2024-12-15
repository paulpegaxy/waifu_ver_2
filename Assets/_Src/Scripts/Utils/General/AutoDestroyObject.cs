using System.Collections;
using System.Collections.Generic;
using Game.Runtime;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
   [SerializeField] private float time;
   
   public void OnEnable()
   {
      this.StartDelayMethod(time, () =>
      {
         ControllerSpawner.Instance.Return(gameObject);
      });
   }
}
