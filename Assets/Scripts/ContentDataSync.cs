using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ContentDataSync : RealtimeComponent<ContentDataSyncModel> {
    public int ContentType = -1;
    public int ContentId = -1;
    public float TranslationOffset = 0;
    public float RotationOffset = 0;
    public float SizeOffset = 0;
    public bool IsManipulating = true;
    public int ManipulationMode = 0; //0, 1, or 2 (move, rotate, resize)
    public int RemoteImageNum = -1; //if >= 0, then we are an image type that is on the remote server (i.e a drawing)
    private ContentDataSyncModel _model = null;

   void Start()
   {
      if (NetworkContentMgr.I)
         NetworkContentMgr.I.RegisterNetContent(this);
   }

   void OnDestroy()
   {
      if (NetworkContentMgr.I)
         NetworkContentMgr.I.UnregisterNetContent(this);
   }

    protected override void OnRealtimeModelReplaced(ContentDataSyncModel previousModel, ContentDataSyncModel currentModel) {
        // if (previousModel != null) {
        //     // Unregister from events
        //     previousModel.colorDidChange -= ColorDidChange;
        // }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel) {
                currentModel.contentType = ContentType;
                currentModel.contentId = ContentId;
                currentModel.translationOffset = TranslationOffset;
                currentModel.rotationOffset = RotationOffset;
                currentModel.isManipulating = IsManipulating;
                currentModel.sizeOffset = SizeOffset;
                currentModel.manipulateMode = ManipulationMode;
                currentModel.remoteImageNum = RemoteImageNum;
            }
            _model = currentModel;
            // Register for events so we'll know if the color changes later
            // currentModel.colorDidChange += ColorDidChange;
        }
    }

    void Update() {
        if (_model != null)  //if we own it, put our properties in the model
        {
            //bool isLocal = isOwnedLocallySelf;
            bool isLocal = realtimeView.isOwnedLocallySelf || realtimeView.isOwnedLocallyInHierarchy;
            if (isLocal)
            {
               _model.contentType = ContentType;
               _model.contentId = ContentId;
               _model.translationOffset = TranslationOffset;
               _model.rotationOffset = RotationOffset;
               _model.isManipulating = IsManipulating;
               _model.sizeOffset = SizeOffset;
               _model.manipulateMode = ManipulationMode;
               _model.remoteImageNum = RemoteImageNum;
            }
            else //if we arent the owner, get the freshest values from the model
            {
               ContentType = _model.contentType;
               ContentId = _model.contentId;
               TranslationOffset = _model.translationOffset;
               RotationOffset = _model.rotationOffset;
               SizeOffset = _model.sizeOffset;
               IsManipulating = _model.isManipulating;
               ManipulationMode = _model.manipulateMode;
               RemoteImageNum = _model.remoteImageNum;
            }
         }
    }
}