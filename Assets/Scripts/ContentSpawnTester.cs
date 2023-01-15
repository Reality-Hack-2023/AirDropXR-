using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ContentSpawnTester : MonoBehaviour
{
    public Realtime NormCoreManager;
    private ContentDataSync _contentDataSync;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject spawnedContentObject = Realtime.Instantiate("ContentNetSyncer", ownedByClient: true, useInstance: NormCoreManager);
            _contentDataSync = spawnedContentObject.GetComponent<ContentDataSync>();

            RealtimeView myObjectView = spawnedContentObject.GetComponent<RealtimeView>();
            myObjectView.RequestOwnership();
      }
    }

    
}
