using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ReturnButtonEvent : UnityEvent<ReturnScript>{}

[RequireComponent(typeof(Button))]
public class ReturnScript : MonoBehaviour
{
    public int ReturnID = 0;

    //events
    public ReturnButtonEvent OnButtonPressed = new ReturnButtonEvent();

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
}