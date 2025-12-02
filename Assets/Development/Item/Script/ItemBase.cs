using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public ParticleSystem collectParticle;
    public AudioClip collectSound;

    public string TargetTag => Tag.Player;

    // 아이템 효과 적용 메서드
    public abstract void Apply();

    private void Collect()
    {
        Apply();
        collectParticle?.Play();
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