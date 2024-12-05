using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EV_Odd : MonoBehaviour
{
    [SerializeField]
    private Button[] _btns;
    private void OnEnable()
    {
        for(int i = 0; i < _btns.Length; i++)
        {
            _btns[i].interactable = true;
        }

        switch ((int)GameManager.Instance.Current_stage)
        {
            case (int)stage.Store:
                _btns[0].interactable = false;
                break;

            case (int)stage.Light_Robby:
                _btns[1].interactable = false;
                break;

            case (int)stage.Cook:
                _btns[2].interactable = false;
                break;

            case (int)stage.Casino:
                _btns[3].interactable = false;
                break;
        }
    }
}
