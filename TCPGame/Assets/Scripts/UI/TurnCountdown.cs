using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnCountdown : MonoBehaviour
{
    [SerializeField]
    TMP_Text CountdownText;

    bool PieceWasSelected = false;

    int count = 15;

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while(!PieceWasSelected)
        {
            if(count == -1)
            {
                // EndTurn
                yield break;
            }
            CountdownText.text = "" + count;
            count--;
            yield return new WaitForSeconds(1f);
        }

        PieceWasSelected = false;
    }

    public void ResetCountdown()
    {
        count = 15;
    }

    public void PlayerSelectedPiece()
    {
        PieceWasSelected = true;
    }
}
