using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsSystem : MonoBehaviour
{
    private static GameAssetsSystem _i;

    public static GameAssetsSystem i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssetsSystem>("GameAssetsSystem"));
            return _i;
        }
    }

    public Transform pfPingWorld;
    public RectTransform pfPingUI;

    public Color pingMoveColor;
    public Color pingEnemyColor;

    public Sprite pingMoveSprite;
    public Sprite pingEnemySprite;


}
