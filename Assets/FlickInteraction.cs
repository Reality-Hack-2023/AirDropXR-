using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FlickInteraction : MonoBehaviour
{
    public RectTransform picture;
    public RectTransform rectTransform;
    public Touch touch;
    public float Flickoffset;
    public Vector3 initialTransform;
    public float speed; 
    private Vector2 point;
    public UnityEvent OnFlicked =  new UnityEvent();
    
    // Start is called before the first frame update
    void Start()
    {
       

    }

    void SetButton(bool ButtonSwipe)
    {

    }

    void OnEnable()
    {
         RectTransform picture = GetComponent<RectTransform>();

         initialTransform = picture.gameObject.transform.position;
        
          ResetFlick();
    }
    void Flick()
    {
        


    }
    void ResetFlick()
    {
        picture.gameObject.transform.position = initialTransform;
        picture.gameObject.SetActive(true);
    }
    void CheckBoundary()
    {
        if(picture.anchoredPosition.y >= 200f)
        {
             OnFlicked.Invoke(); 
             picture.gameObject.SetActive(false);

           
        }
     
    }

    

    // Update is called once per frame
    void Update()
    {
        Flick();
        CheckBoundary();
    }
}
