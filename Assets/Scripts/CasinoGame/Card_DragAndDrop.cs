using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Element
{
    Null = 0,
    Air = 1,
    Earth = 2,
    Water = 3,
    Fire = 4,
}

public class Card_DragAndDrop : MonoBehaviour
{
    private Vector3 mousePosition;
    [SerializeField]
    private Vector3 InitPosition;


    public bool Onspell;

    private void OnEnable()
    {
        InitPosition = this.GetComponent<RectTransform>().anchoredPosition;
        

    }

    private void OnDisable()
    {        

        
    }

    private void Awake()
    {
        Onspell = false;
    }

    private Vector3 GetmousePos()
    {
        Vector3 re = Camera.main.WorldToScreenPoint(transform.position);
        return new Vector3(re.x, re.y, 0);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetmousePos();
    }

    private void OnMouseDrag()
    {
        //이미지 레이어 최상단으로 이동
        transform.SetAsLastSibling();

        //이미지 이동
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2((Input.mousePosition.x - 960.0f) - mousePosition.x,
            (Input.mousePosition.y - 540.0f) - mousePosition.y);
    }

    private void OnMouseUp()
    {
        this.GetComponent<RectTransform>().anchoredPosition = InitPosition;

    }

    private void OnTriggerExit(Collider other)
    {
        //landPosition = InitPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Card"))
        {
            Onspell = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Onspell = false;
    }

    public void ReturnPos()
    {
        transform.rotation = Quaternion.identity;
        transform.position = InitPosition;
    }
}
