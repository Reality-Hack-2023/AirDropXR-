using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpringyChaser : MonoBehaviour
{
   public Transform ChaseTarget = null;
   [Space(10)]
   public bool EnablePos = true;
   public float Spring_Pos = 100.0f;
   public float Damper_Pos = 10.0f;
   [Space(10)]
   public bool EnableRot = true;
   public float Spring_Rot = 100.0f;
   public float Damper_Rot = 10.0f;
   [Space(10)]
   public Vector3 DesiredVelocity = Vector3.zero;
   public Vector3 DesiredAngularVelocity = Vector3.zero;
   [Space(10)]
   public bool ScaleSettingsWithMass = true;

   Rigidbody _myBody = null;

   void Awake()
   {
      _myBody = GetComponent<Rigidbody>();
   }

   void FixedUpdate()
   {
      if (!ChaseTarget)
         return;

      Vector3 chasePos = ChaseTarget.position;
      Quaternion chaseRot = ChaseTarget.rotation;

      if (EnablePos)
      {

         Vector3 posForce = (Spring_Pos * (chasePos - transform.position)) - (Damper_Pos * (_myBody.velocity - DesiredVelocity));
         //scale with mass
         if(ScaleSettingsWithMass)
            posForce *= _myBody.mass;

         _myBody.AddForce(posForce);
      }

      if (EnableRot)
      {
         Transform pivot = this.transform;
         Quaternion meInv = Quaternion.Inverse(transform.rotation);
         Quaternion pivotOffset = meInv * pivot.rotation;

         Quaternion change = chaseRot * Quaternion.Inverse(transform.rotation);

         float changeAngle = 0.0f;
         Vector3 changeAxis = Vector3.up;
         change.ToAngleAxis(out changeAngle, out changeAxis);
         if (changeAngle > 180.0f)
            changeAngle = changeAngle - 360.0f;
         else if (changeAngle < -180.0f)
            changeAngle = changeAngle + 360.0f;

         if (Mathf.Abs(changeAngle) >= .01f)
         {
            Vector3 rotForce = (Spring_Rot * changeAngle * Mathf.Deg2Rad * changeAxis) - (Damper_Rot * (_myBody.angularVelocity - DesiredAngularVelocity));
            Vector3 rotForceLocal = transform.InverseTransformVector(rotForce);
            Vector3 tensor = _myBody.inertiaTensor;
            if (!ScaleSettingsWithMass)
               tensor = new Vector3(1.0f, 1.0f, 1.0f);

            Vector3 torque = transform.TransformVector(Vector3.Scale(rotForceLocal, tensor));

            _myBody.AddTorque(torque);
         }
      }
   }
}
