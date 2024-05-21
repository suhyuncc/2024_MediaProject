using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private Camera _diceCam;
    [SerializeField]
    private GameObject _dicePanel;

    [SerializeField]
    private Dialogue_Manage DM;

    [SerializeField]
    private Text _sanText;

    private Camera _CurrentCam;//현재 카메라

    // Start is called before the first frame update
    void Start()
    {
        _CurrentCam = _mainCam;
    }

    // Update is called once per frame
    void Update()
    {
        _sanText.text = $"{P_stat.C_san} / {P_stat.Max_san}";
    }

    public void Dice_On()
    {
        Cam_switch(_CurrentCam);
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
        DM.GetEventName("Example");
    }
}
