using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int card_id;
    public int card_index;

    [SerializeField]
    private Sprite[] card_images;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationClip[] clip;
    public bool is_back;
    [SerializeField]
    private bool is_player;
    [SerializeField]
    private bool is_hand;
    [SerializeField]
    private Image image;

    public bool On_card;

    private Vector3 mousePosition;
    [SerializeField]
    private Vector3 InitPosition;
    private Vector3 InitRotation;
    private Vector3 InitScale;

    private bool on_drag;

    public bool is_empty;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private AudioClip flip_sfx;


    private void OnEnable()
    {
        InitPosition = this.GetComponent<RectTransform>().anchoredPosition;
        InitRotation = this.GetComponent<RectTransform>().eulerAngles;
        InitScale = this.GetComponent<RectTransform>().localScale;

        if (is_hand)
        {
            Flip();
        }
        
    }

    private void Update()
    {
        if (On_card)
        {
            image.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            image.color = new Color(1f, 1f, 1f);
        }
    }

    public void Card_reset()
    {
        card_id = 0;
        is_back = true;
        image.sprite = card_images[3];


        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y);
        this.GetComponent<RectTransform>().eulerAngles = InitRotation;
        this.GetComponent<RectTransform>().localScale = InitScale;

        

        if (is_player)
        {
            transform.SetSiblingIndex(7);
        }
    }

    public void Flip()
    {
        if (animator.enabled == false)
        {
            animator.enabled = true;
        }

        if (!animator.GetBool("flip"))
        {
            animator.SetBool("flip", true);
            if(GameManager.Instance != null)
            {
                GameManager.Instance.Change_SFX(flip_sfx);
            }
            
            StartCoroutine(Wait(0.6f));
        }

        
    }

    public void End_Flip()
    {
        animator.SetBool("flip",false);
    }

    public void Hand()
    {
        if (animator.enabled == false)
        {
            animator.enabled = true;
        }

        if (!animator.GetBool("hand"))
        {
            animator.SetBool("hand", true);

            StartCoroutine(hand(1.0f));
        }

    }

    public void End_Hand()
    {
        animator.SetBool("hand", false);
    }

    public void Battle(float during)
    {
        
        

        StartCoroutine(battle(during));
    }

    public void OFF_collider()
    {
        if (is_player)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void ON_collider()
    {
        if (is_player)
        {
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private Vector3 GetmousePos()
    {
        Vector3 re = Camera.main.WorldToScreenPoint(transform.position);
        return new Vector3(re.x, re.y, 0);
    }

    private void OnMouseDown()
    {
        //on_drag = true;
        mousePosition = Input.mousePosition - GetmousePos();
    }

    private void OnMouseDrag()
    {
        on_drag = true;

        //이미지 레이어 최상단으로 이동
        transform.SetAsLastSibling();

        //이미지 이동
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2((Input.mousePosition.x - 960.0f) - mousePosition.x,
            (Input.mousePosition.y - 540.0f) - mousePosition.y);

        this.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
    }

    private void OnMouseUp()
    {
        
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y);
        this.GetComponent<RectTransform>().eulerAngles = InitRotation;

        if (target != null && on_drag)
        {
            if(this.CompareTag("Card") && target.CompareTag("Card"))
            {

                int temp;

                //타겟의 이미지 정보 저장
                temp = target.GetComponent<Card>().card_id;

                //타겟의 이미지를 드래그하고있는 카드의 이미지로 변경
                target.GetComponent<Card>().Change_Image(card_id);

                //타겟의 카드 유무 체크
                if (card_id != 3)
                {
                    target.GetComponent<Card>().is_empty = false;
                }
                else
                {
                    target.GetComponent<Card>().is_empty = true;
                }

                //타겟의 이미지로 자신의 이미지를 변경
                Change_Image(temp);

                //자신의 카드 유무 체크
                if(temp != 3)
                {
                    is_empty = false;
                }
                else
                {
                    is_empty = true;
                }

            }
            else
            {
                if (target.GetComponent<Card>().is_empty)
                {
                    int temp;

                    //타겟의 이미지 정보 저장
                    temp = target.GetComponent<Card>().card_id;

                    //타겟의 이미지를 드래그하고있는 카드의 이미지로 변경
                    target.GetComponent<Card>().Change_Image(card_id);

                    //타겟의 이미지로 자신의 이미지를 변경
                    Change_Image(temp);

                    target.GetComponent<Card>().is_empty = false;

                    this.gameObject.SetActive(false);
                }
                
            }
             

            target = null;
            on_drag = false;
        }
    }

    private void OnMouseOver()
    {
        if (!on_drag)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y + 20.0f);
        }
            
    }

    private void OnMouseExit()
    {
        
        if (!on_drag)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (on_drag && collision.gameObject.CompareTag("Card"))
        {
            if(target == null)
            {
                target = collision.gameObject;
                target.GetComponent<Card>().On_card = true;
            }
            else
            {
                target.GetComponent<Card>().On_card = false;
                target = collision.gameObject;
                target.GetComponent<Card>().On_card = true;
            }
            
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (on_drag && collision.gameObject.CompareTag("Card") && (target == null))
        {
            target = collision.gameObject;
            target.GetComponent<Card>().On_card = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(on_drag && (target != null))
        {
            target = null;
        }
        On_card = false;
    }

    IEnumerator Wait(float delay)
    { 
        yield return new WaitForSecondsRealtime(delay/1.2f);

        if (is_back)
        {
            //int ran = Random.Range(0, card_images.Length);

            //card_id = ran;

            image.sprite = card_images[card_id];
            is_back = false;
        }
        else
        {
            image.sprite = card_images[3];
            is_back = true;
        }

        

        yield return new WaitForSecondsRealtime(delay - (delay / 1.2f));

        animator.enabled = false;

        StopCoroutine(Wait(delay));
    }

    IEnumerator hand(float delay)
    {

        yield return new WaitForSecondsRealtime(delay);

        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y);
        this.GetComponent<RectTransform>().eulerAngles = InitRotation;
        this.GetComponent<RectTransform>().localScale = InitScale;

        this.gameObject.SetActive(false);

        StopCoroutine(hand(delay));
    }

    IEnumerator battle(float during)
    {
        float time = 0f;

        Vector2 init_pos = this.GetComponent<RectTransform>().anchoredPosition;
        Vector2 new_pos = init_pos;

        while (time < during)
        {
            time += Time.deltaTime / during;

            if (is_player)
            {
                //플레이어 카드라면 위로 이동
                new_pos.y = Mathf.Lerp(init_pos.y, init_pos.y + 40.0f, time);
            }
            else
            {
                //딜러라면 아래로 이동
                new_pos.y = Mathf.Lerp(init_pos.y, init_pos.y - 40.0f, time);
            }

            this.GetComponent<RectTransform>().anchoredPosition = new_pos;
            yield return null;
        }

        StopCoroutine(battle(during));
    }

    public void Change_Image(int index)
    {
        card_id = index;
        image.sprite = card_images[card_id];
    }

    public void Return_position()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(InitPosition.x, InitPosition.y);
        this.GetComponent<RectTransform>().eulerAngles = InitRotation;
    }
}
