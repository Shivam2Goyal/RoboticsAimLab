# RoboticsAimLab

A Unity robotics learning project with a five-joint simulated arm.

## Environment
- Unity 6.6: 6000.6.5f1
- Universal Render Pipeline
- Unity Input System
- Units: metres; joint angles stored in degrees

## Verified progress
- Test scene with floor, target, and camera
- Five-joint transform hierarchy
- Configurable joint axes and angular limits
- Keyboard control: number-row 1–5 selects a joint; arrows move it
- Forward-kinematics composition checked against Unity
- Pose prediction from proposed angles without moving the scene
- Numerical position Jacobian tested at straight and bent poses

## Scripts
- TargetSpinner: introductory exercise; disabled
- RevoluteJoint: joint angle, rotation axis, and limits
- ManualArmController: keyboard control
- ForwardKinematicsCheck: FK prediction and numerical checks

## Implementation assumptions
- Root and joint scales remain (1, 1, 1)
- Joint zero pose uses zero local rotations
- Joint axes: Y, Z, Z, Y, Z
- Visual geometry is separate from joint coordinate frames

## Resume here
The next exercise is one Jacobian-transpose IK correction.
Its code was provided, but its before/after error results are
not yet confirmed.

Next: verify that correction reduces endpoint error, then
develop an iterative solver and handle limits and singularities.