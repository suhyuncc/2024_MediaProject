using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Image outline;
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private Text txt;

    public bool is_fake;
    public float spawntime = 1.0f;

    private Image node;

    private void Update()
    {
        if(is_fake)
        {
            txt.text = "¶Ù±â";
        }
        else
        {
            txt.text = "°È±â";
        }
    }

    private void OnEnable()
    {
        StartCoroutine("count");
    }

    IEnumerator count()
    {
        
        float time = 0f;

        while (time < spawntime)
        {
            time += Time.deltaTime / spawntime;
            outline.fillAmount = (1 / spawntime) * time;
            yield return null;
        }

        if (!is_fake)
        {
            S_R_Manager.instance.Non_Click_Check();
        }
        
        Destroy(this.gameObject);

        StopCoroutine("count");
        
    }

    public void Click()
    {
        
        GameManager.Instance.Change_SFX(clip);
        Destroy(this.gameObject);
        if(is_fake)
        {
            S_R_Manager.instance.is_over = true;
            S_R_Manager.instance.fail();
            Debug.Log("Å¬¸¯ ½ÇÆÐ!!");
        }
        else
        {
            S_R_Manager.instance.Click_Check();
        }
    }

}
