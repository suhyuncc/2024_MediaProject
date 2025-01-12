using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player_stat : ScriptableObject
{
    public int P_int;       // �÷��̾� ����
    public int P_str;       // �÷��̾� �ٷ�
    public int P_dex;       // �÷��̾� ��ø
    public int P_luk;      // �÷��̾� ���
    public int Max_san;     // �ִ� san��ġ
    public int C_san;       // ���� san��ġ
    public int[] Item_list; // ������ ����Ʈ
    public int C_Item_Index = 0; //������ ����Ʈ�� ���� �ε���
    public int Coin_num = 0; //ī���� ���� ����
    public int Current_stage_num = 0; //���� �������� �������� ��ȣ
    public bool Is_menual = false;
    public bool Is_minimap = false;
}
