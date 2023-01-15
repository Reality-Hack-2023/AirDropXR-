//
// Store references to all the canned content in the experience
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContentMgr : MonoBehaviour
{
   [Header("Config")]
   public KeyCode DeleteAllContentCheat = KeyCode.Backspace;
   public VRContentDisplay DisplayPrefab_Image;
   public Sprite[] Images = new Sprite[0];
   //TODO: make list of Video clips here!
   public VRContentDisplay DisplayPrefab_Video;
   public VideoEntry[] Videos = new VideoEntry[0];
   public VRContentDisplay DisplayPrefab_Video360;

   public VideoEntry[] Videos360 = new VideoEntry[0];
   public AudioEntry[] AudioClips =  new AudioEntry[0];
   public VRContentDisplay DisplayPrefab_AudioSource;
   [Space(10)]
   public float RemoteImageDelaySecs = 1.0f; //gotta wait a bit before we can request the download

   [System.Serializable]
   
   public class AudioEntry
   {
      public UnityEngine.AudioClip AudioClips = null; 
      public Sprite PreviewImage = null;
   }

   [System.Serializable]
   public class VideoEntry
   {
      public UnityEngine.Video.VideoClip Video = null;
      public Sprite PreviewImage = null;
   }

   //events
   public UnityEvent OnAllContentDeleting = new UnityEvent();

   List<VRContentDisplay> _spawnedContent = new List<VRContentDisplay>(); //all the content currently spawned


   public static ContentMgr I { get; private set; }
   void Awake()
   {
      I = this;
   }


   public void DeleteAllContent()
   {
      //tell other scripts so they can stop referencing any active content!
      OnAllContentDeleting.Invoke();

      foreach (var v in _spawnedContent)
      {
         if (v)
            Destroy(v.gameObject);
      }
      _spawnedContent.Clear();
   }

   public Sprite GetImageSprite(int contentID)
   {
      return _GetImageSprite(contentID);
   }

   public Sprite GetVideoPreviewSprite(int contentID)
   {
      return _GetVideoPreviewSprite(contentID);
   }

   public Sprite Get360VideoPreviewSprite(int contentID)
   {
      return _GetVideo360PreviewSprite(contentID);
   }
   public Sprite GetAudioPreviewSprite(int contentID)
   {
      return _GetAudioPreviewSprite(contentID);
   }

   public VRContentDisplay SpawnContent(VRContentDisplay.ContentTypes contentType, int contentID, int remoteImageNum = -1)
   {
      VRContentDisplay result = null;
      if (contentType == VRContentDisplay.ContentTypes.Image)
      {
         if(DisplayPrefab_Image == null)
         {
            Debug.LogWarning("No content prefab for images specified!");
            return null;
         }

         GameObject contentObj = Instantiate(DisplayPrefab_Image.gameObject) as GameObject;
         VRContentDisplay content = contentObj.GetComponent<VRContentDisplay>();
         if(remoteImageNum >= 0)//remote server image
         {
            content.ConfigureForRemoteImage(remoteImageNum);
         }
         else //normal image
         {
            var sprite = _GetImageSprite(contentID);
            if (sprite == null)
            {
               Debug.LogWarning("Can't find image content with ID " + contentID);
               return null;
            }
            content.ConfigureImage(sprite);
         }

         result = content;
      }
      else if(contentType == VRContentDisplay.ContentTypes.Video)
      {
         //spawn and configure VRContentDisplay for video here!!
         if(DisplayPrefab_Video == null)
         {
            Debug.LogWarning("No content prefab for videos specified!");
            return null;
         }
         GameObject contentObj = Instantiate(DisplayPrefab_Video.gameObject) as GameObject;
         VRContentDisplay content = contentObj.GetComponent<VRContentDisplay>();
         var player = _GetVideoClip(contentID);
         if(player == null)
         {
            Debug.LogWarning("Can't find video content with ID " + contentID);
            return null;
         }
         content.ConfigureVideo(player);

         result = content;
      }
      else if(contentType == VRContentDisplay.ContentTypes.Video360)
      {
         if(DisplayPrefab_Video360 == null)
         {
            Debug.LogWarning("No content prefab for 360 videos specified!");
            return null;
         }
         GameObject contentObj = Instantiate(DisplayPrefab_Video360.gameObject) as GameObject;
         VRContentDisplay content = contentObj.GetComponent<VRContentDisplay>();
         var player = _GetVideo360Clip(contentID);
         if(player == null)
         {
            Debug.LogWarning("Can't find 360 video content with ID " + contentID);
            return null;
         }
         content.ConfigureVideo(player);

         result =  content;
      }
      else if(contentType == VRContentDisplay.ContentTypes.Audio)
      {
         if(DisplayPrefab_AudioSource == null)
         {
            Debug.LogWarning("No content prefab for 360 videos specified!");
            return null;
         }
         GameObject contentObj = Instantiate(DisplayPrefab_AudioSource.gameObject) as GameObject;
         VRContentDisplay content = contentObj.GetComponent<VRContentDisplay>();
         var player = _GetAudioClip(contentID);
         if(player == null)
         {
            Debug.LogWarning("Can't find AudioClip content with ID " + contentID);
            return null;
         }
         content.ConfigureAudio(player);

         result =  content;
      }

      //start tracking this content
      if (result)
      {
         //adjust audio, make current load
         if (result.Audio)
            result.Audio.volume = 1.0f;

         //other content gets turned down in mix
         const float kOtherVolume = .25f;
         foreach(var otherC in _spawnedContent)
         {
            if (otherC && otherC.Audio)
               otherC.Audio.volume = kOtherVolume;
         }

         _spawnedContent.Add(result);
      }

      return result;
   }


   Sprite _GetImageSprite(int contentID)
   {
      if (contentID < 0 || contentID >= Images.Length)
         return null;
      return Images[contentID];
   }

   UnityEngine.Video.VideoClip _GetVideoClip(int contentID)
   {
      if (contentID < 0 || contentID >= Videos.Length)
         return null;
      return Videos[contentID].Video;
   }
   Sprite _GetVideoPreviewSprite(int contentID)
   {
      if (contentID < 0 || contentID >= Videos.Length)
         return null;
      return Videos[contentID].PreviewImage;
   }

   UnityEngine.Video.VideoClip _GetVideo360Clip(int contentID)
   {
      if (contentID < 0 || contentID >= Videos360.Length)
         return null;
      return Videos360[contentID].Video;
   }

   Sprite _GetVideo360PreviewSprite(int contentID)
   {
      if (contentID < 0 || contentID >= Videos360.Length)
         return null;
      return Videos360[contentID].PreviewImage;
   }
   UnityEngine.AudioClip _GetAudioClip(int contentID)
   {
      if (contentID < 0 || contentID >= Videos.Length)
         return null;
      return AudioClips[contentID].AudioClips;
   }

   
   Sprite _GetAudioPreviewSprite(int contentID)
   {
      if (contentID < 0 || contentID >= Videos.Length)
         return null;
      return AudioClips[contentID].PreviewImage;
   }

   void Update()
   {
      if (Input.GetKeyDown(DeleteAllContentCheat))
         DeleteAllContent();
   }
}
