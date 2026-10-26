using UnityEngine;
using UnityEngine.InputSystem;

public class ManualArmController : MonoBehaviour {
    [SerializeField] private RevoluteJoint[] joints = new RevoluteJoint[5];
    [SerializeField] private float speedDegreesPerSecond = 45f;

    private int selectedJoint = 0;

    private void Update(){
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null) return;

        if (keyboard.digit1Key.wasPressedThisFrame) selectedJoint = 0;
        if (keyboard.digit2Key.wasPressedThisFrame) selectedJoint = 1;
        if (keyboard.digit3Key.wasPressedThisFrame) selectedJoint = 2;
        if (keyboard.digit4Key.wasPressedThisFrame) selectedJoint = 3;
        if (keyboard.digit5Key.wasPressedThisFrame) selectedJoint = 4;

        if (selectedJoint >= joints.Length || joints[selectedJoint] == null) return;

        float direction = 0f;

        if (keyboard.rightArrowKey.isPressed) direction += 1f;
        if (keyboard.leftArrowKey.isPressed) direction -= 1f;

        float angleStep = direction * speedDegreesPerSecond * Time.deltaTime;
        joints[selectedJoint].RotateBy(angleStep);
    }
}