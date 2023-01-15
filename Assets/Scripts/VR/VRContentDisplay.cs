using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VRContentDisplay : MonoBehaviour
{
   public Transform RotationOffsetReciever;
   public Transform SizeOffsetReceiver;
   public ContentTypes ContentType = ContentTypes.Image;
   public Image ImageReceiver;
   public UnityEngine.Video.VideoPlayer VideoPlayerIns;
   public UnityEngine.Video.VideoPlayer Video360PlayerIns;
   public TextMeshProUGUI DownloadingText;
   public AudioSource Audio;


   [Space(10)]
   public int DefaultFrameIdx = 0;
   public ContentFrame[] Frames = new ContentFrame[0];

   [Space(10)]

   public Transform TetherAttachPt;

   [Space(10)]

   public float RemoteImageDelaySecs = 1.0f; //gotta wait a bit before we can request the download

   int _curFrameIdx = -1;
   Sprite _curSprite = null;

   [System.Serializable]
   public class ContentFrame
   {
      public GameObject FrameVisual;
      public bool MapImageTo3DModel = false;
      public Renderer RndToMapImage = null;
      public string MaterialPropForSprite = null;
   }

   public enum ContentTypes : uint
   {
      Image = 0,
      Video,           
      Video360,
      Audio
   }

   void Awake()
   {
      if (DownloadingText)
         DownloadingText.gameObject.SetActive(false);
   }

   public void ConfigureImage(Sprite sp)
   {
      ContentType = ContentTypes.Image;
      _curSprite = sp;
      if (ImageReceiver)
         ImageReceiver.sprite = sp;

      _SetFrame(DefaultFrameIdx);
   }

   public void ConfigureForRemoteImage(int imageNum)
   {
      StartCoroutine(_DoDownloadRemoteImage(imageNum));
   }

   IEnumerator _DoDownloadRemoteImage(int remoteImageNum)
   {
      //clear image
      if (ImageReceiver)
         ImageReceiver.sprite = null;

      //showDownloading text
      if (DownloadingText)
         DownloadingText.gameObject.SetActive(true);

      //wait a bit (to be sure server has image, and also for download text to be seen
      yield return new WaitForSeconds(RemoteImageDelaySecs);

      //request image from network
      if (NetworkContentMgr.I && NetworkContentMgr.I.RemoteSpriteGetter)
      {
         NetworkContentMgr.I.RemoteSpriteGetter.OnDownloadComplete.AddListener(_OnRemoteImageComplete);
         NetworkContentMgr.I.RemoteSpriteGetter.DownloadImage(remoteImageNum, ImageReceiver);
      }
   }

   void _OnRemoteImageComplete()
   {
      //hide download text
      if (DownloadingText)
         DownloadingText.gameObject.SetActive(false);
   }

   public Transform GetTetherAttach()
   {
      return TetherAttachPt ? TetherAttachPt : this.transform;
   }

   //go to next "frame" around image (if we have any), wrap around when we get to last one
   public void CycleToNextFrame()
   {
      if (Frames.Length <= 1)
         return;

      int newFrameIdx = (_curFrameIdx + 1) % Frames.Length;
      _SetFrame(newFrameIdx);
   }

   void _SetFrame(int frameIdx)
   {
      if (frameIdx == _curFrameIdx)
         return;

      _curFrameIdx = frameIdx;

      for(int i = 0; i < Frames.Length; i++)
      {
         var frame = Frames[i];
         
         //turn on appropriate frame visual
         if (frame.FrameVisual)
            frame.FrameVisual.SetActive(i == frameIdx);

         if(i == frameIdx)
         {
            //map to 3d model
            if (frame.MapImageTo3DModel && frame.RndToMapImage && (frame.MaterialPropForSprite.Length > 0) && _curSprite)
            {
               if (ImageReceiver)
                  ImageReceiver.gameObject.SetActive(false);

               frame.RndToMapImage.material.SetTexture(frame.MaterialPropForSprite, _curSprite.texture);
            }
            else
            {
               //turn normal image receiver back on
               if (ImageReceiver)
                  ImageReceiver.gameObject.SetActive(true);
            }
         }
      }
   }

   public void ConfigureVideo(VideoClip clip)
   {
      //configure and play video here!
      ContentType = ContentTypes.Video;
      if (VideoPlayerIns)
         VideoPlayerIns.clip = clip;
         Debug.Log(VideoPlayerIns.clip.name);
         // VideoPlayerIns.Play();
   }

   public void ConfigureAudio(AudioClip clip)
   {
      //TODO
   }

   public void ConfigureVideo360(VideoClip clip)
   {
      //configure and play video here!
      ContentType = ContentTypes.Video360;
      if (VideoPlayerIns)
         VideoPlayerIns.clip = clip;
         Debug.Log(VideoPlayerIns.clip.name);
         // VideoPlayerIns.Play();
   }
}
