using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_changer : Event_button
{
    [SerializeField]
    private Player_stat P_stat;

    public override void Play_Event()
    {
        if(Get_count() == 0)
        {
            base.Play_Event();
        }
        else
        {
            if(P_stat.Coin_num >= 100)
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(1));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
            }
            else
            {
                //�̺�Ʈ ���̾�α� �ε�
                CSVParsingD.instance.Setcsv(Get_events(2));
                //���̾�α� ����
                Dialogue_Manage.Instance.GetEventName("Example");
            }
        }
    }
}
