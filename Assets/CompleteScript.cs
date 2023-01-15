using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class CompleteButtonEvent : UnityEvent<CompleteScript>{}

[RequireComponent(typeof(Button))]
public class CompleteScript : MonoBehaviour
{
    public int CompleteID = 0;
   public postRequestMain ImageSender;

    //events
    public CompleteButtonEvent OnButtonPressed = new CompleteButtonEvent();

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
      //send image
        ImageSender.OnClick();
      //tell the phone app
        OnButtonPressed.Invoke(this);
    }
}