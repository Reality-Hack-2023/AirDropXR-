using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
   
    float InitialPositionX;
    float InitialPositionY;
    float CurrentPositionX;
    float CurrentPositionY;
    [HideInInspector]
    public float DeltaX;
    [HideInInspector] 
    public float DeltaY;
    public Vector2 previousLocalpoint;
    public Vector2 CurrentLocalpoint;
    
    




    public Vector2 distance;
    public RectTransform rectTransform;

    void Start()
    {
        float rectWidth = (rectTransform.anchorMax.x - rectTransform.anchorMin.x)*Screen.width;
        float rectHeight =(rectTransform.anchorMax.y - rectTransform.anchorMin.y)*Screen.height;
        Vector2 position = new Vector2(rectTransform.anchorMin.x*Screen.width, rectTransform.anchorMin.y * Screen.height);
 
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0) return;
           if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform,Input.touches[0].position ))
            {
                Touches();
              
            }
        else 
            {
               // Debug.Log("Menu");
            }
       

    }
    public void Touches()
    {
        foreach(Touch touch in Input.touches)
        {
            if(Input.touchCount == 1 )
            {
                if(touch.phase == TouchPhase.Began)
                {
                  if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out previousLocalpoint))
                  {
                        InitialPositionX = Mathf.Abs(previousLocalpoint.x+173f)/(164f-173f);
                        InitialPositionY = Mathf.Abs(previousLocalpoint.y+352f)/(339f-352f);
                        // Debug.Log("CurrentLocalPointX" + InitialPositionX);
                        // Debug.Log("CurrentLocalPointX" + InitialPositionY);   
                  }
                  
                }
                if(touch.phase == TouchPhase.Moved)
                {
                    if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out CurrentLocalpoint))
                  {
                        CurrentPositionX = Mathf.Abs(CurrentLocalpoint.x+173f)/(164f-173f);
                        CurrentPositionY = Mathf.Abs(CurrentLocalpoint.y+352f)/(339f-352f);
                       ///    Debug.Log(CurrentPositionX);
                        DeltaX = (InitialPositionX - CurrentPositionX);
                        DeltaY = (InitialPositionY - CurrentPositionY);
                    //    Debug.Log("Delta"+DeltaX);
                    //    Debug.Log("DeltaY"+DeltaY);
                
                  }
                }

            }   
           
        }
    }
}
              
    
    
  
       
  



  

