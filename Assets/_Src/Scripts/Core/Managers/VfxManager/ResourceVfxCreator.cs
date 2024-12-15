using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Template.VfxManager
{
    public class ResourceVfxCreator : IVfxHandlerCreator
    {
        private const string HANDLER_LOAD_PATH = "VfxHandlers/";

        private Dictionary<string, GameObject> caches;

        public ResourceVfxCreator()
        {
            caches = new();
        }

        public IVfxHandler CreateVfxHandler(string handlerName)
        {
            GameObject handlerObject;
            if (caches.ContainsKey(handlerName))
            {
                handlerObject = caches[handlerName];
            }
            else
            {
                handlerObject = Resources.Load<GameObject>(HANDLER_LOAD_PATH + handlerName);
            }
            if (handlerObject == null)
            {
                throw new Exception($"Cannot find any handler with name <{handlerName}>");
            }

            return UnityEngine.Object.Instantiate(handlerObject).GetComponent<IVfxHandler>();
        }
    }
}