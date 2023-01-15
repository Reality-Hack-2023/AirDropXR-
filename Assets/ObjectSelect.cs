using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelect : MonoBehaviour
{
    private Button [] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<Button>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
