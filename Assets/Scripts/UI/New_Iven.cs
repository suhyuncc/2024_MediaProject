using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class New_Iven : MonoBehaviour
{
    [SerializeField]
    private Player_stat P_stat;
    [SerializeField]
    private Discriptions _discriptions;
    [SerializeField]
    private Button[] _itemSlot;
    [SerializeField]
    private Text _itemDiscrip;
    [SerializeField]
    private Text[] _itemName;

    private void OnEnable()
    {
        _itemDiscrip.text = "";

        for (int i = 0; i < P_stat.Item_list.Length; i++)
        {
            int item_id = P_stat.Item_list[i];

            if (item_id == -1)
            {
                _itemName[i].text = "";
                _itemSlot[i].interactable = false;
            }
            else if (item_id == 2)
            {
                _itemName[i].text = $"{_discriptions.Item_Names[item_id]} X{P_stat.Coin_num}";
                _itemSlot[i].interactable = true;
            }
            else
            {
                _itemName[i].text = $"{_discriptions.Item_Names[item_id]}";
                _itemSlot[i].interactable = true;
            }
        }
    }

    public void Show_Discript(int btn_num)
    {
        int item_id = P_stat.Item_list[btn_num];

        _itemDiscrip.text = _discriptions.Item_Discriptions[item_id];
    }
}
