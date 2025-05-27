using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manabar : MonoBehaviour
{
    Slider manaSlider;

    private void Start()
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
