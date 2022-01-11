using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 2f;
    private Character character;

    private Image healthImage;
    private Image damageHealthImage;
    private float damageHealthFadeTimer;
    private int damagedHealthPreviousHealthAmount;

    private Image lowHealthFlashingImage;
    private float lowHealthAlphaChange;

    private Image shield0Image;
    private Image shield1Image;
    private Image shield2Image;
    private Image shield3Image;
    private Image[] shieldImageArray;

    private void Awake()
    {
        healthImage = transform.Find("healthBar").Find("bar").GetComponent<Image>();
        damageHealthImage = transform.Find("healthBar").Find("damaged").GetComponent<Image>();
        lowHealthFlashingImage = transform.Find("healthBar").Find("flashing").GetComponent<Image>();
        lowHealthAlphaChange = +4f;
        lowHealthFlashingImage.gameObject.SetActive(false);


        shield0Image = transform.Find("shieldBar_0").Find("bar").GetComponent<Image>();
        shield1Image = transform.Find("shieldBar_1").Find("bar").GetComponent<Image>();
        shield2Image = transform.Find("shieldBar_2").Find("bar").GetComponent<Image>();
        shield3Image = transform.Find("shieldBar_3").Find("bar").GetComponent<Image>();

        shieldImageArray = new Image[]
        {
            shield0Image,
            shield1Image,
            shield2Image,
            shield3Image
        };
    }


    private void Update()
    {
        // Is the damaged health image visible ?
        if (damageHealthImage.color.a > 0)
        {
            // Cound down fade timer
            damageHealthFadeTimer -= Time.deltaTime;
            if(damageHealthFadeTimer < 0)
            {
                // Fade timer over , lower alpha
                Color newColor = damageHealthImage.color;
                newColor.a -= Time.deltaTime * 2f;
                damageHealthImage.color = newColor;
            }
        }

        if (lowHealthFlashingImage.gameObject.activeSelf)
        {
            // Flashing health image
            Color lowHealthColor = lowHealthFlashingImage.color;
            lowHealthColor.a += lowHealthAlphaChange;
            Debug.Log(lowHealthColor.a);
            if (lowHealthColor.a > 1f)
            {
                lowHealthAlphaChange *= -1f;
                lowHealthColor.a = 1f;
            }
            if (lowHealthColor.a < 0f)
            {
                lowHealthAlphaChange *= -1f;
                lowHealthColor.a = 0f;
            }
            lowHealthFlashingImage.color = lowHealthColor;
        }

    }

    public void setCharacter(Character character)
    {
        this.character = character;

        damagedHealthPreviousHealthAmount = character.GetHealth();


        UpdateShieldSegments();
        UpdateBodyArmorIcon();
        UpdateHealthShield();

        character.OnHealthShieldChanged += Character_OnHealthShieldChanged; 
    }
    // 监听生命值变换事件
    private void Character_OnHealthShieldChanged(object sender, System.EventArgs e)
    {
        UpdateHealthShield();

        // Health changed , reset fade timer
        damageHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        if(damageHealthImage.color.a <= 0)
        {
            // Damaged health bar not visible , set size
            damageHealthImage.fillAmount = (float)damagedHealthPreviousHealthAmount / Character.HEALTH_MAX;

        }
        // Make damaged health bar visible
        Color damagedColorFullAlpha = damageHealthImage.color;
        damagedColorFullAlpha.a = 1f;
        damageHealthImage.color = damagedColorFullAlpha;

        // Set the previous health amount to the current health amount
        damagedHealthPreviousHealthAmount = character.GetHealth();

        lowHealthFlashingImage.gameObject.SetActive(character.GetHealth() <= 30);
    }

    // 更新护甲值
    private void UpdateHealthShield()
    {
        healthImage.fillAmount = (float)character.GetHealth() / Character.HEALTH_MAX;


        int shield = character.GetShield();
        int shieldSegmentCount = 4;
        for (int i = 0; i < shieldSegmentCount; i++)
        {
            int shieldSegmentMin = i * Character.SHIELD_AMOUNT_PER_SEGMENT;
            int shieldSegmentMax = (i + 1) * Character.SHIELD_AMOUNT_PER_SEGMENT;

            if (shield <= shieldSegmentMin)
            {
                // Shield amount under minumum for this aegment
                shieldImageArray[i].fillAmount = 0f;
            }
            else
            {
                if(shield >= shieldSegmentMax)
                {
                    // Shield amount above max
                    shieldImageArray[i].fillAmount = 1f;
                }
                else
                {
                    // Shield amount somewhere in between this segment
                    float fillAmount = (float)(shield - shieldSegmentMin) / Character.SHIELD_AMOUNT_PER_SEGMENT;
                    shieldImageArray[i].fillAmount = fillAmount;
                }
            };
        }
    }

    // 更新护甲段数
    private void UpdateShieldSegments()
    {
        Character.BodyArmor bodyArmor = character.GetEquippedBodyArmor();

        transform.Find("shieldBar_0").gameObject.SetActive(false);
        transform.Find("shieldBar_1").gameObject.SetActive(false);
        transform.Find("shieldBar_2").gameObject.SetActive(false);
        transform.Find("shieldBar_3").gameObject.SetActive(false);

        Color bodyArmorColor = Color.white;

        switch (bodyArmor)
        {
            default:
            case Character.BodyArmor.None:
                break;
            case Character.BodyArmor.Tier_1:
                transform.Find("shieldBar_0").gameObject.SetActive(true);
                transform.Find("shieldBar_1").gameObject.SetActive(true);
                bodyArmorColor = Character.TIER_1_COLOR;
                break;
            case Character.BodyArmor.Tier_2:
                transform.Find("shieldBar_0").gameObject.SetActive(true);
                transform.Find("shieldBar_1").gameObject.SetActive(true);
                transform.Find("shieldBar_2").gameObject.SetActive(true);
                bodyArmorColor = Character.TIER_2_COLOR;
                break;
            case Character.BodyArmor.Tier_3:
                transform.Find("shieldBar_0").gameObject.SetActive(true);
                transform.Find("shieldBar_1").gameObject.SetActive(true);
                transform.Find("shieldBar_2").gameObject.SetActive(true);
                transform.Find("shieldBar_3").gameObject.SetActive(true);
                bodyArmorColor = Character.TIER_3_COLOR;
                break;
        }

        shield0Image.color = bodyArmorColor;
        shield1Image.color = bodyArmorColor;
        shield2Image.color = bodyArmorColor;
        shield3Image.color = bodyArmorColor;
    }
    // 更新防弹衣图标
    private void UpdateBodyArmorIcon()
    {
        Character.BodyArmor bodyArmor = character.GetEquippedBodyArmor();

        transform.Find("armor").gameObject.SetActive(false);

        switch (bodyArmor)
        {
            default:
            case Character.BodyArmor.None:
                break;
            case Character.BodyArmor.Tier_1:
                transform.Find("armor").gameObject.SetActive(true);
                transform.Find("armor").Find("background").GetComponent<Image>().color = Character.TIER_1_COLOR;
                break;
            case Character.BodyArmor.Tier_2:
                transform.Find("armor").gameObject.SetActive(true);
                transform.Find("armor").Find("background").GetComponent<Image>().color = Character.TIER_2_COLOR;
                break;
            case Character.BodyArmor.Tier_3:
                transform.Find("armor").gameObject.SetActive(true);
                transform.Find("armor").Find("background").GetComponent<Image>().color = Character.TIER_3_COLOR;
                break;
        }
    }
}
