using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Data
{
    public int P_int;       // �÷��̾� ����
    public int P_str;       // �÷��̾� �ٷ�
    public int P_dex;       // �÷��̾� ��ø
    public int P_luk;      // �÷��̾� ���
    public int Max_san;     // �ִ� san��ġ
    public int C_san;       // ���� san��ġ
    public List<int> Item_list = new List<int>(); // ������ ����Ʈ
    public int C_Item_Index = 0; //������ ����Ʈ�� ���� �ε���
    public int Coin_num = 0; //ī���� ���� ����
    public int Current_stage_num = 0; //���� �������� �������� ��ȣ
}
