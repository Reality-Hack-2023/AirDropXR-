//
// get at things like networking status
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

[RequireComponent(typeof(Realtime))]
public class NetworkMgr : MonoBehaviour
{
   Realtime _normcoreMgr = null;

   public bool GetIsMultiplayerSession()
   {
      return _normcoreMgr ? _normcoreMgr.connected : false;
   }

   public bool GetIsVRClient()
   {
      return VRCamRig.I != null;
   }

   public bool GetIsPhoneClient()
   {
      return !GetIsVRClient();
   }

   public static NetworkMgr I { get; private set; }
   void Awake()
   {
      I = this;

      _normcoreMgr = GetComponent<Realtime>();
   }

}
