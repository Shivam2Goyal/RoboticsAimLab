# RoboticsAimLab

A Unity robotics learning project featuring a five-joint simulated
arm, manual control, forward kinematics, and numerical inverse
kinematics.

## Environment

- Unity 6.6 — Editor 6000.6.5f1
- Universal Render Pipeline (URP)
- Unity Input System
- Units: metres
- Joint angles stored in degrees
- Jacobian calculations use radians

## Current capabilities

Verified:
- Five-joint transform hierarchy
- Configurable joint axes and angular limits
- Keyboard joint control
- Forward kinematics checked against Unity's world transforms
- Pose prediction without moving the scene
- Numerical position Jacobian with boundary-aware differences
- Jacobian-transpose position IK
- Application of solved angles to the visible arm
- Smooth movement toward successful solutions
- Scene-goal solving tested after troubleshooting

Latest addition:
- Optional automatic replanning when IKGoal moves
- End-to-end automatic-following checks remain to be recorded

## Scene structure

RobotRoot
└── Joint1
    ├── Link1Visual
    └── Joint2
        ├── Link2Visual
        └── Joint3
            ├── Link3Visual
            └── Joint4
                ├── Link4Visual
                └── Joint5
                    ├── Link5Visual
                    └── EndpointMarker

IKGoal is a separate scene-root object.
The scene also contains a floor, target cube, camera, and lighting.

## Joint configuration

| Joint | Local axis | Minimum | Maximum |
|---|---|---:|---:|
| Joint1 | Y | -180° | 180° |
| Joint2 | Z | -90° | 90° |
| Joint3 | Z | -120° | 120° |
| Joint4 | Y | -180° | 180° |
| Joint5 | Z | -90° | 90° |

Link lengths: 1.5, 1.2, 1.0, 0.4, and 0.3 metres.
Joint1 is 0.25 metres above RobotRoot.

## Scripts

- TargetSpinner
  Introductory rotation exercise; normally disabled.

- RevoluteJoint
  Stores joint angles, enforces limits, calculates rotations,
  and applies angles to joint Transforms.

- ManualArmController
  Selects and moves joints using the keyboard.

- ForwardKinematicsCheck
  Contains FK diagnostics, pose prediction, numerical Jacobian
  calculations, position IK, smooth movement, and optional
  automatic goal following.

## Controls

Manual control:
- Enable ManualArmController.
- Click inside the Game view.
- Number-row keys 1–5 select a joint.
- Left/Right arrows decrease/increase its angle.

IK control:
- Disable ManualArmController while using animated IK.
- Use the ForwardKinematicsCheck component's context menu.
- "Place IK Goal At Endpoint" positions the goal at the current tip.
- Move IKGoal, then select "Solve To IK Goal".
- A successful solution starts smooth joint movement.

Automatic following:
- Enable Automatic Following in the Inspector.
- Goal changes greater than 5 mm request another solve.
- Attempts are limited to at most four per second.
- Failed attempts hold the current pose.
- An unchanged failed goal is not retried automatically.

## Validation results

- Zero-angle endpoint position: approximately (0, 4.65, 0).
- Joint2 at 90°, other joints zero: approximately (-2.9, 1.75, 0).
- A single Jacobian-transpose correction reduced a test error
  from approximately 0.10000 m to 0.05821 m.
- A known-reachable iterative test reached approximately
  0.0009918 m error, within the 1 mm tolerance.
- Predicted and actual scene endpoint errors agreed to
  floating-point precision.

## Assumptions and limitations

- RobotRoot and joint scales remain (1, 1, 1).
- Joint reference poses use zero local rotation.
- Visual scaling is separate from joint coordinate frames.
- IK currently controls endpoint position only.
- Different joint configurations can reach the same position.
- Solver failure does not prove that a target is unreachable.
- Straight-arm singularities and difficult targets can cause failure.
- The solver uses a fixed-gain Jacobian-transpose update,
  a one-degree per-joint step cap, and up to 500 iterations.
- Solver work is synchronous and may cause frame pauses.
- Smooth joint movement does not guarantee a straight endpoint path.
- Joint limits do not prevent self-collision or obstacle collision.
- No projectile system, aiming objective, supervised model,
  or reinforcement learning has been implemented.

## Learning checkpoint — 29 October 2026

Completed topics:
- GameObjects, components, and parent/local/world transforms
- C# fields, methods, arrays, and component references
- Frame-rate-independent movement
- Quaternion rotation composition
- Forward kinematics and numerical differentiation
- Jacobian-transpose IK and convergence checks
- Coroutines and smooth joint movement

Resume next:
1. Confirm automatic following and failed-goal behaviour.
2. Check responsiveness and solver failure cases.
3. Improve solver robustness before extending the task objective.

Workflow:
Code is explained in chat and entered manually by the learner.
Features are marked verified only after implementation and testing.