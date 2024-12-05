using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDiscript : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private Player_stat _playerStat;
    [SerializeField]
    private Discriptions _discriptions;
    [SerializeField]
    private GameObject _disPanel;
    [SerializeField]
    private Text _itemName;
    [SerializeField]
    private Text _disTxt;
    public int Item_num;
    private bool mouseOn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        int item = _playerStat.Item_list[Item_num];
        mouseOn = true;

        if(item != -1)
        {
            _disPanel.SetActive(true);
            _disTxt.text = _discriptions.Item_Discriptions[item];
            _itemName.text = _discriptions.Item_Names[item];
        }
        
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOn = false;
        _disPanel.SetActive(false);
    }

    private void Update()
    {
        if (mouseOn)
        {
            _disPanel.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition + new Vector3(-960f, -540f, 0)
                + new Vector3(150f, -150f, 0) + new Vector3(20f, -20f, 0);
        }

    }
}
