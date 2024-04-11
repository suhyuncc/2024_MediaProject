using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InnerDriveStudios.DiceCreator;

public class Die_print : MonoBehaviour
{
    [SerializeField]
    private ARollable die;

    public Text R_txt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Print_result();
    }

    void Print_result()
    {
        IRollResult result = die.GetRollResult();

        string resultColor = (die.HasEndResult() && result.isExact) ? "green" : "blue";
        //only show values if we are NOT rolling OR updating every frame
        string resultValues =
            (!die.isRolling) ? result.valuesAsString : "*";

        R_txt.text = string.Format("<color={0}>{1} = {2}</color>", resultColor, die.name, resultValues);
    }
}
