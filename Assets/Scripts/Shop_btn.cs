using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_btn : Event_button
{
    [SerializeField]
    private Player_stat _Pstat;
    [SerializeField]
    private int prise;
    private Button _btn;

    private void OnEnable()
    {
        _btn = this.GetComponent<Button>();

        if(_Pstat.Coin_num >= 10)
        {
            _btn.interactable = true;
        }
        else
        {
            _btn.interactable = false;
        }
    }

    public override void Play_Event()
    {
        _Pstat.Coin_num -= prise;
        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Get_events(0));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
