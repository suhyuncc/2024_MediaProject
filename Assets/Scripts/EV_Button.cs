using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_Button : Event_button
{
    public override void Play_Event()
    {
        //�̺�Ʈ ���̾�α� �ε�
        CSVParsingD.instance.Setcsv(Get_events(0));
        //���̾�α� ����
        Dialogue_Manage.Instance.GetEventName("Example");
    }
}
