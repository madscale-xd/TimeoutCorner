using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAdd : MonoBehaviour
{
    private TimerScript timerScript;
    // Start is called before the first frame update
    void Start()
    {
        GameObject timerObject = GameObject.Find("TimerUI");
        timerScript = timerObject.GetComponent<TimerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TimerScript.GetStartTime()==21) 
        {
            timerScript.AddTime(1f);
            timerScript.StopTimer();
        }
    }
}
