using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino_Btn : Event_button
{
    public override void Play_Event()
    {
        if (Get_count() == 0)
        {
            base.Play_Event();
        }
        else 
        {
            //�̺�Ʈ ���̾�α� �ε�
            CSVParsingD.instance.Setcsv(Get_events(Get_count()));
            //���̾�α� ����
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        
    }
}
