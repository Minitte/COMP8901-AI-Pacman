using UnityEngine;

using UnityEngine.UI;

public class ShooterGameManager : MonoBehaviour
{
    public ShooterController playerOne;

    public ShooterController playerTwo;

    public ShooterChoice playerOneChoice;

    public ShooterChoice playerTwoChoice;

    public Text endText;

    private enum GamePhase { CHOICE, ACT, POSTACT, END }

    private GamePhase m_phase;

    private void Awake()
    {
        ResetGame();
    }

    private void Update()
    {
        if (m_phase == GamePhase.CHOICE) ChoicePhase();
        else if (m_phase == GamePhase.ACT) ActPhase();
        else if (m_phase == GamePhase.POSTACT) PostActPhase();
        else if (m_phase == GamePhase.END) EndPhase();
    }

    private void ChoicePhase()
    {
        // player 1 choice
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerOne.HasAmmo) playerOneChoice = ShooterChoice.SHOOT;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerOne.HasEnergy) playerOneChoice = ShooterChoice.DODGE;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) playerOneChoice = ShooterChoice.RELOAD;

        // Player 2 choice
        if (Input.GetKeyDown(KeyCode.Alpha8) && playerTwo.HasAmmo) playerTwoChoice = ShooterChoice.SHOOT;
        else if (Input.GetKeyDown(KeyCode.Alpha9) && playerTwo.HasEnergy) playerTwoChoice = ShooterChoice.DODGE;
        else if (Input.GetKeyDown(KeyCode.Alpha0)) playerTwoChoice = ShooterChoice.RELOAD;

        if (playerOneChoice != ShooterChoice.WAITING && playerTwoChoice != ShooterChoice.WAITING)
        {
            m_phase = GamePhase.ACT;
        }

    }

    private void ActPhase()
    {
        // resolve shooting
        if (playerOneChoice == ShooterChoice.SHOOT && playerTwoChoice != ShooterChoice.DODGE)
        {
            if (playerOne.ammoCount >= 1) playerTwo.subtractHealth();
            //if (playerOne.ammoCount >= 2) playerTwo.subtractHealth();
        }

        if (playerTwoChoice == ShooterChoice.SHOOT && playerOneChoice != ShooterChoice.DODGE)
        {
            if (playerTwo.ammoCount >= 1) playerOne.subtractHealth();
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
}
