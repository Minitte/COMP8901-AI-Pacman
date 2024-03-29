﻿using UnityEngine;

using UnityEngine.UI;

public class ShooterGameManager : MonoBehaviour
{
    public delegate void EmptyDelegate();

    public event EmptyDelegate OnActPhase;

    public ShooterController playerOne;

    public ShooterController playerTwo;

    public ShooterChoice playerOneChoice;

    public ShooterChoice playerTwoChoice;

    public Text endText;

    public Text annText;

    public Text p2ModeText;

    private enum GamePhase { CHOICE, ACT, POSTACT, END }

    private GamePhase m_phase;

    private enum PlayerMode { HUMAN, RAND, RULE, NGRAM, ANN }

    private PlayerMode m_player2Mode;

    private ShooterNGramController m_ngramControler;

    private ShooterANNController m_annController;

    private ShooterRuleBasedController m_rbsController;

    private void Awake()
    {
        m_ngramControler = GetComponent<ShooterNGramController>();
        m_annController = GetComponent<ShooterANNController>();
        m_rbsController = GetComponent<ShooterRuleBasedController>();
        ResetGame();
        SetMode(0);
    }

    private void Update()
    {
        if (m_phase == GamePhase.CHOICE) ChoicePhase();
        else if (m_phase == GamePhase.ACT) ActPhase();
        else if (m_phase == GamePhase.POSTACT) PostActPhase();
        else if (m_phase == GamePhase.END) EndPhase();
    }

    public void SetMode(int mode)
    {
        m_player2Mode = (PlayerMode)mode;
        p2ModeText.text = m_player2Mode.ToString();
    }

