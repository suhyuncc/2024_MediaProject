using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changer_btn : Event_button
{
    [SerializeField]
    private Player_stat P_stat;     //�÷��̾� ����

    public override void Play_Event()
    {
        //������ �Ǵ�
        if (P_stat.Coin_num > 100)
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(0));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
            CountFull();
        }
        else
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(1));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }

    }
}
