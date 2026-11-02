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
- Endpoint orientation indicator along local +Z
- Alignment geometry checks and configurable alignment-success indicator
- Continuous-alignment timer and one tag per uninterrupted alignment

Latest additions (learner-reported working; code present):
- Tool-to-target angular error, ahead/behind check, and target-centre
  distance to the forward ray
- Configurable 3-degree angular and 0.1-metre ray-distance tolerances
- Timed virtual tagging with a default one-second continuous hold
- Red/green/cyan status ray and yellow target line in the Scene view

Automatic replanning triggers correctly, but general goal following is
not reliable because the position solver can stall. Keep Automatic
Following off for current manual orientation and alignment exercises.

## Scene structure

```text
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
                    ├── EndpointMarker
                    └── ToolDirectionVisual
```

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
  automatic goal following, endpoint-direction inspection, and
  target-alignment measurements, visual alignment status, and timed tagging.

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
- Experimental; currently disabled for lessons due to solver convergence issues.
- Enable Automatic Following in the Inspector to test it.
- Goal changes greater than 5 mm request another solve.
- Attempts are limited to at most four per second.
- Failed attempts hold the current pose.
- An unchanged failed goal is not retried automatically.
- Turning following off prevents new requests but does not cancel an
  animation already running.

Orientation and alignment:
- Keep Automatic Following off and enable manual control.
- Assign the Target cube to Interaction Target; keep IKGoal in Ik Goal.
- ToolDirectionVisual extends along the endpoint's local +Z direction.
- Use "Inspect Endpoint Direction" and "Measure Tool Alignment" from
  the component context menu.
- During Play mode, enable Gizmos in the Scene view. The yellow line
  connects the endpoint to the target centre. The forward ray is red
  when unaligned, green while holding alignment, and cyan after a tag.

Timed virtual tagging:
- The target must be ahead and within both alignment tolerances.
- Hold alignment continuously for Required Hold Time (default: 1 second).
- A successful hold logs a tag and increments the play-session counter.
- Remaining aligned does not repeatedly register tags.
- Losing alignment resets the hold timer and permits another tag.
- The same target can be tagged again after breaking and restoring alignment.
- Timer and count are not yet displayed in a Game-view HUD.

## Validation results

- Zero-angle endpoint position: approximately (0, 4.65, 0).
- Joint2 at 90°, other joints zero: approximately (-2.9, 1.75, 0).
- A single Jacobian-transpose correction reduced a test error
  from approximately 0.10000 m to 0.05821 m.
- A known-reachable iterative test reached approximately
  0.0009918 m error, within the 1 mm tolerance.
- Predicted and actual scene endpoint errors agreed to
  floating-point precision.
- Automatic triggering was confirmed during debugging. A subsequent
  solve stopped at iteration 6: error changed from 0.9684123 m to
  0.9683778 m, then the candidate slightly worsened it to 0.968378 m.
  No animation started because unsuccessful solutions are not applied.
- Alignment tests were accepted as working by the learner. With all
  joints at zero, the reference cases are:
  - Target (0, 4.65, 2): 0 degrees, ahead, 0 m ray distance.
  - Target (1, 4.65, 2): about 26.565 degrees, ahead, 1 m ray distance.
  - Target (0, 4.65, -2): 180 degrees, behind, 2 m ray distance.
- Colour-threshold and timed-tag exercises were reported working.
  Raw measurements and timing logs have not been archived.

## Assumptions and limitations

- RobotRoot and joint scales remain (1, 1, 1).
- Joint reference poses use zero local rotation.
- Visual scaling is separate from joint coordinate frames.
- IK currently controls endpoint position only.
- Different joint configurations can reach the same position.
- Solver failure does not prove that a target is unreachable.
- Straight-arm singularities and difficult targets can cause failure.
- The baseline solver uses a Jacobian-transpose update,
  a one-degree per-joint step cap, and up to 500 iterations.
- Solver work is synchronous and may cause frame pauses.
- Smooth joint movement does not guarantee a straight endpoint path.
- Joint limits do not prevent self-collision or obstacle collision.
- Alignment measurements describe geometry only, not collisions or hits.
- Tags are geometric alignment events, not physical contact or projectile
  hits; target size and intervening obstacles are ignored.
- No orientation-solving objective, projectile system, supervised model,
  reinforcement learning, or multi-arm coordinator has been implemented.
- General IK convergence is explicitly deferred. A step-size reduction
  was proposed during debugging, but no successful recovery was verified.

## Learning checkpoint — 2 November 2026

Completed topics:
- GameObjects, components, and parent/local/world transforms
- C# fields, methods, arrays, and component references
- Frame-rate-independent movement
- Quaternion rotation composition
- Forward kinematics and numerical differentiation
- Jacobian-transpose IK and convergence checks
- Coroutines and smooth joint movement
- Distinguishing automatic triggering, solver success, and animation
- Endpoint orientation and local-to-world direction conversion
- Angular alignment, dot-product projection, and forward-ray distance
- Combining angle and distance thresholds into a boolean success condition
- Continuous hold timing, reset behaviour, and preventing repeated events

Resume next:
1. Display alignment status, hold progress, and tag count in the Game view.
2. Continue the manual tagging task independently of the deferred IK issue.
3. Revisit solver robustness before relying on arbitrary moving goals.

Future direction: a harmless virtual catching or tagging task, followed
by multi-arm task assignment. No moving-object interception or learned
coordination has been built.

Workflow:
Code is explained in chat and entered manually by the learner.
Features are marked verified only after implementation and testing.
Documentation may be edited directly when the learner explicitly requests it.
