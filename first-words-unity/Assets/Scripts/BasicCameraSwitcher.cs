using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;
    private Spell switchCam;
    private Spell camNumber;

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
            camNumber.cast += SwitchToCamNumber;
        }
    }

    private void OnDisable()
    {
        switchCam.cast -= SwitchToNextCam;
        camNumber.cast -= SwitchToCamNumber;
    }

    private void SwitchToNextCam(SpellEventArgs args)
    {
        
    }

    private void SwitchToCamNumber(SpellEventArgs args)
    {

    }

}
