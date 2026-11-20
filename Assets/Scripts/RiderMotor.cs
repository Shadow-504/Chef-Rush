using UnityEngine;

namespace ChefRush
{
    public enum RiderRole
    {
        Chef,
        Courier
    }

    [RequireComponent(typeof(CharacterController))]
    public sealed class RiderMotor : MonoBehaviour
    {
        [SerializeField] private RiderRole role;
        [SerializeField, Min(0.1f)] private float minimumSpeed = 6f;
        [SerializeField, Min(0.1f)] private float cruiseSpeed = 12f;
        [SerializeField, Min(0.1f)] private float chefBoostSpeed = 17f;
        [SerializeField, Min(0.1f)] private float courierBoostSpeed = 21f;
        [SerializeField, Min(0.1f)] private float acceleration = 8f;
        [SerializeField, Min(0.1f)] private float deceleration = 16f;
        [SerializeField, Range(0f, 70f)] private float maxCarveAngle = 38f;
        [SerializeField, Min(1f)] private float turnRate = 115f;
        [SerializeField] private LayerMask groundLayers = Physics.DefaultRaycastLayers;
        [SerializeField, Min(1)] private float resetDrop = 45f;

        private CharacterController controller;
        private Pose resetPose;
        private float driveSpeed;
        private float verticalSpeed;

        public RiderRole Role { get => role; set => role = value; }
        public Vector3 Velocity { get; private set; }
        public bool IsGrounded { get; private set; }
        public float Steer { get; private set; }
        public Vector3 GroundNormal { get; private set; } = Vector3.up;
        public Vector3 GroundPoint { get; private set; }
        public event System.Action ResetCompleted;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            SetResetPose(new Pose(transform.position, transform.rotation));
            driveSpeed = cruiseSpeed;
        }

        public void Simulate(float steer, float boost, float brake, float deltaTime)
        {
            if (deltaTime <= 0f || !isActiveAndEnabled || !controller.enabled)
                return;

            bool hasGround = TryGetGround(out RaycastHit ground);
            GroundNormal = hasGround ? ground.normal : Vector3.up;
            GroundPoint = hasGround ? ground.point : transform.position;

            Steer = Mathf.Clamp(steer, -1f, 1f);
            float heading = resetPose.rotation.eulerAngles.y;
            Vector3 fallLine = Vector3.ProjectOnPlane(Vector3.down, GroundNormal);
            // Flat stretches use the route heading supplied by the last safe pose.
            if (fallLine.sqrMagnitude > 0.0025f)
                heading = Mathf.Atan2(fallLine.x, fallLine.z) * Mathf.Rad2Deg;

            float yaw = Mathf.MoveTowardsAngle(transform.eulerAngles.y, heading + Steer * maxCarveAngle, turnRate * deltaTime);
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, GroundNormal).normalized;
            float topSpeed = Mathf.Max(cruiseSpeed, role == RiderRole.Courier ? courierBoostSpeed : chefBoostSpeed);
            float targetSpeed = brake > 0f
                ? Mathf.Lerp(cruiseSpeed, Mathf.Min(minimumSpeed, cruiseSpeed), brake)
                : Mathf.Lerp(cruiseSpeed, topSpeed, boost);
            float speedChangeRate = targetSpeed < driveSpeed ? deceleration : acceleration;
            driveSpeed = Mathf.MoveTowards(driveSpeed, targetSpeed, speedChangeRate * deltaTime);

            verticalSpeed = hasGround ? 0f : verticalSpeed + Physics.gravity.y * deltaTime;
            Vector3 adhesion = hasGround ? -GroundNormal * 2f : Vector3.up * verticalSpeed;
            Vector3 motion = (forward * driveSpeed + adhesion) * deltaTime;
            Vector3 positionBeforeMove = transform.position;
            CollisionFlags contacts = controller.Move(motion);
            Velocity = (transform.position - positionBeforeMove) / deltaTime;
            IsGrounded = (contacts & CollisionFlags.Below) != 0;

            if (transform.position.y < resetPose.position.y - resetDrop)
                ResetToSafePose();
        }

        public void SetResetPose(Pose pose)
        {
            resetPose = new Pose(pose.position, Quaternion.Euler(0f, pose.rotation.eulerAngles.y, 0f));
        }

        public void ResetToSafePose()
        {
            bool wasEnabled = controller.enabled;
            controller.enabled = false;
            transform.SetPositionAndRotation(resetPose.position, resetPose.rotation);
            controller.enabled = wasEnabled;
            driveSpeed = cruiseSpeed;
            verticalSpeed = 0f;
            Velocity = Vector3.zero;
            IsGrounded = false;
            Steer = 0f;
            GroundNormal = Vector3.up;
            GroundPoint = resetPose.position;
            ResetCompleted?.Invoke();
        }

        private bool TryGetGround(out RaycastHit hit)
        {
            // The rider root sits at the capsule center. Keep the probe short so drops use gravity.
            float probeLength = controller.height * 0.5f + controller.skinWidth + 0.08f;
            return Physics.Raycast(transform.TransformPoint(controller.center), Vector3.down, out hit,
                       probeLength, groundLayers, QueryTriggerInteraction.Ignore) &&
                   Vector3.Angle(hit.normal, Vector3.up) <= controller.slopeLimit;
        }
    }
}
