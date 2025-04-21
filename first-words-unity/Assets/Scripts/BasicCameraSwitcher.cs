using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;
    private int currentCamPositionIndex = 0;
    private Spell switchCam;
    private Spell camNumber;

    private void Start()
    {
        SetToPositionIndex(currentCamPositionIndex);
    }

    private void OnEnable()
    {
        switchCam = SessionSpellCache.GetSpell(SpellWords.SwitchCam);

        if(switchCam != null)
        {
            switchCam.cast += SwitchToNextCam;
        }
        
        camNumber = SessionSpellCache.GetSpell(SpellWords.CamNumber);

        if(camNumber != null)
        {
            // camNumber.cast += SwitchToCamNumber;
        }
    }

    private void OnDisable()
    {
        switchCam.cast -= SwitchToNextCam;
        // camNumber.cast -= SwitchToCamNumber;
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

    private void SwitchToCamNumber(SpellEventArgs args)
    {

    }

    private void SetToPositionIndex(int index)
    {
        transform.position = camPositions[index].position;
        transform.rotation = camPositions[index].rotation;
    }

}
