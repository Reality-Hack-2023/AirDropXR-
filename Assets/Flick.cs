using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flick : MonoBehaviour
{
    public Touch touch;
    public RectTransform rectTransform;
    private float Swipevalue;
    private Vector2 point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void flick()
    {
         if(Input.touchCount > 0 )
                {
                    touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Moved)
                        {
                            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out point))
                             {
                                   // cube.transform.position = new Vector3(cube.transform.position.x + touch.deltaPosition.x/speed, cube.transform.position.y + touch.deltaPosition.y/speed, 0);
                                   Swipevalue = touch.deltaPosition.y;
                                   
                            }
                        }
                }
        
    }
    void Spawn()
    {
        
    }
}
