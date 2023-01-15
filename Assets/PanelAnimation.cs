using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{   
   public AnimationCurve FlickAnimEase = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
   public RectTransform ImageToMove;
 
   public void TriggerFlickAnim()
   {
      StartCoroutine(_DoFlickAnim());
   }

   IEnumerator _DoFlickAnim()
   {
      const float kAnimTime = .5f; //how long to travel
      const float kFlickDistance = 100; //how far to travel

      float startTime = Time.time;
      float endTime = startTime + kAnimTime;
      float curTime = Time.time;

      while(curTime <= endTime)
      {
         curTime = Time.time;
         float u = Mathf.InverseLerp(startTime, endTime, curTime);
         float easedU = FlickAnimEase.Evaluate(u);

         float curTravelAmount = easedU * kFlickDistance;

         //TODO: apply to image here!!
         this.gameObject.transform.localPosition= new Vector3(this.gameObject.transform.localPosition.x, curTravelAmount, this.gameObject.transform.localPosition.z);

         yield return new WaitForEndOfFrame();
      }

      //TODO: trigger next screen here!
   }

    
}
