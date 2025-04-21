using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Management : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UI_panels;

    private GameObject _currentPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentPanel == null)
            {
                _currentPanel = UI_panels[0];
                _currentPanel.SetActive(true);
            }
            else
            {
                Off_Panel();
            }
        }
    }

    public void Switch_panel(int panel_id)
    {
        if(_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }

        _currentPanel = UI_panels[panel_id];
        _currentPanel.SetActive(true);
    }

    public void Off_Panel()
    {
        _currentPanel.SetActive(false);
        _currentPanel = null;
    }
}
