using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
public class ParticleMultiplier : MonoBehaviour
{
    public float maxAmount = 50f;

    private ParticleSystem particles;
    private float multiplier = 1f;

    // TODO: If using gameobject pooling in the future, set particles OnEnable
    // and reset particles to inital values OnDisable
    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        if (multiplier != 1f)
            MultiplyParticles();
    }

    void MultiplyParticles()
    {
        var e = particles.emission;
        e.rateOverTime = MultiplyCurve(e.rateOverTime, multiplier);

        for (int i = 0; i < e.burstCount; i++)
        {
            var burst = e.GetBurst(i);
            burst.count = MultiplyCurve(burst.count, multiplier);
            e.SetBurst(i, burst);
        }
    }

    ParticleSystem.MinMaxCurve MultiplyCurve(ParticleSystem.MinMaxCurve curve, float f)
    {
        curve.constant = Mathf.Ceil(curve.constant * f);
        curve.constantMax = Mathf.Ceil(curve.constantMax * f);
        curve.constantMin = Mathf.Ceil(curve.constantMin * f);
        curve.curveMultiplier = f;

        return curve;
    }

    public void SetAmount(float amount)
    {
        multiplier = Mathf.InverseLerp(0f, maxAmount, amount);
    }

    public void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }
}
