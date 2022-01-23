using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PingUIHandler : MonoBehaviour
{
    private PingSystem.Ping ping;
    private RectTransform rectTransform;
    private Image image;
    private TextMeshProUGUI distanceText;

    public void Setup(PingSystem.Ping ping)
    {
        this.ping = ping;
        rectTransform = transform.GetComponent<RectTransform>();
        image = transform.GetComponent<Image>();
        distanceText = transform.Find("distanceText").GetComponent<TextMeshProUGUI>();

        switch (ping.GetPingType())
        {
            default:
            case PingSystem.Ping.Type.Move:
                break;
            case PingSystem.Ping.Type.Enemy:
                image.sprite = GameAssetsSystem.i.pingEnemySprite;
                distanceText.color = GameAssetsSystem.i.pingEnemyColor;
                break;
        }

        ping.OnDestroyed += delegate (object sender,System.EventArgs e) {
            UnityEngine.Object.Destroy(gameObject);
        };
    }

    private void Update()
    {
        Vector2 pingScreenCoordinates = Camera.main.WorldToScreenPoint(ping.GetPosition());

        bool isOffScreen = pingScreenCoordinates.x > Screen.width ||
                           pingScreenCoordinates.x < 0 ||
                           pingScreenCoordinates.y > Screen.height ||
                           pingScreenCoordinates.y < 0;

        image.enabled = isOffScreen;
        distanceText.enabled = isOffScreen;

        if (isOffScreen)
        {
            Vector3 pingPosition = ping.GetPosition();
            // Update UI position
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (pingPosition - fromPosition).normalized;

            float uiRadius = 270f;
            rectTransform.anchoredPosition = dir * uiRadius;

            // Update distance text
            Vector3 playerPosition = PlayerCharacter.GetPosition();
            int distance = Mathf.RoundToInt(Vector3.Distance(pingPosition, playerPosition) / 3f);
            distanceText.text = distance + "M";
        }
        else
        {
            // Ping is on screen 
            // UI element hidden
        }


    }



}
