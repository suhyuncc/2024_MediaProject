using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Sprite[] card_images;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationClip clip;
    [SerializeField]
    private bool is_back;
    [SerializeField]
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip()
    {
        StopAllCoroutines();

        animator.SetTrigger("flip");


        StartCoroutine(Wait(clip.length));
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSecondsRealtime(delay/1.5f);

        if (is_back)
        {
            int ran = Random.Range(0, card_images.Length);

            image.sprite = card_images[ran];
        }

        is_back = false;
    }
}
