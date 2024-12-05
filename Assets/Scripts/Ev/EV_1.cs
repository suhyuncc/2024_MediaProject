using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EV_1 : MonoBehaviour
{
    [SerializeField]
    private Button _btn;

    private void OnEnable()
    {
        switch ((int)GameManager.Instance.Current_stage)
        {

            case (int)stage.Light_Robby:
                _btn.interactable = false;
                break;

        }
    }
}
