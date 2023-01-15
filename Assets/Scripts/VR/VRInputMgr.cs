//
// Expose VR hand positions and button events 
//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputMgr : MonoBehaviour
{
   public enum Hand
   {
      Left,
      Right
   }

   public static Transform GetHeadTrans()
   {
      if (VRCamRig.I.GetIs2DMode())
         return VRCamRig.I.transform;
      else
         return VRCamRig.I.GetOVRCamRig().centerEyeAnchor;
   }

   public static Transform GetHandTrans(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
         return (h == Hand.Left) ? VRCamRig.I.LeftHand2D : VRCamRig.I.RightHand2D;
      else
         return (h == Hand.Left) ? VRCamRig.I.LeftHandVR : VRCamRig.I.RightHandVR;
   }

   public static Vector3 GetHandPos(Hand h)
   {
      if(VRCamRig.I.GetIs2DMode())
      {
         Transform handTrans = (h == Hand.Left) ? VRCamRig.I.LeftHand2D : VRCamRig.I.RightHand2D;
         return handTrans ? handTrans.position : Vector3.zero;
      }
      else
      {
         Vector3 localPos = (OVRInput.GetLocalControllerPosition((h == Hand.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch));
         return VRCamRig.I.transform.TransformPoint(localPos);
      }
   }

   public static Quaternion GetHandRot(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         Transform handTrans = (h == Hand.Left) ? VRCamRig.I.LeftHand2D : VRCamRig.I.RightHand2D;
         return handTrans ? handTrans.rotation : Quaternion.identity;
      }
      else
      {
         Quaternion localRot = (OVRInput.GetLocalControllerRotation((h == Hand.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch));
         return VRCamRig.I.transform.rotation * localRot;
      }
   }

      public static float GetStickHorizontal(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         if (Input.GetKey(KeyCode.RightArrow))
            return 1.0f;
         else if (Input.GetKey(KeyCode.LeftArrow))
            return -1.0f;
         else
            return 0.0f;
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x : OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
      }
   }

   public static float GetStickVertical(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         if (Input.GetKey(KeyCode.UpArrow))
            return 1.0f;
         else if (Input.GetKey(KeyCode.DownArrow))
            return -1.0f;
         else
           return 0.0f;
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y : OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
      }
   }

   public static bool GetBottomFaceButtonDown(Hand h)
   {
      if(VRCamRig.I.GetIs2DMode())
      {
         if (h == Hand.Left)
            return Input.GetKey(KeyCode.Z);  //for scaling down
         else
            return Input.GetKey(KeyCode.Z); //dont have any use for this yet on right controller...
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.GetDown(OVRInput.RawButton.X) : OVRInput.GetDown(OVRInput.RawButton.A);
      }
   }

   public static bool GetBottomFaceButtonHeld(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         if (h == Hand.Left)
            return Input.GetKey(KeyCode.Z);  //for scaling down
         else
            return Input.GetKey(KeyCode.Z); //dont have any use for this yet on right controller...
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.Get(OVRInput.RawButton.X) : OVRInput.Get(OVRInput.RawButton.A);
      }
   }

   public static bool GetTopFaceButtonDown(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         if (h == Hand.Left)
            return Input.GetKeyDown(KeyCode.A); //for scaling up
         else
            return Input.GetKeyDown(KeyCode.F); //for cycling thru "frames"
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.GetDown(OVRInput.RawButton.Y) : OVRInput.GetDown(OVRInput.RawButton.B);
      }
   }

   public static bool GetTopFaceButtonHeld(Hand h)
   {
      if (VRCamRig.I.GetIs2DMode())
      {
         if (h == Hand.Left)
            return Input.GetKey(KeyCode.A); //for scaling up
         else
            return Input.GetKey(KeyCode.F); //for cycling thru "frames"
      }
      else
      {
         return (h == Hand.Left) ? OVRInput.Get(OVRInput.RawButton.Y) : OVRInput.Get(OVRInput.RawButton.B);
      }
   }

   public static bool GetAppButtonDown()
   {
      return OVRInput.GetDown(OVRInput.Button.Start);
   }

   public static bool GetTriggerDown(Hand h)
   {
      return (h == Hand.Left) ? OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) : OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
   }

   public static bool GetTriggerHeld(Hand h)
   {
      return (h == Hand.Left) ? OVRInput.Get(OVRInput.RawButton.LIndexTrigger) : OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
   }

   public static bool GetStickClickDown(Hand hand)
   {
      return (hand == Hand.Left) ? OVRInput.GetDown(OVRInput.RawButton.LThumbstick) : OVRInput.GetDown(OVRInput.RawButton.RThumbstick);
   }

}
