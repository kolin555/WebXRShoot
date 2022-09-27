using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;


namespace ShootTower
{
    public class ControllerSystem : MonoBehaviour
    {
        private WebXRController controller;

        private void Awake()
        {
            controller = gameObject.GetComponent<WebXRController>();
        }

        
        
        private void Update()
        {

            // Get button A(0 or 1), or Axis Trigger/Grip (0 to 1), the larger between them all, by that order
            float normalizedTime = controller.GetButton(WebXRController.ButtonTypes.ButtonA) ? 1 :
                Mathf.Max(controller.GetAxis(WebXRController.AxisTypes.Trigger),
                    controller.GetAxis(WebXRController.AxisTypes.Grip));

            /*if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger)
                || controller.GetButtonDown(WebXRController.ButtonTypes.Grip)
                || controller.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
            {
                
            }

            if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger)
                || controller.GetButtonUp(WebXRController.ButtonTypes.Grip)
                || controller.GetButtonUp(WebXRController.ButtonTypes.ButtonA))
            {
                
            }*/

            if(controller==null)
                return;
            if(MsgSystem.instance==null)
                return;
            if (controller.GetButton(WebXRController.ButtonTypes.ButtonA))
            {
                MsgSystem.instance.SendMsg(MsgSystem.vr_button_a_down,null);
            }

            
            
            
        }
        
        
        
    }

}
