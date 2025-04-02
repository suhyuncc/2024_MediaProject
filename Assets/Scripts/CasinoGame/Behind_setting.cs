using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behind_setting : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject set_card;
    [SerializeField]
    private AudioClip set_sfx;

    private void OnEnable()
    {
        //Set();
    }

    public void Set()
    {
        animator.SetTrigger("set");
        GameManager.Instance.Change_SFX(set_sfx);
        StartCoroutine(Set(0.95f));
    }

    public void Reroll()
    {
        animator.SetTrigger("reroll");
        //GameManager.Instance.Change_SFX(set_sfx);
        StartCoroutine(Reroll(2.05f));
    }

    IEnumerator Set(float delay)
    {
        

        yield return new WaitForSecondsRealtime(delay);

        this.gameObject.SetActive(false);
        set_card.SetActive(true);

        StopCoroutine(Set(delay));
    }

    IEnumerator Reroll(float delay)
    {


        yield return new WaitForSecondsRealtime(delay);

        this.gameObject.SetActive(false);
        set_card.SetActive(true);

        StopCoroutine(Reroll(delay));
    }
}
