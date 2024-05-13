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

    // 패널이 켜질때 마다 출력되는 텍스트를 지정
    private void OnEnable()
    {
        INT.text = $"지능: {P_stat.P_int}";
        STR.text = $"근력: {P_stat.P_str}";
        DEX.text = $"민첩: {P_stat.P_dex}";
        SAN.text = $"정신: {P_stat.C_san} / {P_stat.Max_san}";
        LUK.text = $"행운: {P_stat.P_luk}";
    }
}