    private void ChoicePhase()
    {
        
        // player 1 choice
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerOne.HasAmmo) playerOneChoice = ShooterChoice.SHOOT;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerOne.HasEnergy) playerOneChoice = ShooterChoice.DODGE;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) playerOneChoice = ShooterChoice.RELOAD;

        // player 2
        switch (m_player2Mode)
        {
            case PlayerMode.HUMAN:
                playerTwoChoice = P2HumanChoice();
                break;

            case PlayerMode.RAND:
                if (playerTwoChoice != ShooterChoice.WAITING) break;
                playerTwoChoice = MakeRandomChoice(playerTwo);
                break;

            case PlayerMode.RULE:
                if (playerTwoChoice != ShooterChoice.WAITING) break;
                playerTwoChoice = RBSChoice(false);
                break;

            case PlayerMode.NGRAM:
                if (playerTwoChoice != ShooterChoice.WAITING) break;
                playerTwoChoice = NGramChoice();
                break;

            case PlayerMode.ANN:
                if (playerTwoChoice != ShooterChoice.WAITING) break;
                playerTwoChoice = ANNChoice(false);
                break;
        }

        playerOne.isReady = playerOneChoice != ShooterChoice.WAITING;
        playerOne.UpdateSprites();
        playerTwo.isReady = playerTwoChoice != ShooterChoice.WAITING;
        playerTwo.UpdateSprites();

        if (playerOne.isReady && playerTwo.isReady)
        {
            m_phase = GamePhase.ACT;
        }

    }

    private void ActPhase()
    {
        OnActPhase?.Invoke();

        // resolve shooting
        if (playerOneChoice == ShooterChoice.SHOOT && playerTwoChoice != ShooterChoice.DODGE)
        {
            if (playerOne.ammoCount >= 1) playerTwo.SubtractHealth();
            //if (playerOne.ammoCount >= 2) playerTwo.subtractHealth();
        }

        if (playerTwoChoice == ShooterChoice.SHOOT && playerOneChoice != ShooterChoice.DODGE)
        {
            if (playerTwo.ammoCount >= 1) playerOne.SubtractHealth();
            //if (playerTwo.ammoCount >= 2) playerOne.subtractHealth();
        }

        UpdateController(playerOne, playerOneChoice);
        UpdateController(playerTwo, playerTwoChoice);

        // tie
        if (!playerOne.HasHealth && !playerTwo.HasHealth)
        {
            endText.text = "TIE!";
        }
        // player two wins
        else if (!playerOne.HasHealth)
        {
            endText.text = "PLAYER TWO WINS!";
        }
        // player one wins
        else if (!playerTwo.HasHealth)
        {
            endText.text = "PLAYER ONE WINS!";
        }

        if (!playerOne.HasHealth || !playerTwo.HasHealth) m_phase = GamePhase.END;
        else m_phase = GamePhase.POSTACT;
        
    }

    private void UpdateController(ShooterController controller, ShooterChoice choice)
    {
        switch (choice)
        {
            case ShooterChoice.SHOOT:
                controller.SubtractAmmo();
                break;

            case ShooterChoice.DODGE:
                controller.SubtractEnergy();
                break;

            case ShooterChoice.RELOAD:
                controller.AddAmmo();
                controller.AddEnergy();
                break;
        }

        controller.RunAnimation(choice);
    }

    private void PostActPhase()
    {
        playerOneChoice = ShooterChoice.WAITING;
        playerTwoChoice = ShooterChoice.WAITING;

        m_phase = GamePhase.CHOICE;
    }

    private void EndPhase()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();

            m_phase = GamePhase.CHOICE;
        }
    }

    private void ResetGame()
    {
        playerOne.ammoCount = 1;
        playerOne.energyCount = 2;
        playerOne.health = 2;
        playerOne.UpdateSprites();

        playerTwo.ammoCount = 1;
        playerTwo.energyCount = 2;
        playerTwo.health = 2;
        playerTwo.UpdateSprites();

        playerOneChoice = ShooterChoice.WAITING;
        playerTwoChoice = ShooterChoice.WAITING;

        endText.text = "";
    }

    private ShooterChoice MakeRandomChoice(ShooterController shooter)
    {
        Debug.Log("Made random choice.");

        ShooterChoice choice = (ShooterChoice)Random.Range(1, 4);

        while (true)
        {
            // got shoot and have ammo
            if (choice == ShooterChoice.SHOOT && shooter.HasAmmo) return choice;

            // got dodge and have energy
            if (choice == ShooterChoice.DODGE && shooter.HasEnergy) return choice;

            if (choice == ShooterChoice.RELOAD) return choice;

            choice++;

            if ((int)choice >= 4)
            {
                choice = (ShooterChoice)1;
            }
        }
    }

    private ShooterChoice P2HumanChoice()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8) && playerTwo.HasAmmo) return ShooterChoice.SHOOT;
        else if (Input.GetKeyDown(KeyCode.Alpha9) && playerTwo.HasEnergy) return ShooterChoice.DODGE;
        else if (Input.GetKeyDown(KeyCode.Alpha0)) return ShooterChoice.RELOAD;

        return playerTwoChoice;
    }

    private ShooterChoice NGramChoice()
    {
        ShooterChoice choice = m_ngramControler.MakePrediction(false);

        // ngram failed to make a choice, defaulting random
        if (choice == ShooterChoice.WAITING) return MakeRandomChoice(playerTwo);

        return choice;
    }

    private ShooterChoice ANNChoice(bool asPlayerOne)
    {
        ShooterController me = asPlayerOne ? playerOne : playerTwo;
        ShooterController them = asPlayerOne ? playerTwo : playerOne;

        ShooterChoice choice = m_annController.Predict(me, them);

        if (choice == ShooterChoice.WAITING)
        {
            annText.text = "ANN: Not enough training. Using random choice.";
            return MakeRandomChoice(me);
        }

        annText.text = "ANN: Made a choice.";

        return choice;
    }

    private ShooterChoice RBSChoice(bool asPlayerOne)
    {
        ShooterController me = asPlayerOne ? playerOne : playerTwo;
        ShooterController them = asPlayerOne ? playerTwo : playerOne;

        ShooterChoice choice = m_rbsController.GetChoice(me, them);

        if (choice == ShooterChoice.WAITING)
        {
            return MakeRandomChoice(me);
        }

        return choice;
    }
}
