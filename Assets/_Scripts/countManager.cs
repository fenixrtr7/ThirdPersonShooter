using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countManager : MonoBehaviour
{
    public string m_Tag = "Battery";
    public string textTag = "Batteries: ";
    public int currentBatteryCount = 0;
    Text batteryTextCount;
    public GameObject[] battery;

    // Start is called before the first frame update
    void Start()
    {
        batteryTextCount = GetComponent<Text>();
        currentBatteryCount = 1;
        RecalculateBeetles();
    }

    // Update is called once per frame
    public void RecalculateBeetles()
    {
        battery = GameObject.FindGameObjectsWithTag(m_Tag);
        currentBatteryCount = battery.Length;
        batteryTextCount.text = textTag + currentBatteryCount.ToString();
    }
}
