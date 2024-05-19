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

    // �г��� ������ ���� ��µǴ� �ؽ�Ʈ�� ����
    private void OnEnable()
    {
        //â�� �������� �̹����� ���� �ؽ�Ʈ�� ���
        _showImage.gameObject.SetActive(false);
        _name.text = "";
        _discript.text = "";

        //���� ���
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
        _name.text = "�ٷ�";
        _discript.text = "���� ���õ� �̺�Ʈ���� ���̴� ����";
    }

    public void LUK_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[1];
        _name.text = "���";
        _discript.text = "��� ���õ� �̺�Ʈ���� ���̴� ����";
    }

    public void INT_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[2];
        _name.text = "����";
        _discript.text = "������ ���õ� �̺�Ʈ���� ���̴� ����";
    }

    public void DEX_button()
    {
        if (!_showImage.gameObject.activeSelf)
        {
            _showImage.gameObject.SetActive(true);
        }
        _showImage.sprite = _showImages[3];
        _name.text = "��ø";
        _discript.text = "��ø�� �ൿ�� ���õ� �̺�Ʈ���� ���̴� ����";
    }
}
