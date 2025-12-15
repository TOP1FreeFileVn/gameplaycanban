using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealBar : MonoBehaviour
{
    [Header("UI Reference")]
    public Image fillImage;
    void Start()
    {
        SetMaxHealth();
    }
    public void SetMaxHealth()
    {
        if(fillImage == null)
        {
            fillImage.fillAmount = 1f;
        }
    }
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
