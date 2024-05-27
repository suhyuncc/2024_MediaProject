using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Panel : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttons;
    

    private void OnEnable()
    {
        _buttons.SetActive(false);

    }

    private void OnDisable()
    {
        _buttons.SetActive(true);

        //현 스테이지 버튼 켜기
        //GameManager.Instance.Stage_Btn_On();

        
    }
}
