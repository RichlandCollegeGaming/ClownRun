using Unity.VisualScripting;
using UnityEngine;

public static class ConfigurableJointExtensions
{
   
    public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
    {
        if (joint.configuredInWorldSpace)
        {
            Debug.LogError("Joint rotationlocal should not be used with world space");
            return;
        }

        SetTargetRotationInternal(joint, targetLocalRotation, startLocalRotation, Space.Self);
    }

    public static void SetTargetRotationWorld(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
    {
        if (!joint.configuredInWorldSpace)
        {
            Debug.LogError("Joint rotationworld should be used with world space");
            return;
        }
        SetTargetRotationInternal(joint, targetWorldRotation, startWorldRotation, Space.World);
    }

    static void SetTargetRotationInternal(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
    {
        Vector3 right = joint.axis;
        Vector3 forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;
    
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);
    
            
    
        
        if (space == Space.World)
        {
            resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
        }
        else
        {
            resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
        }

        resultRotation *= worldToJointSpace;

        joint.targetRotation = resultRotation;

    }





}
