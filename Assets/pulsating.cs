using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pulsating : MonoBehaviour
{
    private Image rend; 
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(pulse());
        rend = this.GetComponent<Image>();
        
    }

    IEnumerator pulse()
    {
        if(rend != null)
        {
        var re = rend.color.a;
        if(re >= 0.5)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0f);
        }
        else if(re == 0)
        {
             rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.7f);
        }
        Debug.Log(re);
     
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(pulse());
        }
    }

}
