using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public ParticleSystem collectParticle;
    public AudioClip collectSound;

    public abstract void Apply();

    private void Collect()
    {
        Apply();
        collectParticle?.Play();
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, SoundVolume.SFX.ItemCollect);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Player))
        {
            Collect();
            Destroy(gameObject);
        }
    }
}