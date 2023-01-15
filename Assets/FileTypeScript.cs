using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class FileTypeButtonEvent : UnityEvent<FileTypeScript>{}

[RequireComponent(typeof(Button))]
public class FileTypeScript : MonoBehaviour
{
    public int FileTypeID = 0;

    //events
    public FileTypeButtonEvent OnButtonPressed = new FileTypeButtonEvent();

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