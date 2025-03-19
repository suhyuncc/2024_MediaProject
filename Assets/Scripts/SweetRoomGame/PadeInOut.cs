using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PadeInOut : MonoBehaviour
{
    [SerializeField]
    private RawImage image;

    private void OnEnable()
    {
        StartCoroutine("pade");
    }

    IEnumerator pade()
    {
        float spawntime = 1.0f;
        float time = 0f;

        Color alpha = image.color;

        while (time < spawntime)
        {
            time += Time.deltaTime / spawntime;

            alpha.a += -1 * (Time.deltaTime / spawntime);

            image.color = alpha;
            yield return null;
        }

        this.gameObject.SetActive(false);

        StopCoroutine("pade");
    }
}
