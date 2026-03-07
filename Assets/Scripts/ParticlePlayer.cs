using UnityEngine;
using UnityEngine.Events;
using Core;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private ParticleSystem particleEffect1;
    private UnityEvent OnParticleEffectTriggered;
        private void Start()
        {
            GameManager.Instance.OnParticleEffectTriggered.AddListener(PlayParticleEffect);
        }
    private void OnDestroy()
    {
        GameManager.Instance.OnParticleEffectTriggered.RemoveListener(PlayParticleEffect);
    }
    public void PlayParticleEffect()
    {
        Debug.Log("Particle effect triggered!");
        if (particleEffect != null)
        {
            particleEffect.Play();
        }
        if (particleEffect1 != null)
        {
            particleEffect1.Play();
        }
    }
}
