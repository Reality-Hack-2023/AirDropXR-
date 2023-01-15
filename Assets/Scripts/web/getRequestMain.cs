using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;


public class getRequestMain : MonoBehaviour
{

    public Image image;
    public int fileOrder;
    public KeyCode DownloadScriptKey = KeyCode.Y;
    public string path = "http://172.20.10.5:44444/server_download/";

   public UnityEvent OnDownloadComplete = new UnityEvent();

    void Update()
    {
      // change it to listen for a command to download script
      /*if (Input.GetKeyDown(DownloadScriptKey))
      {
         // filename = "temp.png";
         object[] fname = { fileOrder, ".png" };
         string filename = string.Join("", fname);
         DownloadImage(filename);
      }    */
    }

    public void DownloadImage(int fileOrder, Image image)
    {
        object[] fname = {fileOrder, ".png"};
        string filename = string.Join("", fname);
        StartCoroutine(ImageRequest(filename, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            } else
            {
                // Get the texture out using a helper downloadhandler
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                // Save it into the Image UI's sprite
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

              OnDownloadComplete.Invoke();
            }
        }));
    }

    IEnumerator ImageRequest(string filename, Action<UnityWebRequest> callback)
    {
        
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(Path.Join(path, filename)))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }
}