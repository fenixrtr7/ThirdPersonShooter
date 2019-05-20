using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    RawImage minimap;
    // Start is called before the first frame update
    void Start()
    {
        minimap = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            //this.gameObject.SetActive(!this.gameObject.activeInHierarchy); ==>> OPCION
            minimap.enabled = !minimap.enabled;
        }
    }
}
