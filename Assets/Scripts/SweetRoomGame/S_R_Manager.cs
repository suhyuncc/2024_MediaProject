using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_R_Manager : MonoBehaviour
{
    public static S_R_Manager instance;

    public bool is_over;

    [SerializeField]
    private GameObject s_r_game;
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private GameObject node_prefab;
    [SerializeField]
    private GameObject warning_text;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private RawImage pade;
    [SerializeField]
    private Texture[] background_images;
    [SerializeField]
    private GameObject monster_image;
    [SerializeField]
    private Sprite origin_image;
    [SerializeField]
    private Animator monster_anim;
    [SerializeField]
    private GameObject _finishPanel;

    public bool monster;

    public int count;
    private int stage_num;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        monster = false;
    }

    private void OnEnable()
    {
        //Start_stage(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (monster)
        {
            
            //warning_text.SetActive(true);
        }
        else
        {
            
            //warning_text.SetActive(false);
        }
    }

    IEnumerator stage1()
    {
        while (!is_over)
        {
            spawn();
            if (monster)
            {
                Debug.Log("해제!!");
                monster = false;
                monster_anim.SetTrigger("off");
            }

            float spawntime = Random.Range(1.5f, 2.0f);

            int ran = Random.Range(0, 10);

            if(ran < 4)
            {
                Debug.Log("경계!!");
                monster = true;
                monster_anim.SetTrigger("on");
            }
            yield return new WaitForSecondsRealtime(spawntime);
        }

        StopCoroutine("stage1");
    }

    IEnumerator stage2()
    {
        Debug.Log("스테이지2 시작!!");

        while (!is_over)
        {
            spawn2();
            if (monster)
            {
                monster = false;
                monster_anim.SetTrigger("off");
            }

            float spawntime = Random.Range(1.5f, 2.0f);

            int ran = Random.Range(0, 10);

            if (ran < 4)
            {
                monster = true;
                monster_anim.SetTrigger("on");
            }
            yield return new WaitForSecondsRealtime(spawntime);
        }

        StopCoroutine("stage2");
    }

    IEnumerator stage3()
    {
        Debug.Log("스테이지3 시작!!");

        int node_num = 0;

        while (!is_over && node_num < 30)
        {
            spawn3();
            node_num++;

            float spawntime = Random.Range(0.3f, 0.5f);

            yield return new WaitForSecondsRealtime(spawntime);
        }

        StopCoroutine("stage3");
    }

    private void spawn()
    {
        float x = Random.Range(-750.0f, 750.0f);
        float y = Random.Range(-365.0f, 365.0f);

        Vector3 Spos = new Vector3(x, y, 0);

        GameObject newObject = Instantiate(node_prefab, parent);

        newObject.transform.localPosition = Spos;
    }

    private void spawn2()
    {
        float x = Random.Range(-750.0f, 750.0f);
        float y = Random.Range(-365.0f, 365.0f);

        Vector3 Spos = new Vector3(x, y, 0);

        GameObject newObject = Instantiate(node_prefab, parent);

        int ran = Random.Range(0, 10);

        if (ran < 4)
        {
            newObject.GetComponent<Node>().is_fake = true;
        }

        newObject.transform.localPosition = Spos;
    }

    private void spawn3()
    {
        float x = Random.Range(-750.0f, 750.0f);
        float y = Random.Range(-365.0f, 365.0f);

        Vector3 Spos = new Vector3(x, y, 0);

        GameObject newObject = Instantiate(node_prefab, parent);

        //newObject.GetComponent<Node>().spawntime = 0.75f;
        newObject.transform.localPosition = Spos;
    }

    //클릭시 체크
    public void Click_Check()
    {
        switch (stage_num)
        {
            case 1:
            case 2:
                if (monster)
                {
                    is_over = true;
                    Debug.Log("클릭해서 죽음!!");
                    fail();
                }
                else
                {
                    count++;
                    Debug.Log("클릭 성공!!");

                    //스테이지 성공 조건
                    if (count == 10)
                    {
                        stage_num++;
                        change_stage(stage_num);
                    }
                    else
                    {
                        
                        Walk();
                    }
                }
                break;
            case 3:
                count++;
                Debug.Log("클릭 성공!!");

                //스테이지 성공 조건
                if (count == 30)
                {
                    is_over = true;
                    Debug.Log("클리어!!");
                    StopAllCoroutines();
                    _finishPanel.SetActive(true);
                }
                else
                {

                    Walk();
                }
                break;
        }
    }

    //클릭 못했을시 체크
    public void Non_Click_Check()
    {
        switch (stage_num)
        {
            case 1:
            case 2:
                if (!monster)
                {
                    is_over = true;
                    Debug.Log("클릭 안해서 죽음!!");
                    fail();
                }          
                break;
            case 3:
                is_over = true;
                Debug.Log("클릭 실패!!");
                fail();
                break;
        }
    }

    private void change_stage(int num)
    {
        is_over = true;

        StartCoroutine("padeInOut");

        switch (num)
        {
            case 2:
                StopCoroutine("stage1");
                break;
            case 3:
                StopCoroutine("stage2");
                break;
        }
    }

    public void Start_stage(int num)
    {
        StopAllCoroutines();

        monster = false;
        is_over = false;

        switch (num)
        {
            case 1:
                stage_num = 1;
                set_stage(1);
                StartCoroutine("stage1");
                break;
            case 2:
                stage_num = 2;

                set_stage(2);

                StartCoroutine("stage2");
                break;
            case 3:
                stage_num = 3;

                set_stage(3);

                StartCoroutine("stage3");
                break;
        }

        count = 0;
    }

    private void set_stage(int num)
    {
        switch (num)
        {
            case 1:
                monster_image.SetActive(true);

                monster_image.transform.localPosition = new Vector3(-100.0f, -27.0f, 0f);
                monster_image.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                //이미지 원상태로 변경
                monster_image.GetComponent<Animator>().enabled = false;
                monster_image.GetComponent<Image>().sprite = origin_image;
                monster_image.GetComponent<Animator>().enabled = true;
                

                background.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                background.GetComponent<RawImage>().texture = background_images[0];
                break;
            case 2:
                monster_image.transform.localPosition = new Vector3(-280.0f, -170.0f, 0f);
                monster_image.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                //이미지 원상태로 변경
                monster_image.GetComponent<Image>().sprite = origin_image;

                background.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                background.GetComponent<RawImage>().texture = background_images[1];

                break;
            case 3:
                monster_image.SetActive(false);

                background.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                background.GetComponent<RawImage>().texture = background_images[2];
                break;
        }
    }

    public void Walk()
    {
        
        switch (stage_num)
        {
            case 1:
            case 2:
                StartCoroutine("walk");
                break;
            case 3:
                StartCoroutine("walk3");
                break;
        }
    }

    public void fail()
    {
        Dialogue_Manage.Instance.Soft_On();
        
        s_r_game.SetActive(false);
    }

    IEnumerator walk()
    {
        float spawntime = 1.0f;
        float time = 0f;

        float increase_back_scale = 0.25f;
        float increase_monster_scale = 0.01f;

        Vector3 back_scale = background.transform.localScale;
        Vector3 back_pos = background.transform.localPosition;
        Vector3 monster_scale = monster_image.transform.localScale;

        int direction = 1;

        while (time < spawntime)
        {
            time += Time.deltaTime / spawntime;

            back_scale.x += increase_back_scale * (Time.deltaTime / spawntime);
            back_scale.y += increase_back_scale * (Time.deltaTime / spawntime);
            back_scale.z += increase_back_scale * (Time.deltaTime / spawntime);

            background.transform.localScale = back_scale;

            monster_scale.x += increase_monster_scale * (Time.deltaTime / spawntime);
            monster_scale.y += increase_monster_scale * (Time.deltaTime / spawntime);
            monster_scale.z += increase_monster_scale * (Time.deltaTime / spawntime);

            monster_image.transform.localScale = monster_scale;

            if (time >= (spawntime * 0.25f) && time < (spawntime * 0.5f) || time >= (spawntime * 0.75f))
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            back_pos.y += direction * 0.1f;

            background.transform.localPosition = back_pos;
            yield return null;
        }

        StopCoroutine("walk");

    }

    IEnumerator walk3()
    {
        float spawntime = 1.0f;
        float time = 0f;

        float increase_back_scale = 0.1f;
        float increase_monster_scale = 0.01f;

        Vector3 back_scale = background.transform.localScale;
        Vector3 back_pos = background.transform.localPosition;
        Vector3 monster_scale = monster_image.transform.localScale;

        int direction = 1;

        while (time < spawntime)
        {
            time += Time.deltaTime / spawntime;

            back_scale.x += increase_back_scale * (Time.deltaTime / spawntime);
            back_scale.y += increase_back_scale * (Time.deltaTime / spawntime);
            back_scale.z += increase_back_scale * (Time.deltaTime / spawntime);

            background.transform.localScale = back_scale;

            monster_scale.x += increase_monster_scale * (Time.deltaTime / spawntime);
            monster_scale.y += increase_monster_scale * (Time.deltaTime / spawntime);
            monster_scale.z += increase_monster_scale * (Time.deltaTime / spawntime);

            monster_image.transform.localScale = monster_scale;

            if (time >= (spawntime * 0.25f) && time < (spawntime * 0.5f) || time >= (spawntime * 0.75f))
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            back_pos.y += direction * 0.1f;

            background.transform.localPosition = back_pos;
            yield return null;
        }

        StopCoroutine("walk3");
    }

    IEnumerator padeInOut()
    {
        pade.gameObject.SetActive(true);

        float spawntime = 1.0f;
        float time = 0f;

        Color alpha = pade.color;

        int di = 1;

        while (time < spawntime)
        {
            time += Time.deltaTime / spawntime;

            if (time > spawntime / 2.0f)
            {
                di = -1;
            }

            alpha.a += 2.5f * di * (Time.deltaTime / spawntime);

            if (alpha.a > 0.95f)
            {
                set_stage(stage_num);
                
            }

            pade.color = alpha;
            yield return null;
        }

        pade.gameObject.SetActive(false);
        Start_stage(stage_num);

        StopCoroutine("padeInOut");
    }
}
