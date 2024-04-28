using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player_stat : ScriptableObject
{
    public string P_name;   // �÷��̾� �̸�
    public int P_age;       // �÷��̾� ����
    public string P_job;    // �÷��̾� ����
    public int P_int;       // �÷��̾� ����
    public int P_str;       // �÷��̾� �ٷ�
    public int P_dex;       // �÷��̾� ��ø
    public int Max_san;     // �ִ� san��ġ
    public int Max_hp;      // �ִ� hp��ġ
    public int C_san;       // ���� san��ġ
    public int C_hp;        // ���� hp��ġ
}
