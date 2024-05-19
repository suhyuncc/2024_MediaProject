using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_info_panel : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;

    [SerializeField]
    private Image _showImage;
    [SerializeField]
    private Sprite[] _showImages;
    [SerializeField]
    private Text _name; 
    [SerializeField]
    private Text _discript;

    [SerializeField]
    private Text INT;
    [SerializeField]
    private Text STR;
    [SerializeField]
    private Text DEX;
    [SerializeField]
    private Text LUK;

    // 패널이 켜질때 마다 출력되는 텍스트를 지정
    private void OnEnable()
    {
        //창이 켜졌을때 이미지를 끄고 텍스트를 비움
        _showImage.gameObject.SetActive(false);
        _name.text = "";
        _discript.text = "";

        //스탯 출력
        INT.text = $"{P_stat.P_int}";
        STR.text = $"{P_stat.P_str}";
        DEX.text = $"{P_stat.P_dex}";
        LUK.text = $"{P_stat.P_luk}";
    }

    public void STR_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[0];
        _name.text = "근력";
        _discript.text = "힘과 관련된 이벤트에서 쓰이는 스탯";
    }

    public void LUK_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[1];
        _name.text = "행운";
        _discript.text = "운과 관련된 이벤트에서 쓰이는 스탯";
    }

    public void INT_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[2];
        _name.text = "지능";
        _discript.text = "관찰과 관련된 이벤트에서 쓰이는 스탯";
    }

    public void DEX_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[3];
        _name.text = "민첩";
        _discript.text = "민첩한 행동과 관련된 이벤트에서 쓰이는 스탯";
    }
}
