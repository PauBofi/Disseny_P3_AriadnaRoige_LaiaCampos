using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinaMana : MonoBehaviour
{
    public ManaBar manabar;
    public int maxMana = 8;
    internal int currentMana;
    public float manaRegenInterval = 3f;
    private float manaRegenTimer;

    void Start()
    {
        InitializeMana();
    }

    void Update()
    {
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= manaRegenInterval)
        {
            RegenerateMana(1);
            manaRegenTimer = 0f;
        }
    }

    public void InitializeMana()
    {
        currentMana = maxMana;
        manabar.SetMaxMana(maxMana);
        manabar.SetMana(currentMana);
    }

    public void UseMana(int amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        manabar.SetMana(currentMana);
    }

    public void RegenerateMana(int amount)
    {
        if (currentMana < maxMana)
        {
            currentMana += amount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            manabar.SetMana(currentMana);
        }
    }

    public void Initialize(ManaBar manabar)
    {
        this.manabar = manabar;
        InitializeMana();
    }
}
