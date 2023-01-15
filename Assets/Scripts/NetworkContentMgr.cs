//
// See when content objects are spawned in multiplayer and make corresponding content visuals in VR
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkContentMgr : MonoBehaviour
{
   public float TranslationOffsetScale = 1.0f;
   public float RotationOffsetScale = 1.0f;
   public float SizeOffsetScale = 1.0f;

   [Space(10)]

   public getRequestMain RemoteSpriteGetter;

   public static NetworkContentMgr I { get; private set; }

   //track a piece of content spawned by a network client
   [System.Serializable]
   class RuntimeNetContent
   {
      public NetContentState State = NetContentState.None;
      public ContentDataSync NetContent = null;
      public VRContentDisplay VRContent = null;

      public bool IsReady()
      {
         //ContentID and ContentType default to -1 and will be 0 or more once we receive data
         return NetContent && (NetContent.ContentId >= 0) && (NetContent.ContentType >= 0);
      }
   }

   public enum NetContentState
   {
      None,
      WaitingToBeReady,
      IsBeingManipulated,
      IsPlaced,
      Error
   }

   List<RuntimeNetContent> _netContents = new List<RuntimeNetContent>();

   void Awake()
   {
      I = this;
   }

   public void RegisterNetContent(ContentDataSync netContent)
   {
      //only care about tracking these on the VR client
      if(NetworkMgr.I.GetIsVRClient())
      {
         //begin tracking it
         RuntimeNetContent n = new RuntimeNetContent();
         n.NetContent = netContent;
         //we need to wait until we are sure we;ve received the content idx and type
         n.State = NetContentState.WaitingToBeReady;

         _netContents.Add(n);
      }
   }

   int _FindNetContentIdx(ContentDataSync content)
   {
      for(int i = 0; i < _netContents.Count; i++)
      {
         if (_netContents[i].NetContent == content)
            return i;
      }
      return -1;
   }

   public void UnregisterNetContent(ContentDataSync netContent)
   {
      int contentIdx = _FindNetContentIdx(netContent);
      if (contentIdx >= 0)
         _netContents.RemoveAt(contentIdx);
   }

   void Update()
   {
      foreach(var c in _netContents)
      {
         //wait to till we receive data we need to create the vr display
         if(c.State == NetContentState.WaitingToBeReady)
         {
            if(c.IsReady())
            {
               Debug.Log("Spawning visual with contentType " + c.NetContent.ContentType + " and id " + c.NetContent.ContentId);

               var contentType = (VRContentDisplay.ContentTypes)c.NetContent.ContentType;
               if (c.NetContent.RemoteImageNum >= 0)
                  contentType = VRContentDisplay.ContentTypes.Image;
               VRContentDisplay cd = ContentMgr.I.SpawnContent(contentType, c.NetContent.ContentId, c.NetContent.RemoteImageNum);
               if (cd != null)
               {
                  VRPhoneMgr.I.StartManipulatingContent(cd);
                  c.State = NetContentState.IsBeingManipulated;
                  c.VRContent = cd;

                  //now that we're being manipulated, we need put content that may have been manipulated before into the "placed" state
                  foreach(var otherC in _netContents)
                  {
                     if (otherC == c)
                        continue;

                     if (otherC.State == NetContentState.IsBeingManipulated)
                        otherC.State = NetContentState.IsPlaced;
                  }
               }
               else
               {
                  c.State = NetContentState.Error;
                  Debug.LogWarning("Error: cant spawn vr content display for content " + ((VRContentDisplay.ContentTypes)c.NetContent.ContentType).ToString() + " with ID " + c.NetContent.ContentId);
               }
            }
         }
         else if(c.State == NetContentState.IsBeingManipulated)
         {
            //VRPhoneMgr.I.SetContentForwardOffset(TranslationOffsetScale*c.NetContent.TranslationOffset);
            //VRPhoneMgr.I.SetContentRotationOffset(RotationOffsetScale*c.NetContent.RotationOffset);
            //VRPhoneMgr.I.SetContentSizeOffset(SizeOffsetScale * c.NetContent.SizeOffset);
            VRPhoneMgr.I.SetContentForwardOffset(TranslationOffsetScale*c.NetContent.transform.localPosition.y);
            VRPhoneMgr.I.SetContentRotationOffset(RotationOffsetScale*c.NetContent.transform.localPosition.x);
            VRPhoneMgr.I.SetContentSizeOffset(SizeOffsetScale * c.NetContent.transform.localPosition.z);

            int manipModeInt = c.NetContent.ManipulationMode;
            VRPhoneMgr.I.SetUIManipMode((VRPhoneMgr.UIManipMode)manipModeInt );

            //stop manipulating if flag goes false
            if (!c.NetContent.IsManipulating)
            {
               if (VRPhoneMgr.I.GetManipulationContent() == c.VRContent) //double check phone mgr thinks we are the one being manipulated!
               {
                  VRPhoneMgr.I.StopManipulatingContent();
                  c.State = NetContentState.IsPlaced;
               }
               else
               {
                  c.State = NetContentState.Error;
                  Debug.LogWarning("Error: manipulating flag went false but isnt the one being manipulated.  for content: " + ((VRContentDisplay.ContentTypes)c.NetContent.ContentType).ToString() + " with ID " + c.NetContent.ContentId);
               }
            }
         }
      }
   }
}
