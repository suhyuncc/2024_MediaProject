using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweatRoomBtn : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;     //플레이어 정보
    [SerializeField]
    private Button _btn;

    private bool _first = true;

    private void OnEnable()
    {
        if ((int)GameManager.Instance.Current_stage == 11 && _first)
        {
            _first = false;
        }

        if (_first)
        {
            //아이템 판단
            for (int i = 0; i < P_stat.Item_list.Length; i++)
            {
                //카드키
                if (P_stat.Item_list[i] == 13)
                {
                    _btn.interactable = true;
                    return;
                }

            }
        }
    }
}
