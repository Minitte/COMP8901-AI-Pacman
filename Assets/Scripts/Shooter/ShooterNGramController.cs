using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterNGramController : MonoBehaviour
{
    public NGram.NGramString ngram;

    private ShooterGameManager m_sgm;

    private List<string> playerOneChoices;

    private List<string> playerTwoChoices;

    private int count { get { return playerOneChoices.Count; } }

    private void Awake()
    {
        ngram = new NGram.NGramString();

        playerOneChoices = new List<string>();
        playerTwoChoices = new List<string>();

        m_sgm = GetComponent<ShooterGameManager>();

        m_sgm.OnActPhase += RegisterChoice;
    }

    public void RegisterChoice()
    {
        string p1c = ChoiceToString(m_sgm.playerOneChoice);
        string p2c = ChoiceToString(m_sgm.playerTwoChoice);

        string p1Ammo = m_sgm.playerOne.HasAmmo ? "1" : "0";
        string p2Ammo = m_sgm.playerTwo.HasAmmo ? "1" : "0";

        string p1Energy = m_sgm.playerOne.HasEnergy ? "1" : "0";
        string p2Energy = m_sgm.playerTwo.HasEnergy ? "1" : "0";

        // atleast 3
        if (count > 2)
        {
            ngram.Add(p1Ammo + p1Energy + GetLastCombo(playerOneChoices, 2), p1c);
            ngram.Add(p1Ammo + p1Energy + GetLastCombo(playerTwoChoices, 2), p2c);
        }

        playerOneChoices.Add(p1c);
        playerTwoChoices.Add(p2c);

        //StreamWriter wr = new StreamWriter("ngramshooter.ngram");
        //wr.Write(ngram.ToJson());
        //wr.Close();
    }

    public ShooterChoice MakePrediction(bool forPlayer1)
    {
        string prediction = "";

        string ammo = forPlayer1 ? m_sgm.playerOne.HasAmmo ? "1" : "0" : m_sgm.playerTwo.HasAmmo ? "1" : "0"; ;
        string energy = forPlayer1 ? m_sgm.playerOne.HasEnergy ? "1" : "0" : m_sgm.playerTwo.HasEnergy ? "1" : "0"; ;

        if (forPlayer1) prediction = ngram.Predict(ammo + energy + GetLastCombo(playerOneChoices, 2));
        else prediction = ngram.Predict(ammo + energy + GetLastCombo(playerTwoChoices, 2));

        ShooterChoice choice = StringToChoice(prediction);

        return choice;
    }

    private string GetLastCombo(List<string> history, int n)
    {
        string combo = "";
        for (int i = history.Count - n; i < history.Count && i >= 0; i++)
        {
            combo += history[i];
        }

        return combo;
    }

    private string ChoiceToString(ShooterChoice sc)
    {
        switch (sc)
        {
            case ShooterChoice.SHOOT: 
                return "s";
            case ShooterChoice.DODGE:
                return "d";
            case ShooterChoice.RELOAD:
                return "r";
        }

        return "w";
    }

    private ShooterChoice StringToChoice(string s)
    {
        if (s == "s") return ShooterChoice.SHOOT;
        if (s == "d") return ShooterChoice.DODGE;
        if (s == "r") return ShooterChoice.RELOAD;

        return ShooterChoice.WAITING;
    }
}
