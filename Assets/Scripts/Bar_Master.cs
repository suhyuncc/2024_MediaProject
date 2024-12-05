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

    private bool curse = true;          //���� �뼺���� ��ư�� ���߱� ���� ����
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
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(3));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
            }
            else
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(4));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
            }

            //���� 2�� ��ư ����
            _evbtn.interactable = true;
        }
        else
        {
            base.Play_Event();

            //��������
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
