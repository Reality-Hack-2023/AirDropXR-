using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPhoneCalibrator : MonoBehaviour
{
   public bool EnableCalibratorAtStart = false;
   public GameObject MoveWithCalibrationController;
   public GameObject CalibrationPhoneVisual; //expected to be a child of MoveWithCalibrationController


   public AnimationCurve FlickAnimEase = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));

   void TriggerFlickAnim()
   {
      StartCoroutine(_DoFlickAnim());
   }

   IEnumerator _DoFlickAnim()
   {
      const float kAnimTime = .5f; //how long to travel
      const float kFlickDistance = 100; //how far to travel

      float startTime = Time.time;
      float endTime = startTime + kAnimTime;
      float curTime = Time.time;

      while(curTime <= endTime)
      {
         curTime = Time.time;
         float u = Mathf.InverseLerp(startTime, endTime, curTime);
         float easedU = FlickAnimEase.Evaluate(u);

         float curTravelAmount = easedU * kFlickDistance;

         //TODO: apply to image here!!

         yield return new WaitForEndOfFrame();
      }

      //TODO: trigger next screen here!
   }


   bool _isCalibrating = false;

   void Start()
   {
      if (CalibrationPhoneVisual)
         CalibrationPhoneVisual.SetActive(false);

      if (EnableCalibratorAtStart)
         BeginCalibration();
   }

   public bool GetIsCalibrating()
   {
      return _isCalibrating;
   }

   public void BeginCalibration()
   {
      if (GetIsCalibrating())
         return;

      _isCalibrating = true;

      if (CalibrationPhoneVisual)
         CalibrationPhoneVisual.SetActive(true);
   }

   void _CompleteCalibration()
   {
      if (!GetIsCalibrating())
         return;

      //compute calibration offset
      //Vector3 refPos = VRInputMgr.GetHandPos(VRInputMgr.Hand.Left);
      //Quaternion refRot = VRInputMgr.GetHandRot(VRInputMgr.Hand.Left);
      //Vector3 posOffset = (refPos - CalibrationPhoneVisual.transform.position)

      //we know the that phone mgr's visual is parented to left controller, so in theory we can just set its position to the calibrated phone loc
      //(then it will get the appropriate local offset from the left controller it is still parented to)
      VRPhoneMgr.I.PhoneVisualParent.position = CalibrationPhoneVisual.transform.position;
      VRPhoneMgr.I.PhoneVisualParent.rotation = CalibrationPhoneVisual.transform.rotation;

      if (CalibrationPhoneVisual)
         CalibrationPhoneVisual.SetActive(false);

      _isCalibrating = false;
   }

   void LateUpdate()
   {
      if(GetIsCalibrating())
      {
         //move calibration visual with right hand
         if(CalibrationPhoneVisual)
         {
            MoveWithCalibrationController.transform.position = VRInputMgr.GetHandPos(VRInputMgr.Hand.Right);
            MoveWithCalibrationController.transform.rotation = VRInputMgr.GetHandRot(VRInputMgr.Hand.Right);
         }

         //when trigger goes down, complete calibration
         if(VRInputMgr.GetTriggerDown(VRInputMgr.Hand.Right))
         {
            _CompleteCalibration();
         }
      }
      else
      {
         //begin calibration by clicking in stick on right controller
         if (VRInputMgr.GetStickClickDown(VRInputMgr.Hand.Right) || VRInputMgr.GetTopFaceButtonDown(VRInputMgr.Hand.Right) || VRInputMgr.GetBottomFaceButtonDown(VRInputMgr.Hand.Right))
            BeginCalibration();
      }
   }
}
