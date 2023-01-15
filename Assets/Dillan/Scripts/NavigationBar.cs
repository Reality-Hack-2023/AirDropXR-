using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NavigationModeEvent : UnityEvent<int> { }

public class NavigationBar : MonoBehaviour
{
    public GameObject [] Panels;
    [HideInInspector]
    public int FormSwitch;

   //events
   public NavigationModeEvent OnNavModeChange = new NavigationModeEvent();

    public void Start()
    {
        Panels[0].SetActive(true);
    }
    public void NavigationBarPanel(GameObject ActivePanel)
    {
        for(int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(false);
        }
        ActivePanel.SetActive(true);

         for(int i = 0; i < Panels.Length; i++)
        {
            if(Panels[i].activeSelf)
            {
                FormSwitch = i;
                Debug.Log(FormSwitch);

            }
        }

         //tell app mgr
      OnNavModeChange.Invoke(FormSwitch);
    }
}
