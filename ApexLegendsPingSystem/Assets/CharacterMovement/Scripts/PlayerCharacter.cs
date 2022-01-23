/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    public static PlayerCharacter instance; 
    private PlayerCharacter_Base playerCharacterBase;
    private Vector3 lastMoveDir;

    private void Awake() {
        instance = this;
        playerCharacterBase = gameObject.GetComponent<PlayerCharacter_Base>();
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        float speed = 50f;
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            playerCharacterBase.PlayIdleAnimation(lastMoveDir);
        } else {
            Vector3 moveDir = new Vector3(moveX, moveY).normalized;
            lastMoveDir = moveDir;
            playerCharacterBase.PlayWalkingAnimation(moveDir);
            transform.position += moveDir * speed * Time.deltaTime;
        }
    }

    public static Vector3 GetPosition()
    {
        return instance.transform.position;
    }
}
