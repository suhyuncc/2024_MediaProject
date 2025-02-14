using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    standby,
    main,
    dice,
    result
}

public class CardGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cards;
    [SerializeField]
    private Transform[] card_positions;

    private Phase phase;

    // Start is called before the first frame update
    void Start()
    {
        phase = Phase.standby;
    }

    // Update is called once per frame
    void Update()
    {
        cards[0].transform.localPosition = Vector3.MoveTowards(cards[0].transform.localPosition,
                card_positions[0].localPosition, 0.3f * Time.fixedDeltaTime);

        if (phase == Phase.standby)
        {
            //Set();
            phase = Phase.main;
        }
    }

    private void Set()
    {
        StartCoroutine("card_set");
    }

    IEnumerator card_set()
    {
        int target_index = 0;

        foreach(var card in cards)
        {
            

            while (card.transform.localPosition.x < card_positions[target_index].localPosition.x)
            {
                Debug.Log($"target_index: {target_index}");

                card.transform.localPosition = Vector3.MoveTowards(card.transform.localPosition,
                card_positions[target_index].localPosition, 0.3f * Time.fixedDeltaTime);

                Debug.Log($"{card.transform.localPosition}");
                Debug.Log($"{card_positions[target_index].localPosition}");

                yield return new WaitForSecondsRealtime(0.3f);
            }

            

            target_index++;
        }
    }
}
