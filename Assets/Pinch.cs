using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pinch : MonoBehaviour 
{
    public Vector2 previousPinch1;
    public Vector2 previousPinch2;
    public Vector2 currentPinch1;
    public Vector2 currentPinch2;

    public Vector2 pos1;
    public Vector2 pos2;
    public Vector2 Currentpos1;
    public Vector2 Currentpos2;
     public Vector2 distance;
     public Vector2 _Temp;
     public float Zoom;
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         foreach(Touch touch in Input.touches)
        {
         if(Input.touchCount ==2)
            {
                
                  if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out previousPinch1))
                  { 
                       
                       pos1 = new Vector2( (previousPinch1.x+173f)/(164f-173f), (previousPinch1.y+352f)/(339f-352f)); 
                        
                  }
                  if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[1].position, Camera.current, out previousPinch2))
                  {
                        pos2 = new Vector2( (previousPinch2.x+173f)/(164f-173f), (previousPinch2.y+352f)/(339f-352f));
                        
                  }
                
                
                 

                  
                 
                  
                
            var pos1b = pos1 - Input.GetTouch(0).deltaPosition;
            var pos2b = pos2 - Input.GetTouch(1).deltaPosition;


                    //calc zoom
                    Zoom = Vector3.Distance(pos1, pos2);
                
                     if (Zoom == 0 || Zoom >= 1)
                return;
               
                
                }
        }
    }
}
