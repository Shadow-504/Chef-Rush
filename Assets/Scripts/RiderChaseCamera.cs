using UnityEngine;

namespace ChefRush
{
    public sealed class RiderChaseCamera : MonoBehaviour
    {
        [SerializeField] private RiderMotor target;
        [SerializeField] private Vector3 offset = new Vector3(0.65f, 3f, -6f);
        [SerializeField, Min(0)] private float lookAhead = 5f;
        [SerializeField, Min(0.1f)] private float followSharpness = 10f;
        [SerializeField, Min(0)] private float carveRoll = 4f;
        [SerializeField, Min(0)] private float carveOffset = 0.5f;

        private void OnEnable()
        {
            if (target == null)
                return;

            target.ResetCompleted += SnapToTarget;
            SnapToTarget();
        }

        private void OnDisable()
        {
            if (target != null)
                target.ResetCompleted -= SnapToTarget;
        }

        private void LateUpdate() => Follow(1f - Mathf.Exp(-followSharpness * Time.deltaTime)); //

        private void SnapToTarget() => Follow(1f); //

        private void Follow(float blend)
        {
            if (target == null)
                return;

            Transform rider = target.transform;
            Vector3 followOffset = offset - Vector3.right * target.Steer * carveOffset;
            Vector3 position = Vector3.Lerp(transform.position, rider.TransformPoint(followOffset), blend);
            //Vector3 lookPoint = rider.position + rider.forward * lookAhead;
            //Quaternion rotation = Quaternion.LookRotation(lookPoint - position, Vector3.up);
            //rotation *= Quaternion.AngleAxis(-target.Steer * carveRoll, Vector3.forward);
            transform.position = position;
        }
    }
}
