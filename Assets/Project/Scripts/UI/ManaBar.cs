using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    Slider manaSlider;

    private void Awake()
    {
        manaSlider = GetComponent<Slider>();
    }

    public void SetMaxMana(int maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = maxMana;
    }

    public void SetMana(int mana)
    {
        manaSlider.value = mana;
    }
}
