using UnityEngine;

public class RevoluteJoint : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float angleDegrees = 0f;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    // Update is called once per frame
    void Update()
    {
        angleDegrees = Mathf.Clamp(angleDegrees, minAngle, maxAngle);

        transform.localRotation = CalculateLocalRotation(angleDegrees);
    }

    public void RotateBy(float deltaDegrees){
        angleDegrees = Mathf.Clamp(
            angleDegrees + deltaDegrees,
            minAngle,
            maxAngle
        );
    }

    public Quaternion CalculateLocalRotation(float requestedAngle){
        float limitedAngle = ClampAngle(requestedAngle);
        return Quaternion.AngleAxis(limitedAngle, rotationAxis);
    }

    public float ClampAngle(float requestedAngle){
        return Mathf.Clamp(requestedAngle, minAngle, maxAngle);
    }
}
