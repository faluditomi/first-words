using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;
    private int currentCamPositionIndex = 0;

    private void Start()
    {
        SetToPositionIndex(currentCamPositionIndex);
    }

    private void OnEnable()
    {
        SpellEventSubscriber.Instance.SubscribeToSpell(SpellWords.Switch_Cam, SwitchToNextCam);
    }

    // private void OnDisable()
    // {
    //     SpellEventSubscriber.Instance.UnsubscribeFromSpell(SpellWords.Switch_Cam, SwitchToNextCam);
    // }

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
