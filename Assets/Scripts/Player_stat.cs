using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player_stat : ScriptableObject
{
    public string P_name;   // 플레이어 이름
    public int P_age;       // 플레이어 나이
    public string P_job;    // 플레이어 직업
    public int P_int;       // 플레이어 지능
    public int P_str;       // 플레이어 근력
    public int P_dex;       // 플레이어 민첩
    public int Max_san;     // 최대 san수치
    public int Max_hp;      // 최대 hp수치
    public int C_san;       // 현재 san수치
    public int C_hp;        // 현재 hp수치
}
