using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_info_panel : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;

    [SerializeField]
    private Text INT;
    [SerializeField]
    private Text STR;
    [SerializeField]
    private Text DEX;
    [SerializeField]
    private Text SAN;
    [SerializeField]
    private Text LUK;

    // �г��� ������ ���� ��µǴ� �ؽ�Ʈ�� ����
    private void OnEnable()
    {
        INT.text = $"����: {P_stat.P_int}";
        STR.text = $"�ٷ�: {P_stat.P_str}";
        DEX.text = $"��ø: {P_stat.P_dex}";
        SAN.text = $"����: {P_stat.C_san} / {P_stat.Max_san}";
        LUK.text = $"���: {P_stat.P_luk}";
    }
}
