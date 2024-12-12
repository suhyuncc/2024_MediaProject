using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Data
{
    public int P_int;       // 플레이어 지능
    public int P_str;       // 플레이어 근력
    public int P_dex;       // 플레이어 민첩
    public int P_luk;      // 플레이어 행운
    public int Max_san;     // 최대 san수치
    public int C_san;       // 현재 san수치
    public List<int> Item_list = new List<int>(); // 아이템 리스트
    public int C_Item_Index = 0; //아이템 리스트의 현재 인덱스
    public int Coin_num = 0; //카지노 코인 개수
    public int Current_stage_num = 0; //현재 진행중인 스테이지 번호
}
