using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public ParticleSystem collectParticle;
    public AudioClip collectSound;

    // 아이템 효과 적용 메서드
    public abstract void Apply();
    // 아이템을 획득할 수 있는지 여부를 결정하는 조건 메서드
    public abstract bool CanCollect(Collider other);

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
        if (CanCollect(other))
        {
            Collect();
            Destroy(gameObject);
        }
    }
}