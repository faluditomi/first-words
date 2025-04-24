using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;
    private int currentCamPositionIndex = 0;
    private Spell<SpellArgs> switchCam;

    private void Start()
    {
        SetToPositionIndex(currentCamPositionIndex);
    }

    private void OnEnable()
    {
        SpellEventSubscriber.Instance.SubscribeToSpell<SpellArgs>(SpellWords.Switch_Cam, SwitchToNextCam, (spell) => switchCam = spell);
    }

    private void OnDisable()
    {
        switchCam.cast -= SwitchToNextCam;
    }

    private void SwitchToNextCam(SpellArgs args)
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
