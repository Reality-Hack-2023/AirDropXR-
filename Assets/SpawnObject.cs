using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SpawnObject : MonoBehaviour
{
    public Realtime NormCoreManager;
    private ContentDataSync _contentDataSync;

    
    public TransformChanger TransformChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_contentDataSync == null) return;
        _contentDataSync.RotationOffset = TransformChange.RotationOffsetValue;
      
    }
    public void SpawnPrefab()
    {
        if(!NetworkMgr.I.GetIsMultiplayerSession() ) return;
       
        GameObject spawnedContentObject = Realtime.Instantiate("ContentNetSyncer", ownedByClient: true, useInstance: NormCoreManager);
        _contentDataSync = spawnedContentObject.GetComponent<ContentDataSync>();

        RealtimeView myObjectView = spawnedContentObject.GetComponent<RealtimeView>();
        myObjectView.RequestOwnership();
    
       


    }
     
}
