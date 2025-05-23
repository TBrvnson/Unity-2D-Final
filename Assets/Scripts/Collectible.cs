using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public enum CollectType
    {
        Cherry,
        Diamond,
        Heart,
        Flag,
        Goal
    }
        
    [SerializeField] private CollectType type;
    private bool isCollected = false;
    [SerializeField] private int diamondUID = -1;
    [SerializeField] AudioClip collectSound;

    private void Start()
    {
        if (type == CollectType.Diamond)
        {
            foreach (int uid in GameManager.instance.collectedDiamonds)
            {
                if (diamondUID == uid)
                {
                    isCollected = true;
                    GetComponent<Animator>().SetBool("isCollected", isCollected);
                    break;
                }
            }
        }
    }

    public CollectType GetCollectType()
    {
        return type;
    }

    public bool IsCollected()
    {
        return isCollected;
    }

    public int GetDiamondUID()
    {
        return diamondUID;
    }

    public void Collect()
    {
        AudioManager.instance.PlaySoundEffect(collectSound, transform, 2f, 0.3f);
        StartCoroutine(DestroyAfterCollect());
    }

    IEnumerator DestroyAfterCollect()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("collected");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}
