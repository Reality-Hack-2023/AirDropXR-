using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class postRequestMain : MonoBehaviour {
    // remove start to prevent start on launch

    public Sprite sprite;
    public string filename;
    public int inc = 0;
    public string url="http://172.20.10.5:44444";
    void Start()
    {
        // StartCoroutine(postRequest());
        
    }

    public void OnClick()
    {
        inc+=1;
        Debug.Log("clicked");
        object[] fname = {inc, ".png"};
        filename = string.Join("", fname);
        StartCoroutine(postRequest(filename=filename));
    }

    public IEnumerator postRequest(string filename)
    {
        // string url="http://172.20.10.5:44444";
        byte[] data = sprite.texture.EncodeToPNG();
        var multiPartSectionName = "sketch";
        //comment out before integration
        // filename = "temp.png";

        // List<IMultipartFormSection> imgData = new List<IMultipartFormSection>();
        // imgData.Add(new MultipartFormDataSection("foo", "bar"));
        // imgData.Add(new MultipartFormFileSection(multiPartSectionName, data, filename, "video/mp4"));
        WWWForm imgData=new WWWForm();
        imgData.AddBinaryData("file", data, filename, "image/jpeg");

        UnityWebRequest uwr = UnityWebRequest.Post(url, imgData);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
        // yield return new WaitForSeconds(4);
    }
}