//
// Access to vr camera rig components
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCamRig : MonoBehaviour
{
   public bool Force2DMode = false;
   public GameObject CamRig2D;
   public GameObject CamRigVR;

   [Space(10)]

   public Transform LeftHand2D;
   public Transform RightHand2D;

   [Space(10)]

   public Transform LeftHandVR;
   public Transform RightHandVR;

   OVRCameraRig _ovrRig = null;
   OVRManager _ovrManager = null;
   bool _hasRegisteredRecenterEvent = false;

   public OVRCameraRig GetOVRCamRig() { return _ovrRig; }
   public OVRManager GetOVRManager() { return _ovrManager; }

   public bool GetIs2DMode() { return _is2DMode; }

   public static VRCamRig I { get; private set; }

   bool _is2DMode = false;

   void Awake()
   {
      I = this;
      _ovrRig = GetComponent<OVRCameraRig>();
      if (!_ovrRig)
         _ovrRig = GetComponentInChildren<OVRCameraRig>();
      _ovrManager = GetComponent<OVRManager>();
      if (!_ovrManager)
         _ovrManager = GetComponentInChildren<OVRManager>();
   }

   void _Activate2DMode(bool b)
   {
      _is2DMode = b;

      if (CamRig2D)
         CamRig2D.SetActive(_is2DMode);
      if (CamRigVR)
         CamRigVR.SetActive(!_is2DMode);
   }


   void Update()
   {
      //activate 2d mode if we dont see an hmd
      if (Time.time > 1.0f)
      {
         _Activate2DMode((OVRManager.isHmdPresent && !Force2DMode) ? false : true);
      }

   }

}
