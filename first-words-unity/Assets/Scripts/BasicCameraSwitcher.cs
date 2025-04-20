using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwitcher : MonoBehaviour
{

    [SerializeField] private List<Transform> camPositions;

    // private void OnEnable()
    // {
    //     Player.OnPlayerDied += HandlePlayerDeath;
    // }

    // private void OnDisable()
    // {
    //     Player.OnPlayerDied -= HandlePlayerDeath;
    // }

    // private void HandlePlayerDeath()
    // {
    //     Debug.Log("Player has died. Game over!");
    // }

}
