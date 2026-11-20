using UnityEngine;
using UnityEngine.Rendering;

namespace ChefRush
{
    [RequireComponent(typeof(RiderMotor))]
    public sealed class SledEffects : MonoBehaviour
    {
        [SerializeField] private Transform sledVisual;
        [SerializeField] private Material powderMaterial;
        [SerializeField] private Material runnerMarkMaterial;
        [SerializeField, Range(0f, 25f)] private float carveLean = 15f;

        private RiderMotor motor;
        private ParticleSystem powder;
        private ParticleSystem chips;
        private TrailRenderer leftMark;
        private TrailRenderer rightMark;
        private bool wasSliding;

        private void Awake()
        {
            motor = GetComponent<RiderMotor>();
            powder = CreateSpray("Snow powder", 320, new ParticleSystem.MinMaxCurve(0.4f, 0.92f),
                new ParticleSystem.MinMaxCurve(0.24f, 0.62f), new ParticleSystem.MinMaxCurve(2.2f, 5.6f),
                new Color(0.62f, 0.83f, 1f, 0.82f), false);
            chips = CreateSpray("Ice chips", 100, new ParticleSystem.MinMaxCurve(0.3f, 0.57f),
                new ParticleSystem.MinMaxCurve(0.065f, 0.14f), new ParticleSystem.MinMaxCurve(4.5f, 7.5f),
                new Color(0.94f, 0.99f, 1f, 0.92f), true);
            leftMark = CreateRunnerMark("Left runner trail");
            rightMark = CreateRunnerMark("Right runner trail");
        }

        private void OnEnable() => motor.ResetCompleted += ClearEffects;

        private void OnDisable()
        {
            motor.ResetCompleted -= ClearEffects;
            ClearEffects();
        }

        private void LateUpdate()
        {
            Vector3 normal = motor.GroundNormal;
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, normal).normalized;
            Vector3 right = Vector3.Cross(normal, forward);
            Quaternion slopeRotation = Quaternion.LookRotation(forward, normal);
            if (sledVisual != null)
            {
                Quaternion lean = Quaternion.AngleAxis(-motor.Steer * carveLean, Vector3.forward);
                sledVisual.rotation = Quaternion.Slerp(sledVisual.rotation, slopeRotation * lean,
                    1f - Mathf.Exp(-12f * Time.deltaTime));
            }

            bool sliding = motor.IsGrounded && motor.Velocity.sqrMagnitude > 4f;
            SetSprayRate(powder, sliding, 42f + 105f * Mathf.Abs(motor.Steer));
            SetSprayRate(chips, sliding, 9f + 40f * Mathf.Abs(motor.Steer));
            leftMark.emitting = rightMark.emitting = sliding;
            if (sliding)
            {
                Vector3 contact = transform.position - normal * Vector3.Dot(transform.position - motor.GroundPoint, normal);
                Vector3 rear = contact - forward * 0.68f + normal * 0.06f;
                Vector3 sprayDirection = -forward - right * (motor.Steer * 1.2f) + normal * 0.42f;
                Quaternion sprayRotation = Quaternion.LookRotation(sprayDirection, normal);
                powder.transform.SetPositionAndRotation(rear - right * (motor.Steer * 0.32f), sprayRotation);
                chips.transform.SetPositionAndRotation(rear, sprayRotation);
                Quaternion markRotation = Quaternion.FromToRotation(Vector3.forward, normal);
                leftMark.transform.SetPositionAndRotation(rear - right * 0.34f, markRotation);
                rightMark.transform.SetPositionAndRotation(rear + right * 0.34f, markRotation);

                if (!wasSliding)
                {
                    leftMark.Clear();
                    rightMark.Clear();
                    powder.Emit(16);
                    chips.Emit(8);
                }
            }

            wasSliding = sliding;
        }

        private ParticleSystem CreateSpray(string label, int maxParticles, ParticleSystem.MinMaxCurve lifetime,
            ParticleSystem.MinMaxCurve size, ParticleSystem.MinMaxCurve speed, Color color, bool stretch)
        {
            Transform emitter = CreateChild(label);
            emitter.gameObject.SetActive(false);
            var particles = emitter.gameObject.AddComponent<ParticleSystem>();
            var main = particles.main;
            main.playOnAwake = false;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.maxParticles = maxParticles;
            main.startLifetime = lifetime;
            main.startSize = size;
            main.startSpeed = speed;
            main.startColor = color;
            main.gravityModifier = stretch ? 0.9f : 0.08f;
            var emission = particles.emission;
            emission.rateOverTime = 0f;
            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = stretch ? 24f : 34f;
            shape.radius = 0.24f;
            var fade = particles.colorOverLifetime;
            fade.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(new[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) },
                new[] { new GradientAlphaKey(0.65f, 0), new GradientAlphaKey(1f, 0.08f), new GradientAlphaKey(0f, 1) });
            fade.color = gradient;
            var expansion = particles.sizeOverLifetime;
            expansion.enabled = !stretch;
            expansion.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0, 0.55f, 1, 1.5f));
            var render = particles.GetComponent<ParticleSystemRenderer>();
            render.sharedMaterial = powderMaterial;
            render.shadowCastingMode = ShadowCastingMode.Off;
            render.renderMode = stretch ? ParticleSystemRenderMode.Stretch : ParticleSystemRenderMode.Billboard;
            render.lengthScale = 1.25f;
            render.velocityScale = 0.04f;
            emitter.gameObject.SetActive(true);
            return particles;
        }

        private TrailRenderer CreateRunnerMark(string label)
        {
            var trail = CreateChild(label).gameObject.AddComponent<TrailRenderer>();
            trail.sharedMaterial = runnerMarkMaterial;
            trail.shadowCastingMode = ShadowCastingMode.Off;
            trail.alignment = LineAlignment.TransformZ;
            trail.time = 3.2f;
            trail.minVertexDistance = 0.13f;
            trail.widthMultiplier = 0.125f;
            trail.startColor = new Color(0.19f, 0.42f, 0.62f, 0.65f);
            trail.endColor = new Color(0.19f, 0.42f, 0.62f, 0f);
            trail.emitting = false;
            return trail;
        }

        private Transform CreateChild(string label)
        {
            Transform child = new GameObject(label).transform;
            child.SetParent(transform, false);
            return child;
        }

        private static void SetSprayRate(ParticleSystem particles, bool sliding, float rate)
        {
            if (sliding && !particles.isPlaying)
                particles.Play();

            var emission = particles.emission;
            emission.rateOverTime = sliding ? rate : 0f;
        }

        private void ClearEffects()
        {
            wasSliding = false;
            powder.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            chips.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            leftMark.emitting = rightMark.emitting = false;
            leftMark.Clear();
            rightMark.Clear();
            if (sledVisual != null)
                sledVisual.rotation = transform.rotation;
        }
    }
}
