using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    Text victoryText;
    countManager enemy, battery;
    // Start is called before the first frame update
    void Start()
    {
        victoryText = GetComponent<Text>();
        //victoryText.text = "";
        victoryText.enabled = false;
        enemy = GameObject.Find("Text Beetles").GetComponent<countManager>();
        battery = GameObject.Find("Text Baterias").GetComponent<countManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.currentBatteryCount <= 0)
        {
            victoryText.text = "You win!!";
            victoryText.enabled = true;
        }

        if(battery.currentBatteryCount <= 0 || PlayerManager.livesRemaining <= 0)
        {
            victoryText.text = "You lose!!";
            victoryText.enabled = true;
        }

    }
}
