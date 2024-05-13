using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player_stat : ScriptableObject
{
    public int P_int;       // 플레이어 지능
    public int P_str;       // 플레이어 근력
    public int P_dex;       // 플레이어 민첩
    public int P_luk;      // 플레이어 행운
    public int Max_san;     // 최대 san수치
    public int C_san;       // 현재 san수치
}
