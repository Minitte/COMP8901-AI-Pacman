using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterANNController : MonoBehaviour
{
    private ANN.ArtificalNerualNetwork m_ann;

    private ShooterGameManager m_gameManager;

    private ShooterController PlayerOne { get { return m_gameManager.playerOne; } }

    private ShooterController PlayerTwo { get { return m_gameManager.playerTwo; } }

    private void Awake()
    {
        m_gameManager = GetComponent<ShooterGameManager>();

        int[] structure = new int[] { 6, 10, 10, 3 };

        m_ann = new ANN.ArtificalNerualNetwork(structure);

        m_ann.gainTerm = 0.15f;

        m_gameManager.OnActPhase += RegisterChoice;
    }

    public void RegisterChoice()
    {
        float[] input = new float[]
        {
            // player 1 (me)
            PlayerOne.ammoCount,
            PlayerOne.energyCount,
            PlayerOne.health,

            // player 2 (other)
            PlayerTwo.ammoCount,
            PlayerTwo.energyCount,
            PlayerTwo.health
        };

        float[] expected = new float[]
        {
            m_gameManager.playerOneChoice == ShooterChoice.SHOOT ? 1f : 0,
            m_gameManager.playerOneChoice == ShooterChoice.DODGE ? 1f : 0,
            m_gameManager.playerOneChoice == ShooterChoice.RELOAD ? 1f : 0
        };

        m_ann.Train(input, expected);
    }

    public ShooterChoice Predict(ShooterController me, ShooterController enemy)
    { 
        float[] input = new float[]
        {
            // player 1 (me)
            me.ammoCount,
            me.energyCount,
            me.health,

            // player 2 (other)
            enemy.ammoCount,
            enemy.energyCount,
            enemy.health
        };

        float[] output = m_ann.Forward(input);

        int bestChoice = 0;
        float bestResult = output[0];

        // find best
        for (int i = 1; i < output.Length; i++)
        {
            if (output[i] > bestResult)
            {
                bestChoice = i;
                bestResult = output[i];
            }
        }

        // not sure enough.
        if (bestResult < 0.5f) return ShooterChoice.WAITING;

        ShooterChoice choice = (ShooterChoice)(bestChoice + 1);

        // must have ammo to shoot
        if (choice == ShooterChoice.SHOOT && me.HasAmmo) return choice;

        // got dodge and have energy
        if (choice == ShooterChoice.DODGE && me.HasEnergy) return choice;

        if (choice == ShooterChoice.RELOAD) return choice;

        return ShooterChoice.WAITING;
    }
}
