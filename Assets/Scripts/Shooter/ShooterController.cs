﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public const int MAX_AMMO = 3;

    public const int MAX_ENERGY = 3;

    [Header("Shooter Sprites")]

    public Sprite idle;

    public Sprite shoot;

    public Sprite dodge;

    public Sprite reload;

    [Header("Sprites")]

    public SpriteRenderer shooterSprite;

    public SpriteRenderer[] ammoSprites;

    public SpriteRenderer[] energySprites;

    public SpriteRenderer[] healthSprites;

    public SpriteRenderer readySprite;

    [Header("Data")]

    public int ammoCount;

    public int energyCount;

    public int health;

    public bool isReady;

    public bool HasAmmo { get { return ammoCount != 0; } }

    public bool HasEnergy { get { return energyCount != 0; } }

    public bool HasHealth { get { return health != 0; } }

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();   
    }

    public void AddAmmo()
    {
        if (ammoCount >= MAX_AMMO) return;

        ammoCount++;

        UpdateSprites();
    }

    public void SubtractAmmo()
    {
        if (ammoCount <= 0) return;

        ammoCount--;

        UpdateSprites();
    }

    public void AddEnergy()
    {
        if (energyCount >= MAX_ENERGY) return;

        energyCount++;

        UpdateSprites();
    }

    public void SubtractEnergy()
    {
        if (energyCount <= 0) return;

        energyCount--;

        UpdateSprites();
    }

    public void SubtractHealth()
    {
        if (health <= 0) return;

        health--;

        UpdateSprites();
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < ammoSprites.Length; i++)
        {
            if (i < ammoCount) ammoSprites[i].color = Color.yellow;
            else ammoSprites[i].color = Color.black;
        }

        for (int i = 0; i < energySprites.Length; i++)
        {
            if (i < energyCount) energySprites[i].color = Color.green;
            else energySprites[i].color = Color.black;
        }

        for (int i = 0; i < healthSprites.Length; i++)
        {
            if (i < health) healthSprites[i].color = Color.red;
            else healthSprites[i].color = Color.black;
        }

        if (isReady) readySprite.color = Color.green;
        else readySprite.color = Color.black;
    }

    public void RunAnimation(ShooterChoice choice)
    {
        m_animator.SetInteger("Choice", (int)choice);
        m_animator.SetTrigger("Act");
    }
}
