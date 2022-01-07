using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Speedometer : MonoBehaviour
{
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 210;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;

    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLableTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = 0f;
        speedMax = 200f;

        CreateSpeedLables();
    }

    private void Update()
    {
        HandlePlayerInput();

        //speed += 30f * Time.deltaTime;
        //if (speed > speedMax) speed = speedMax;

        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void CreateSpeedLables()
    {
        int lableAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for(int i = 0; i <= lableAmount; i++)
        {
            Transform speedLableTransform = Instantiate(speedLabelTemplateTransform, transform);
            float lableSpeedNormalized = (float)i / lableAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - lableSpeedNormalized * totalAngleSize;
            speedLableTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLableTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(lableSpeedNormalized * speedMax).ToString();
            speedLableTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLableTransform.gameObject.SetActive(true);
        }

        needleTransform.SetAsLastSibling();
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            float acceleration = 50f;
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            float deceleration = 20f;
            speed -= deceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float brakeSpeed = 100f;
            speed -= brakeSpeed * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0f, speedMax);
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = speed / speedMax;
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }

}
