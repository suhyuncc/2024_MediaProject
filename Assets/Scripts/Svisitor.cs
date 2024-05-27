using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Svisitor : Event_button
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private GameObject _npc;
    [SerializeField]
    private GameObject _bell;

    private bool is_juice;
    private bool is_flug = true;
    public override void Play_Event()
    {
        //귀마개 판단
        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            if (P_stat.Item_list[i] == 6)
            {
                is_flug = false;
            }
        }

        if (is_flug && _npc.GetComponent<Event_button>().Get_count() == 2)
        {
            Set_count(2);
        }

        if (Get_count() == 1)
        {
            //주스 판단
            for(int i = 0; i < P_stat.Item_list.Length; i++)
            {
                if (P_stat.Item_list[i] == 4)
                {
                    is_juice = true;
                }
            }
            //음료가 있을 때
            if (is_juice)
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(1));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example"); 
                Up_count(1);
            }
            //음료가 없을 때
            else
            {
                //이벤트 다이얼로그 로드
                CSVParsingD.instance.Setcsv(Get_events(2));
                //다이얼로그 실행
                Dialogue_Manage.Instance.GetEventName("Example");
                Up_count(1);
            }

            is_juice = false;
        }
        else if (Get_count() == 2)
        {
            //이벤트 다이얼로그 로드
            CSVParsingD.instance.Setcsv(Get_events(3));
            //다이얼로그 실행
            Dialogue_Manage.Instance.GetEventName("Example");
        }
        else
        {
            //count가 1이 아니라면 1증가
            if (_npc.GetComponent<Event_button>().Get_count() != 1)
            {
                _npc.GetComponent<Event_button>().Up_count(1);
            }

            if (!_npc.activeSelf)
            {
                _npc.SetActive(true);
            }

            //count가 1이 아니라면 1증가
            if (_bell.GetComponent<Event_button>().Get_count() != 1)
            {
                _bell.GetComponent<Event_button>().Up_count(1);
            }

            if (!_bell.activeSelf)
            {
                _bell.SetActive(true);
            }

            base.Play_Event();
        }
        
    }
}
