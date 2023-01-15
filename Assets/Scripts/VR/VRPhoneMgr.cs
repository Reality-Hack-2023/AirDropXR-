//
// main gameplay script for handling virtual phone in VR
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPhoneMgr : MonoBehaviour
{
   [Header("Config")]
   public float DistToSpawnInFrontOfPhone = .2f;
   public Transform PhoneVisualParent;
   [Tooltip("The top most center of the phone")]
   public Transform PhoneTop;
   public LineRenderer TetherRnd;

   [Space(10)]

   public Transform PhoneStateUIParent = null;
   public Transform PhoneSwipeHintUIParent = null;
   public Transform PhoneStateUIChaseTarget = null;
   public Vector3 PhoneStateUIHeadOffset = new Vector3(0.0f, -.1f, 1.0f);
   public GameObject PhoneStateMode_Move = null;
   public GameObject PhoneStateMode_Rotate = null;
   public GameObject PhoneStateMode_Size = null;

   [Header("Testing")]
   public int TestSpawnImageID = 0;
   public KeyCode TestSpawnImageCheat = KeyCode.I;
   public int TestSpawnVideoID = 0;
   public KeyCode TestSpawnVideoCheat = KeyCode.V;
   public int TestSpawVideo360ID = 0;
   public KeyCode TestSpawnVideo360Cheat = KeyCode.D;
   [Space(10)]
   public bool EnableMoveForwardWithStick = true;
   public float MoveForwardWithStickSpeedMult = 1.0f;
   public bool EnableRotateWithStick = true;
   public float RotateWithStickSpeedMult = 1.0f;
   public bool EnableSizeUpWithTopFaceButton = true;
   public bool EnableSizeDownWithBottomFaceButton = true;
   public float SizeUpDownWithFaceButtonMult = 1.0f;
   [Space(10)]
   public bool EnableCycleImageFramesWithButton = true;

   public enum UIManipMode : uint
   {
      Move = 0,
      Rotate,
      Size
   }

   float _curForwardOffset = 0.0f;
   float _curRotationOffset = 0.0f;
   float _curSizeOffset = 0.0f;
   UIManipMode _curUIManipMode = UIManipMode.Move;

   VRContentDisplay _curContent = null;

   public bool GetIsManipulating() { return _curContent != null; }
   public VRContentDisplay GetManipulationContent() { return _curContent; }

   public void SetUIManipMode(UIManipMode m)
   {
      _curUIManipMode = m;
   }

   public void SetContentForwardOffset(float f)
   {
      _curForwardOffset = f;
      //Debug.Log("Content forward offset changed to: " + _curForwardOffset);

      _RefreshForwardOffset();
   }

   public float GetContentForwardOffset()
   {
      return _curForwardOffset;
   }

   public void SetContentRotationOffset(float f)
   {
      _curRotationOffset = f;
      // Debug.Log("Content rotation offset changed to: " + _curRotationOffset);

      _RefreshRotationOffset();
   }

   public float GetContentRotationOffset()
   {
      return _curRotationOffset;
   }

   public void SetContentSizeOffset(float f)
   {
      _curSizeOffset = f;
      // Debug.Log("Content size offset changed to: " + _curSizeOffset);

      _RefreshSizeOffset();
   }

   public float GetContentSizeOffset()
   {
      return _curSizeOffset;
   }

   public static VRPhoneMgr I { get; private set; }
   void Awake()
   {
      I = this;
   }

   void Start()
   {
      if (TetherRnd)
         TetherRnd.gameObject.SetActive(false);

      if (ContentMgr.I)
      {
         ContentMgr.I.OnAllContentDeleting.AddListener(_OnAllContentDeleting);
      }

      if (PhoneVisualParent)
      {
         PhoneVisualParent.transform.parent = VRInputMgr.GetHandTrans(VRInputMgr.Hand.Left);
         PhoneVisualParent.localPosition = Vector3.zero;
         PhoneVisualParent.localRotation = Quaternion.identity;
      }
   }

   void _OnAllContentDeleting()
   {
      StopManipulatingContent();
   }

   public void StartManipulatingContent(VRContentDisplay content)
   {
      if (content)
      {
         //gotta stop manipulating whatever we were manipulating before
         if (_curContent)
            StopManipulatingContent();


         _curContent = content;

         //position it in front of the phone (use left hand)
         Vector3 spawnPos = PhoneTop.position + PhoneTop.forward * DistToSpawnInFrontOfPhone;
         content.transform.position = spawnPos;
         _RefreshContentRotation();

         SetContentForwardOffset(0.0f);
         SetContentRotationOffset(0.0f);
         SetContentSizeOffset(0.0f);

         if (TetherRnd)
         {
            TetherRnd.gameObject.SetActive(true);
            TetherRnd.useWorldSpace = true;
            TetherRnd.positionCount = 2;
            _RefreshTetherRnd();
         }
      }
   }

   //stop manipulating content, leaving it where it is
   public void StopManipulatingContent()
   {
      if (_curContent)
      {
         _curContent = null;

         if (TetherRnd)
            TetherRnd.gameObject.SetActive(false);
      }
   }

   void _RefreshTetherRnd()
   {
      if (!GetIsManipulating() || !TetherRnd)
         return;

      TetherRnd.positionCount = 2;
      TetherRnd.SetPosition(0, PhoneTop.position);
      TetherRnd.SetPosition(1, _curContent.GetTetherAttach().position);
   }

   void _RefreshForwardOffset()
   {
      if (!_curContent)
         return;

      Vector3 startPos = PhoneTop.position + PhoneTop.forward * DistToSpawnInFrontOfPhone;

      Vector3 finalPos = startPos + (PhoneTop.forward * _curForwardOffset);

      _curContent.transform.position = finalPos;
   }

   void _RefreshRotationOffset()
   {
      if (!_curContent)
         return;

      Transform rotReceiver = _curContent.RotationOffsetReciever ? _curContent.RotationOffsetReciever : _curContent.transform;
      Vector3 curEuler = rotReceiver.localEulerAngles;
      curEuler.y = _curRotationOffset;
      rotReceiver.localEulerAngles = curEuler;
   }

   void _RefreshSizeOffset()
   {
      if (!_curContent)
         return;


      if(_curContent.SizeOffsetReceiver)
      {
         _curContent.SizeOffsetReceiver.localScale = new Vector3(1.0f + _curSizeOffset, 1.0f + _curSizeOffset, 1.0f + _curSizeOffset);
      }
   }

   void _SpawnImage(int contentID)
   {
      Debug.Log("I pressed");
      VRContentDisplay content =  ContentMgr.I.SpawnContent(VRContentDisplay.ContentTypes.Image, contentID);
      StartManipulatingContent(content);
   }  

   void _SpawnVideo(int contentID)
   {
      Debug.Log("V pressed");
      VRContentDisplay content = ContentMgr.I.SpawnContent(VRContentDisplay.ContentTypes.Video, contentID);
      StartManipulatingContent(content);
   }

   void _SpawnVideo360(int contentID)
   {
      Debug.Log("D pressed");
      VRContentDisplay content = ContentMgr.I.SpawnContent(VRContentDisplay.ContentTypes.Video360, contentID);
      StartManipulatingContent(content);
   }

   void _UpdatePhoneLocation()
   {
      //move to left hand
      /*Vector3 handPos = VRInputMgr.GetHandPos(VRInputMgr.Hand.Left);
      Quaternion handRot = VRInputMgr.GetHandRot(VRInputMgr.Hand.Left);
      PhoneVisualParent.position = handPos;
      PhoneVisualParent.rotation = handRot;*/
      
      //NOTE: now parenting phone visual to controller transform so calibrator can do its magic
      //parent to left hand (so calibrator can do its magic later)
      if (PhoneVisualParent && VRCamRig.I.GetIs2DMode())
      {
         PhoneVisualParent.transform.parent = VRInputMgr.GetHandTrans(VRInputMgr.Hand.Left);
         PhoneVisualParent.localPosition = Vector3.zero;
         PhoneVisualParent.localRotation = Quaternion.identity;
      }


      //keep content rotating with phone while we are manipulating itg
      _RefreshContentRotation();
   }

   void _RefreshContentRotation()
   {
      if (_curContent)
      {
         _curContent.transform.rotation = PhoneVisualParent.rotation;
         //rotate so parallel to ground
         _curContent.transform.up = Vector3.up;
      }
   }

   void _RefreshModeUI()
   {
      if (PhoneStateUIParent)
         PhoneStateUIParent.gameObject.SetActive(GetIsManipulating());
      if (PhoneSwipeHintUIParent)
         PhoneSwipeHintUIParent.gameObject.SetActive(GetIsManipulating());

      if (PhoneStateMode_Move)
         PhoneStateMode_Move.SetActive(_curUIManipMode == UIManipMode.Move);
      if (PhoneStateMode_Rotate)
         PhoneStateMode_Rotate.SetActive(_curUIManipMode == UIManipMode.Rotate);
      if (PhoneStateMode_Size)
         PhoneStateMode_Size.SetActive(_curUIManipMode == UIManipMode.Size);

      if (!PhoneStateUIChaseTarget)
         return;

      Vector3 headPos = VRInputMgr.GetHeadTrans().position;
      Vector3 headForward = VRInputMgr.GetHeadTrans().forward;
      headForward.y = 0.0f;
      headForward.Normalize();
      Vector3 headSideDir = Vector3.Cross(headForward, Vector3.up);

      Vector3 chaseTargetPos = headPos;
      chaseTargetPos.y += PhoneStateUIHeadOffset.y;
      chaseTargetPos += PhoneStateUIHeadOffset.x*headSideDir;
      chaseTargetPos += PhoneStateUIHeadOffset.z * headForward;

      PhoneStateUIChaseTarget.position = chaseTargetPos;

      //rotate towards head
      Vector3 toPlayer = (chaseTargetPos - headPos).normalized;
      PhoneStateUIChaseTarget.rotation = Quaternion.LookRotation(toPlayer);
   }

   void Update()
   {
      _UpdatePhoneLocation();
      _RefreshTetherRnd();
      _RefreshModeUI();


      //only show phone model when testing in editor
      bool showPhoneModel = Application.isEditor;
      if (PhoneVisualParent)
         PhoneVisualParent.gameObject.SetActive(showPhoneModel);

      //cheat to test spawning image content
      if (Input.GetKeyDown(TestSpawnImageCheat))
         _SpawnImage(TestSpawnImageID);
      //cheat to test spawning video content
      if (Input.GetKeyDown(TestSpawnVideoCheat))
         _SpawnVideo(TestSpawnVideoID);
      //cheat to test spawning 360 video content
      if (Input.GetKeyDown(TestSpawnVideo360Cheat))
         _SpawnVideo360(TestSpawVideo360ID);
      //cheat to move content forward and backward with stick on controller
      if (EnableMoveForwardWithStick)
      {
         float vertStick = VRInputMgr.GetStickVertical(VRInputMgr.Hand.Left);
         float offsetAmt = (vertStick * MoveForwardWithStickSpeedMult * Time.deltaTime);
         float finalOffset = Mathf.Max(0.0f, GetContentForwardOffset() + offsetAmt);

         SetContentForwardOffset(finalOffset);
      }
      //cheat to rotate content with stick
      if (EnableMoveForwardWithStick)
      {
         float horizStick = VRInputMgr.GetStickHorizontal(VRInputMgr.Hand.Left);
         float offsetAmt = (horizStick * RotateWithStickSpeedMult * Time.deltaTime);
         float finalOffset = GetContentRotationOffset() + offsetAmt;

         SetContentRotationOffset(finalOffset);
      }
      //cheat to size up when you press top face button
      if(EnableSizeUpWithTopFaceButton)
      {
         if(VRInputMgr.GetTopFaceButtonHeld(VRInputMgr.Hand.Left))
         {
            float offsetAmt = SizeUpDownWithFaceButtonMult * Time.deltaTime;
            float finalOffset = GetContentSizeOffset() + offsetAmt;
            SetContentSizeOffset(finalOffset);
         }
      }
      //cheat to size down when you press top face button
      if (EnableSizeDownWithBottomFaceButton)
      {
         if (VRInputMgr.GetBottomFaceButtonHeld(VRInputMgr.Hand.Left))
         {
            float offsetAmt = SizeUpDownWithFaceButtonMult * Time.deltaTime;
            float finalOffset = Mathf.Max(-.9f, GetContentSizeOffset() - offsetAmt);
            SetContentSizeOffset(finalOffset);
         }
      }
      //cycle thru image "frames" with cheat
      if(EnableCycleImageFramesWithButton)
      {
         bool isCalibration = VRPhoneCalibrator.I && VRPhoneCalibrator.I.GetIsCalibrating();
         if (GetIsManipulating() &&  VRInputMgr.GetTopFaceButtonDown(VRInputMgr.Hand.Right) && !isCalibration)
         {
            _curContent.CycleToNextFrame();
         }
      }
   }
}
