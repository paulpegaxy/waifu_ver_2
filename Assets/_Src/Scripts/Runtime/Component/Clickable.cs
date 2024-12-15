using System.Linq;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Game.Runtime
{
    public class Clickable : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsAllowToClick()) return;

                var camera = CameraManager.Instance.GetCamera(CameraManager.CameraType.MainCamera);
                var ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // var pet = hit.collider.transform.parent.GetComponentInParent<Pet>();
                    // if (pet != null)
                    // {
                    //     Pet.OnClick?.Invoke(pet);
                    // }
                }
            }
        }

        private bool IsAllowToClick()
        {
            var isMainNode = SpecialExtensionUI.GetCurrentNode() == UIId.UIViewName.Main.ToString();
            var isNoPopup = UIPopup.visiblePopups.Count() == 0;

            return isMainNode && isNoPopup;
        }
    }
}