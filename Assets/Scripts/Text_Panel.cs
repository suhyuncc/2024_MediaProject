using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Panel : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttons;
    [SerializeField]
    private TextAsset[] _sirenCSV; //수영장 세이렌 이벤트

    private void OnEnable()
    {
        _buttons.SetActive(false);
    }

    private void OnDisable()
    {
        _buttons.SetActive(true);
        if(GameManager.Instance.Siren_count == 1 || GameManager.Instance.Siren_count == 2 || GameManager.Instance.Siren_count == 3)
        {
            CSVParsingD.instance.Setcsv(_sirenCSV[GameManager.Instance.Siren_count]);
            GameManager.Instance.Dialogue_Start();
        }
    }
}
