using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_Master : Event_button
{
    [SerializeField]
    private Dice_panel _dicePanel;
    [SerializeField]
    private GameObject _drink;
    [SerializeField]
    private GameObject _book;

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
        }
        else
        {
            base.Play_Event();
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
    }
}