using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe_Button : Event_button
{
    [SerializeField]
    private GameObject _drink;
    [SerializeField]
    private Dice_panel _dicePanel;

    private bool curse = true;
    public override void Play_Event()
    {
        //���ᰡ �����ִٸ�
        if(!_drink.activeSelf && _dicePanel.is_success && curse)
        {
            //����Death����
            CSVParsingD.instance.Setcsv(Get_events(1));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //���� ��ũ��Ʈ ����
            CSVParsingD.instance.Setcsv(Get_events(0));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }

        GameManager.Instance.current_btn = this.gameObject;
    }

    public void Dis_curse()
    {
        curse = false;
    }
}
