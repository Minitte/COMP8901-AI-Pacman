using UnityEngine;

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

    public Text nGramStatus;

    private enum GamePhase { CHOICE, ACT, POSTACT, END }

    private GamePhase m_phase;

    private ShooterNGramController m_ngramControler;

    private float m_time;

    private void Awake()
    {
        m_ngramControler = GetComponent<ShooterNGramController>();
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
        //if (Input.GetKeyDown(KeyCode.Alpha1) && playerOne.HasAmmo) playerOneChoice = ShooterChoice.SHOOT;
        //else if (Input.GetKeyDown(KeyCode.Alpha2) && playerOne.HasEnergy) playerOneChoice = ShooterChoice.DODGE;
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) playerOneChoice = ShooterChoice.RELOAD;

        if (playerOneChoice == ShooterChoice.WAITING)
        {
            playerOneChoice = m_ngramControler.MakePrediction(false);

            if (playerOneChoice == ShooterChoice.WAITING)
            {
                nGramStatus.text = "Unable to make prediction!";
                playerOneChoice = (ShooterChoice)Random.Range(1, 4);

                if (playerOneChoice == ShooterChoice.SHOOT && !playerOne.HasAmmo) playerOneChoice = ShooterChoice.WAITING;
                if (playerOneChoice == ShooterChoice.DODGE && !playerOne.HasEnergy) playerOneChoice = ShooterChoice.WAITING;
            }
            else
            {
                nGramStatus.text = "Able to make prediction!";
            }
        }

        // Player 2 choice
        //if (Input.GetKeyDown(KeyCode.Alpha8) && playerTwo.HasAmmo) playerTwoChoice = ShooterChoice.SHOOT;
        //else if (Input.GetKeyDown(KeyCode.Alpha9) && playerTwo.HasEnergy) playerTwoChoice = ShooterChoice.DODGE;
        //else if (Input.GetKeyDown(KeyCode.Alpha0)) playerTwoChoice = ShooterChoice.RELOAD;

        if (playerTwoChoice == ShooterChoice.WAITING)
        {
            playerTwoChoice = m_ngramControler.MakePrediction(false);

            if (playerTwoChoice == ShooterChoice.WAITING)
            {
                nGramStatus.text = "Unable to make prediction!";
                playerTwoChoice = (ShooterChoice)Random.Range(1, 4);

                if (playerTwoChoice == ShooterChoice.SHOOT && !playerTwo.HasAmmo) playerTwoChoice = ShooterChoice.WAITING;
                if (playerTwoChoice == ShooterChoice.DODGE && !playerTwo.HasEnergy) playerTwoChoice = ShooterChoice.WAITING;
            }
            else
            {
                nGramStatus.text = "Able to make prediction!";
            }
        }

        if (playerOneChoice != ShooterChoice.WAITING && playerTwoChoice != ShooterChoice.WAITING)
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
        m_time += Time.deltaTime;

        if (m_time > 1) return;

        m_time = 0;

        playerOneChoice = ShooterChoice.WAITING;
        playerTwoChoice = ShooterChoice.WAITING;

        m_phase = GamePhase.CHOICE;
    }

    private void EndPhase()
    {
        m_time += Time.deltaTime;

        if (m_time > 2) return;

        m_time = 0;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        ResetGame();

        m_phase = GamePhase.CHOICE;
        //}
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
