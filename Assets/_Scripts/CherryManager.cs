using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryManager : MonoBehaviour
{
    Text batteryTextCount;

    // Start is called before the first frame update
    void Start()
    {
        batteryTextCount = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        batteryTextCount.text = PlayerManager.currentCherryCount.ToString();
    }
}
