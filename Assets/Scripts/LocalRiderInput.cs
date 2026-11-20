using UnityEngine;
using UnityEngine.InputSystem;

namespace ChefRush
{
    [RequireComponent(typeof(RiderMotor))]
    public sealed class LocalRiderInput : MonoBehaviour
    {
        [SerializeField] private InputActionReference steerRef;
        [SerializeField] private InputActionReference accelRef;
        [SerializeField] private InputActionReference resetRef;
        private float steerInput;
        private float accelInput;

        private RiderMotor motor;

        private bool resetRequested;

        private void Awake()
        {
            motor = GetComponent<RiderMotor>();



        }

        private void OnEnable()
        {
            resetRef.action.performed += RequestReset;
            steerRef.action.Enable();
            accelRef.action.Enable();
            resetRef.action.Enable();
        }

        private void OnDisable()
        {


            steerRef.action.Disable();
            accelRef.action.Disable();
            resetRef.action.Disable();
            resetRef.action.performed -= RequestReset;
            resetRequested = false;
        }

        //private void OnDestroy() => riderActions?.Dispose();

        private void RequestReset(InputAction.CallbackContext context) => resetRequested = true;

        private void FixedUpdate()
        {
            if (resetRequested)
            {
                resetRequested = false;
                motor.ResetToSafePose();
                return;
            }

            motor.Simulate(steerRef.action.ReadValue<float>(), accelRef.action.ReadValue<float>(), Time.fixedDeltaTime);
        }
    }
}
