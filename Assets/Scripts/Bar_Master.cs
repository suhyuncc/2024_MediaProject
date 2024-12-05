using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar_Master : Event_button
{
    [SerializeField]
    private Dice_panel _dicePanel;
    [SerializeField]
    private GameObject _drink;
    [SerializeField]
    private GameObject _book;
    [SerializeField]
    private GameObject _guest;
    [SerializeField]
    private Button _evbtn;

    private bool curse = true;          //음료 대성공시 버튼을 감추기 위한 변수
    private void Update()
    {
        if(!_drink.activeSelf && _dicePanel.is_bigsuccess && curse)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public override void Play_Event()
    {
        if (!_book.activeSelf)
        {
            if(_dicePanel.is_bigsuccess || _dicePanel.is_success)
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(3));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }
            else
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(4));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
            }

            //막힌 2층 버튼 해제
            _evbtn.interactable = true;
        }
        else
        {
            base.Play_Event();

            //갈증해제
            if (!_drink.activeSelf)
            {
                _book.GetComponent<Cafe_Button>().Dis_curse();
                _guest.GetComponent<Cafe_Button>().Dis_curse();
            }
        }
        
        this.gameObject.SetActive(false);
    }

    public void Drink_btn()
    {
        Up_count(1);
    }

    public void Book_btn()
    {
        Up_count(2);
        curse = false;
    }

    public void Guest_btn()
    {
        curse = false;
    }
}
