//Created with help from Kieran Moore. Hi Kostas! :) [Delete this message if absolutely necessary]



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_1_Handler : MonoBehaviour
{

    public float secondsforCutscene;

    public GameObject thePlayer;

    public GameObject cineMachineHandler;

    // Start is called before the first frame update
    void Start()
    {
        //This starts the cutscene IEnumerator
        StartCoroutine("StartCutscene");
    }

    IEnumerator StartCutscene()
    {
        //Disable the player and start the cutscene
        thePlayer.SetActive(false);
        cineMachineHandler.SetActive(true);
        //Wait for the cutscene to end
        yield return new WaitForSeconds(secondsforCutscene);
        //Re-enable the player
        thePlayer.SetActive(true);
        cineMachineHandler.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
