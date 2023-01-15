using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ContentButtonEvent : UnityEvent<ContentScript>{}


[RequireComponent(typeof(Button))]
public class ContentScript : MonoBehaviour
{
    public int ContentID = 0;
    public Image contentImage;

    //events
    public ContentButtonEvent OnButtonPressed = new ContentButtonEvent();

   Button _button = null;

   public Button GetButton() { return _button;}

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(_OnClick);

      
    }

    void _OnClick()
    {
        OnButtonPressed.Invoke(this);
    }
    public void RefreshImage(int contentType)
    {
        Sprite sp = null;
        if(contentType == 0)
        {
            sp = ContentMgr.I.GetImageSprite(ContentID);
        }
        else if(contentType == 1)
        {
            sp = ContentMgr.I.GetVideoPreviewSprite(ContentID);
        }
        else if(contentType == 2)
        {
            sp = ContentMgr.I.Get360VideoPreviewSprite(ContentID);
        }
         if( contentImage)
               contentImage.sprite = sp;
        
        
    }
}