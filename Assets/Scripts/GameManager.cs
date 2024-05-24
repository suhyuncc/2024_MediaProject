using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Core Objects")]
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private GameObject[] _dices; //주사위들

    [Header("UI")]
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private Camera _diceCam;
    [SerializeField]
    private GameObject _dicePanel;
    [SerializeField]
    private GameObject[] _buttons;

    private Camera _CurrentCam;//현재 카메라

    // Start is called before the first frame update
    void Start()
    {
        _CurrentCam = _mainCam;

        //주사위 오브젝트 숨기기
        for(int i = 0; i < _dices.Length; i++)
        {
            _dices[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Button_off();
        }
        Background_change();
    }

    public void Dice_On()
    {
        Cam_switch(_CurrentCam);
        Dice_setting("1d6");
    }

    private void Cam_switch(Camera cam)
    {

        if (cam.Equals(_mainCam))
        {
            _mainCam.gameObject.SetActive(false);
            _diceCam.gameObject.SetActive(true);
            _dicePanel.SetActive(true);
            _CurrentCam = _diceCam;
            _canvas.worldCamera = _CurrentCam;
        }
        else if (cam.Equals(_diceCam))
        {
            _mainCam.gameObject.SetActive(true);
            _diceCam.gameObject.SetActive(false);
            _dicePanel.SetActive(false);
            _CurrentCam = _mainCam;
            _canvas.worldCamera = _CurrentCam;
        }
        
    }

    public void GetTXT()
    {
        Dialogue_Manage.Instance.GetEventName("Example");
    }

    private void Background_change()
    {
        

        //이 아래는 추후 switch로 재작성
        //RedRoom
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _buttons[0].SetActive(true);
            Dialogue_Manage.Instance.Change_back(4);
        }
        //Cafe
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _buttons[1].SetActive(true);
            Dialogue_Manage.Instance.Change_back(6);
        }
        //RedSwim
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _buttons[2].SetActive(true);
            Dialogue_Manage.Instance.Change_back(7);
        }
        //Dark_Robby
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _buttons[3].SetActive(true);
            Dialogue_Manage.Instance.Change_back(8);
        }
        //B2
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _buttons[4].SetActive(true);
            Dialogue_Manage.Instance.Change_back(9);
        }
        //Light_Robby
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _buttons[5].SetActive(true);
            Dialogue_Manage.Instance.Change_back(1);
        }
        //Casino
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _buttons[6].SetActive(true);
            Dialogue_Manage.Instance.Change_back(12);
        }
        //Party
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _buttons[7].SetActive(true);
            Dialogue_Manage.Instance.Change_back(13);
        }
        //Store
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _buttons[8].SetActive(true);
            Dialogue_Manage.Instance.Change_back(14);
        }
    }

    //켜저있는 버튼 끄기
    private void Button_off()
    {
        for(int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].activeSelf)
            {
                _buttons[i].SetActive(false);
            }
        }
    }

    //켜저있는 주사위 끄기
    private void Dice_off()
    {
        for (int i = 0; i < _dices.Length; i++)
        {
            if (_dices[i].activeSelf)
            {
                _dices[i].SetActive(false);
            }
        }
    }

    public void Dice_setting(string dice)
    {
        Dice_off();

        switch (dice)
        {
            case "1d4":
                _dices[0].SetActive(true);
                break;

            case "1d6":
                _dices[1].SetActive(true);
                break;

            case "2d6":
                _dices[1].SetActive(true);
                _dices[2].SetActive(true);
                break;
            case "3d6":
                _dices[1].SetActive(true);
                _dices[2].SetActive(true);
                _dices[3].SetActive(true);
                break;
            case "1d100":
                _dices[4].SetActive(true);
                _dices[5].SetActive(true);
                break;
        }
    }
}
