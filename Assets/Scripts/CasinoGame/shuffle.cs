using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shuffle : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioClip clip;

    private void OnEnable()
    {
        Shuffle();
    }

    public void Shuffle()
    {
        animator.SetTrigger("shuffle");
        GameManager.Instance.Change_SFX(clip);
        StartCoroutine(Shuffle(2.0f));
    }

    IEnumerator Shuffle(float delay)
    {
        

        yield return new WaitForSecondsRealtime(delay);

        this.gameObject.SetActive(false);

        StopCoroutine(Shuffle(delay));
    }
}
