using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;
    private int currentCamPositionIndex = 0;
    private Spell switchCam;

    private void Start()
    {
        SetToPositionIndex(currentCamPositionIndex);
    }

    private void OnEnable()
    {
        //REVIEW
        /* what if, i make a separate util class or smth with a coroutine (could also be a multithreaded solution) that takes the spell 
           that i'm trying to subscribe to and the method i want to subscribe with, waits for spell caching to finish, and then creates 
           the connection? */

        switchCam = SessionSpellCache.GetSpell(SpellWords.Switch_Cam);

        if(switchCam != null)
        {
            switchCam.cast += SwitchToNextCam;
        }
    }

    private void OnDisable()
    {
        switchCam.cast -= SwitchToNextCam;
    }

    private void SwitchToNextCam(SpellEventArgs args)
    {
        if(currentCamPositionIndex + 1 < camPositions.Count)
        {
            currentCamPositionIndex++;
        }
        else
        {
            currentCamPositionIndex = 0;
        }

        SetToPositionIndex(currentCamPositionIndex);
    }

    private void SetToPositionIndex(int index)
    {
        transform.position = camPositions[index].position;
        transform.rotation = camPositions[index].rotation;
    }

}
