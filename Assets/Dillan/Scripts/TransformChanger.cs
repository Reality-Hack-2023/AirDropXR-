using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformChanger : MonoBehaviour   
{   
    [HideInInspector]
    public Transform cube;
    private GameObject OwnerObject; 


    // public Pinch Pinch;
    public NavigationBar Form;
    public float speed = 1000f;
    public float rotationspeed = 1f;

    public Touch touch;
    public RectTransform rectTransform;
    public Vector2 point;
   
    private float _temp;
     private bool _isDragging;
    private float _currentScale;
    public float minScale, maxScale;
    private float _scalingRate = 2;
    public float RotationOffsetValue;
    public float PositionOffsetValue;
    public float SizeValue;

    bool _isDraggingFinger = false;

    float _yPosOnTouched =0.0f;
    float _curPosOffsetOnTouched = 0.0f;


    

    // Start is called before the first frame update
    void Awake()
    {
        cube = this.GetComponent<Transform>();
        // Pinch = Pinch.GetComponent<Pinch>();

    }
  
    
    void Update()
    {

        ChangeTransform();


    }

   float _GetCurOffsetValue()
   {
     if(Form.FormSwitch == 0)
        return PositionOffsetValue;
     else if(Form.FormSwitch == 1)
       return RotationOffsetValue;
     else if(Form.FormSwitch == 2)
       return SizeValue;
    
     return PositionOffsetValue;
   }

   void _SetCurOffsetValue(float f)
   {
     if(Form.FormSwitch == 0)
     {
        PositionOffsetValue = f;
        PhoneAppManager.I.SetPositionValue(PositionOffsetValue); 
     }
     else if(Form.FormSwitch == 1)
     {
       RotationOffsetValue = f;
       PhoneAppManager.I.SetRotationValue(RotationOffsetValue); 
     }
     else if(Form.FormSwitch == 2)
     {
       SizeValue = f;
       PhoneAppManager.I.SetSizeValue(SizeValue); 
     }
   }

    public void ChangeTransform()
    {
    
                if(Input.touchCount > 0 )
                {
                    touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Moved && _isDraggingFinger)
                        {
                            //if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out point))
                             {
                                   // cube.transform.position = new Vector3(cube.transform.position.x + touch.deltaPosition.x/speed, cube.transform.position.y + touch.deltaPosition.y/speed, 0);
                                   //PositionOffsetValue = ((touch.deltaPosition.y - 0)/(50-0));
                                   //Debug.Log(PositionOffsetValue);
                                  
                                
                                const float kOffsetSensitivity = 1.0f;//50.0f;
                                float deltaSinceTouch = (touch.position.y - _yPosOnTouched) / kOffsetSensitivity;
                                
                                float newOffset = Mathf.Max(0.0f, _curPosOffsetOnTouched + deltaSinceTouch);    
                                _SetCurOffsetValue(newOffset);
                                Debug.Log("TouchMOVE touchY " + touch.position.y + " deltaSinceTouch " + deltaSinceTouch + " final offset: " + _GetCurOffsetValue() +" Time "+ Time.time);
                            }
                        }
                    else if(touch.phase == TouchPhase.Began)
                        {
                            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.touches[0].position, Camera.current, out point))
                             {
                                   // cube.transform.position = new Vector3(cube.transform.position.x + touch.deltaPosition.x/speed, cube.transform.position.y + touch.deltaPosition.y/speed, 0);
                                  _yPosOnTouched = touch.position.y;
                                  _curPosOffsetOnTouched = _GetCurOffsetValue();
                                  Debug.Log("TouchBegan touchY " + _yPosOnTouched + " curOffset " + _curPosOffsetOnTouched + " Time "+ Time.time);
                                  _isDraggingFinger = true; 
                            }
                        }
                        else if(touch.phase == TouchPhase.Ended)
                        {
                          _isDraggingFinger = false; 
                        }
                }

       

              }




  

  
}
