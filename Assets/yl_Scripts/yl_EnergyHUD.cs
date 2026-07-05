using UnityEngine;
using UnityEngine.UI;

public class yl_EnergyHUD : MonoBehaviour
{
    [Header("Energy UI")]
    public Image energyIconFill;

    [Header("Energy Settings")]
    [Range(0f, 1f)]
    public float energy = 1f;

    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    [Header("Animation")]
    public float smoothSpeed = 6f;
    public bool fadeWithEnergy = true;

    [Header("Energy Colors")]
    public Color fullEnergyColor = Color.cyan;
    public Color mediumEnergyColor = Color.yellow;
    public Color lowEnergyColor = Color.red;

    private float displayedEnergy = 1f;

    void Start()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        displayedEnergy = currentEnergy / maxEnergy;
        UpdateIconInstant();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            UseEnergy(20f * Time.deltaTime);
        else
            RecoverEnergy(10f * Time.deltaTime);

        energy = currentEnergy / maxEnergy;

        displayedEnergy = Mathf.Lerp(
            displayedEnergy,
            energy,
            Time.deltaTime * smoothSpeed
        );

        UpdateIconSmooth();
    }

    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void RecoverEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void SetEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
    }

    void UpdateIconInstant()
    {
        if (energyIconFill == null) return;

        energyIconFill.fillAmount = displayedEnergy;

        if (fadeWithEnergy)
        {
            UpdateEnergyColor();
        }
    }

    void UpdateIconSmooth()
    {
        if (energyIconFill == null) return;

        energyIconFill.fillAmount = displayedEnergy;

        if (fadeWithEnergy)
        {
            UpdateEnergyColor();
        }
    }

    void UpdateEnergyColor()
    {
        if (energyIconFill == null) return;

        Color targetColor;

        if (displayedEnergy > 0.5f)
        {
            // Cyan -> Yellow
            targetColor = Color.Lerp(
                mediumEnergyColor,
                fullEnergyColor,
                (displayedEnergy - 0.5f) * 2f
            );
        }
        else
        {
            // Red -> Yellow
            targetColor = Color.Lerp(
                lowEnergyColor,
                mediumEnergyColor,
                displayedEnergy * 2f
            );
        }

        if (fadeWithEnergy)
        {
            targetColor.a = Mathf.Lerp(0.2f, 1f, displayedEnergy);
        }

        energyIconFill.color = targetColor;
    }
}