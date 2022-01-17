using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;

public class ManaBar : MonoBehaviour
{
    private Mana mana;
    private float barMaskWidth;
    private RectTransform edgeRectTransform; 
    private RectTransform barMaskRectTransform;
    private RawImage barRawImage;

    private void Awake() 
    {
        barMaskRectTransform = transform.Find("barMask").GetComponent<RectTransform>();
        barRawImage = transform.Find("barMask").Find("bar").GetComponent<RawImage>();
        edgeRectTransform = transform.Find("edge").GetComponent<RectTransform>();

        barMaskWidth = barMaskRectTransform.sizeDelta.x;

        mana = new Mana();

        CMDebug.ButtonUI(new Vector2(0, -50), "Spend Mana", () =>
          {
              mana.TrySpendMana(30);
          });
    }

    private void Update()
    {
        mana.Update();

        Rect uvRect = barRawImage.uvRect;
        uvRect.x += .2f * Time.deltaTime;
        barRawImage.uvRect = uvRect;

        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = mana.GetManaNormalized() * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;

        edgeRectTransform.anchoredPosition = new Vector2(mana.GetManaNormalized() * barMaskWidth, 0);
        edgeRectTransform.gameObject.SetActive(mana.GetManaNormalized() < 1f);
     }

}

public class Mana
{
    public const int MANA_MAX = 100;

    private float manaAmount;
    private float manaRagenAmount;

    public Mana()
    {
        manaAmount = 0;
        manaRagenAmount = 30f;
    }

    public void Update()
    {
        manaAmount += manaRagenAmount * Time.deltaTime;
        manaAmount = Mathf.Clamp(manaAmount, 0f, MANA_MAX);
    }

    public void TrySpendMana(int amount)
    {
        if(manaAmount >= amount)
        {
            manaAmount -= amount;
        }
    }

    public float GetManaNormalized()
    {
        return manaAmount / MANA_MAX;
    }
}
