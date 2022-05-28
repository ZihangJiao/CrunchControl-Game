using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseDisplayer : MonoBehaviour
{

    public Text phaseText;
    // Start is called before the first frame update
    void Start()
    {
        CardBattle.Instance.phaseChangeEvent.AddListener(updateText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void updateText()
    {
        phaseText.text = "Turn " + CardBattle.Instance.TurnCounter; // + " " + CardBattle.Instance.GamePhase.ToString();
    }
}
