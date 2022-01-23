using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PingDistanceHandler : MonoBehaviour
{
    private TextMeshPro distanceText;

    private void Awake()
    {
        distanceText = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        Vector3 pingPosition = transform.parent.position;
        Vector3 playerPosition = PlayerCharacter.GetPosition();
        int distance = Mathf.RoundToInt(Vector3.Distance(pingPosition, playerPosition) / 3f);
        distanceText.text = distance + "M";
    }
}
