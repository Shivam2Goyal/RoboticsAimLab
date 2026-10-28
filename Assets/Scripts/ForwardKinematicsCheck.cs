using UnityEngine;

public class ForwardKinematicsCheck : MonoBehaviour {
    [SerializeField] private Transform robotRoot;
    [SerializeField] private Transform[] joints = new Transform[5];
    [SerializeField] private Transform endpoint;

    [ContextMenu("Check Forward Kinematics")]

    private void CheckForwardKinematics() {
        Vector3 position = robotRoot.position;
        Quaternion rotation = robotRoot.rotation;

        for(int i=0; i<joints.Length; i++){
            position += rotation * joints[i].localPosition;
            rotation = rotation * joints[i].localRotation;
        }

        position += rotation * endpoint.localPosition;
        rotation = rotation * endpoint.localRotation;

        float positionError = Vector3.Distance(position, endpoint.position);
        float rotationError = Quaternion.Angle(rotation, endpoint.rotation);

        Debug.Log("Calculated endpoint: " + position.ToString("F5"));
        Debug.Log("Unity endpoint: " + endpoint.position.ToString("F5"));
        Debug.Log("Position error (metres): " + positionError);
        Debug.Log("Rotation error (degrees): " + rotationError);
    }

    public void PredictPose(float[] angles, out Vector3 position, out Quaternion rotation){
        position = robotRoot.position;
        rotation = robotRoot.rotation;

        for(int i=0; i<joints.Length; i++){
            position += rotation * joints[i].localPosition;

            RevoluteJoint joint = joints[i].GetComponent<RevoluteJoint>();
            Quaternion localRotation = joint.CalculateLocalRotation(angles[i]);

            rotation = rotation * localRotation;
        }

        position += rotation * endpoint.localPosition;
        rotation = rotation * endpoint.localRotation;
    }

    [ContextMenu("Test Predicted Pose")]
    private void TestPredictedPose(){
        float[] proposedAngles = {0f, 90f, 0f, 0f, 0f};

        Vector3 predictedPosition;
        Quaternion predictedRotation;

        PredictPose(proposedAngles, out predictedPosition, out predictedRotation);

        Debug.Log("Predicted Position: " + predictedPosition.ToString("F5"));
        Debug.Log("Predicted up direction: " + predictedRotation * Vector3.up);
    }

    [ContextMenu("Test Position Jacobian")]
    private void TestPositionJacobian(){
        float[] angles = { 20f, 90f, -45f, 15f, 30f };

        Vector3[] columns = CalculatePositionJacobian(angles);

        for(int i=0; i<columns.Length; i++){
            Debug.Log(
                "Joint " + (i+1) + " column (m/rad): "
                + columns[i].ToString("F5")
            );
        }

        Vector3 startPosition;
        Quaternion unusedOrientation;
        PredictPose(angles, out startPosition, out unusedOrientation);

        Vector3 targetPosition = startPosition + new Vector3(0.1f, 0f, 0f);
        Vector3 error = targetPosition - startPosition;

        float gain = 0.05f;
        float maxStepRadians = 1f * Mathf.Deg2Rad;
        float[] candidateAngles = new float[angles.Length];

        for(int i=0; i<angles.Length; i++){
            float deltaRadians = gain * Vector3.Dot(columns[i], error);

            deltaRadians = Mathf.Clamp(
                deltaRadians, -maxStepRadians, maxStepRadians
            );

            candidateAngles[i] = angles[i] + deltaRadians * Mathf.Rad2Deg;
        }

        Vector3 candidatePosition;
        PredictPose(candidateAngles, out candidatePosition, out unusedOrientation);

        float beforeError = error.magnitude;
        float afterError = Vector3.Distance(targetPosition, candidatePosition);

        Debug.Log("Error before step (m): " + beforeError);
        Debug.Log("Error after step (m): " + afterError);
    }

    public Vector3[] CalculatePositionJacobian(float[] angles){
        float stepDegrees = 0.1f;
        Vector3[] columns = new Vector3[angles.Length];

        for(int i=0; i<angles.Length; i++){
            RevoluteJoint joint = joints[i].GetComponent<RevoluteJoint>();
            float originalAngle = angles[i];

            float plusAngle = joint.ClampAngle(originalAngle + stepDegrees);
            float minusAngle = joint.ClampAngle(originalAngle - stepDegrees);

            float intervalRadians = (plusAngle - minusAngle) * Mathf.Deg2Rad;

            if(intervalRadians <= 0f){
                columns[i] = Vector3.zero;
                continue;
            }

            Vector3 plusPosition;
            Vector3 minusPosition;
            Quaternion unusedRotation;

            angles[i] = plusAngle;
            PredictPose(angles, out plusPosition, out unusedRotation);

            angles[i] = minusAngle;
            PredictPose(angles, out minusPosition, out unusedRotation);

            angles[i] = originalAngle;

            columns[i] = (plusPosition - minusPosition) / intervalRadians;
        }

        return columns;
    }
}