using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ParticleSystem collectParticle;
    [SerializeField] private AudioClip collectSound;

    public string TargetTag => Tag.Player;

    // 아이템 효과 적용 메서드
    public abstract void Apply();

    private void Collect()
    {
        Apply();
        collectParticle.Play();
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, _SoundManager.SFXVolume);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            Collect();
            Destroy(gameObject);
        }
    }
}