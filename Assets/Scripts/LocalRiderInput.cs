using UnityEngine;
using UnityEngine.InputSystem;

namespace ChefRush
{
    [RequireComponent(typeof(RiderMotor))]
    public sealed class LocalRiderInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;

        private RiderMotor motor;
        private InputActionMap riderActions;
        private InputAction steer;
        private InputAction boost;
        private InputAction brake;
        private InputAction reset;
        private bool resetRequested;

        private void Awake()
        {
            motor = GetComponent<RiderMotor>();
            if (actions == null)
            {
                Debug.LogError("Assign the input actions asset to the local rider.", this);
                enabled = false;
                return;
            }

            riderActions = actions.FindActionMap("Rider", throwIfNotFound: true).Clone();
            steer = riderActions.FindAction("Steer", throwIfNotFound: true);
            boost = riderActions.FindAction("Boost", throwIfNotFound: true);
            brake = riderActions.FindAction("Brake", throwIfNotFound: true);
            reset = riderActions.FindAction("Reset", throwIfNotFound: true);
        }

        private void OnEnable()
        {
            reset.performed += RequestReset;
            riderActions.Enable();
        }

        private void OnDisable()
        {
            if (riderActions == null)
                return;

            riderActions.Disable();
            reset.performed -= RequestReset;
            resetRequested = false;
        }

        private void OnDestroy() => riderActions?.Dispose();

        private void RequestReset(InputAction.CallbackContext context) => resetRequested = true;

        private void FixedUpdate()
        {
            if (resetRequested)
            {
                resetRequested = false;
                motor.ResetToSafePose();
                return;
            }

            motor.Simulate(steer.ReadValue<float>(), boost.ReadValue<float>(), brake.ReadValue<float>(), Time.fixedDeltaTime);
        }
    }
}
