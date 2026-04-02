using UnityEngine;

public class CharacterFacing : MonoBehaviour
{
    [SerializeField] PlayerController_Test rootController;
    [SerializeField] Transform facingBody;
    [SerializeField] ConfigurableJoint mainJoint;
    [SerializeField] float rotationSpeed = 8f;


    Quaternion startLocalRotation;
    Quaternion startWorldRotation;
    Quaternion currentTargetRotation;

    private void Start()
    {
        if(facingBody != null)
        {
            startLocalRotation = facingBody.localRotation;
            startWorldRotation = facingBody.rotation;
            currentTargetRotation = startLocalRotation;
        }
    }

    void FixedUpdate()
    {
        if (rootController == null || facingBody == null || mainJoint == null) return;

        Vector3 moveDir = rootController.CurrentMoveDirection;
        Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z);

        if (flatDir.sqrMagnitude < 0.001f) return;

        Quaternion yawTarget = Quaternion.LookRotation(flatDir, Vector3.up);

        Quaternion worldTarget = yawTarget * (Quaternion.LookRotation(startWorldRotation * Vector3.forward, Vector3.up)) * startWorldRotation;

        Transform parent = facingBody.parent;
        if (parent == null) return;

        Quaternion localTarget = Quaternion.Inverse(parent.rotation) * worldTarget;

        currentTargetRotation = Quaternion.Slerp(currentTargetRotation, localTarget, rotationSpeed * Time.fixedDeltaTime);

        mainJoint.SetTargetRotationLocal(currentTargetRotation, startLocalRotation);



    }
}
