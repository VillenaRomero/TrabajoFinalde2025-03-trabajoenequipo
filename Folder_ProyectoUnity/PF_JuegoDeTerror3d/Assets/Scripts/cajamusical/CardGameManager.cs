using UnityEngine;
using UnityEngine.UI;

public enum CardType { IncreaseHandle, DecreaseHandle, ReverseDirection, Shield, SkipTurn }

public class Card
{
    public CardType type;
    public int value;

    public Card(CardType t, int v = 0)
    {
        type = t;
        value = v;
    }

    public override string ToString()
    {
        return type.ToString() + (value != 0 ? " (" + value + ")" : "");
    }
}
public class CardGameManager : MonoBehaviour
{
    [Header("Referencias")]
    public SurpriseBox surpriseBox;
    public BoxPlayer[] players;

    [Header("UI")]
    public Text handleText;
    public Text turnText;
    public GameObject cardPanel;
    public GameObject cardPrefab;
    public Button spinButton;
    public Button useCardButton;

    private int currentPlayerIndex = 0;
    private int direction = 1;
    private bool roundActive = false;

    void Start()
    {
        DealCards(2);
        UpdateUI();

        spinButton.onClick.AddListener(PlayerSpin);
        useCardButton.onClick.AddListener(PlayerUseCard);
    }

    void DealCards(int count)
    {
        System.Random rnd = new System.Random();
        foreach (var p in players)
        {
            for (int i = 0; i < count; i++)
            {
                CardType type = (CardType)rnd.Next(0, 5);
                int val = (type == CardType.IncreaseHandle || type == CardType.DecreaseHandle) ? rnd.Next(1, 4) : 0;
                p.AddCard(new Card(type, val));
            }
        }
    }

    void PlayerSpin()
    {
        if (roundActive || surpriseBox.isOpen) return;
        roundActive = true;
        surpriseBox.StartSpin();
        Invoke(nameof(SpinEnd), 4f);
    }

    void SpinEnd()
    {
        BoxPlayer target = FindPlayerByTransform(surpriseBox.target);
        if (target == null || !target.isAlive)
        {
            roundActive = false;
            NextTurn();
            return;
        }

        if (target.isHuman)
        {
            turnText.text = "Tu turno: gira la manija o usa carta.";
        }
        else
        {
            int roll = Random.Range(0, 2);
            if (roll == 0)
            {
                int turns = Random.Range(1, 4);
                for (int i = 0; i < turns; i++) surpriseBox.ModifyHandle(1);
                Debug.Log(target.playerName + " giró la manija " + turns + " veces!");
            }
            else if (target.handCount > 0)
            {
                int cardIndex = Random.Range(0, target.handCount);
                UseCard(target, target.hand[cardIndex]);
                target.RemoveCard(cardIndex);
            }

            CheckBoxEffect(target);
            roundActive = false;
            NextTurn();
        }
        UpdateUI();
    }

    void PlayerUseCard()
    {
        BoxPlayer player = players[currentPlayerIndex];
        if (player.handCount == 0) return;

        Card card = player.hand[0]; 
        UseCard(player, card);
        player.RemoveCard(0);

        CheckBoxEffect(player);
        roundActive = false;
        NextTurn();
        UpdateUI();
    }

    void UseCard(BoxPlayer player, Card card)
    {
        switch (card.type)
        {
            case CardType.IncreaseHandle: surpriseBox.ModifyHandle(card.value); break;
            case CardType.DecreaseHandle: surpriseBox.ModifyHandle(-card.value); break;
            case CardType.ReverseDirection: direction *= -1; break;
            case CardType.Shield: player.hasShield = true; break;
            case CardType.SkipTurn: player.skipNextTurn = true; break;
        }
        Debug.Log(player.playerName + " usó " + card.type);
    }

    void CheckBoxEffect(BoxPlayer player)
    {
        if (surpriseBox.isOpen)
        {
            if (player.hasShield)
            {
                Debug.Log(player.playerName + " se salvó con escudo!");
                player.hasShield = false;
                surpriseBox.ResetBox();
            }
            else
            {
                player.isAlive = false;
                Debug.Log( player.playerName + " murió!");
            }
        }
    }

    void NextTurn()
    {
        currentPlayerIndex += direction;
        if (currentPlayerIndex >= players.Length) currentPlayerIndex = 0;
        if (currentPlayerIndex < 0) currentPlayerIndex = players.Length - 1;

        if (!players[currentPlayerIndex].isAlive || players[currentPlayerIndex].skipNextTurn)
        {
            players[currentPlayerIndex].skipNextTurn = false;
            NextTurn();
            return;
        }

        if (AllPlayersActed()) DealCards(2);

        UpdateUI();
    }

    bool AllPlayersActed()
    {
        foreach (var p in players)
            if (p.isAlive && p.handCount == 0)
                return true;
        return false;
    }

    BoxPlayer FindPlayerByTransform(Transform t)
    {
        foreach (var p in players)
            if (p.transform == t)
                return p;
        return null;
    }

    void UpdateUI()
    {
        handleText.text = "Manija: " + surpriseBox.handleValue + "/" + surpriseBox.handleLimit;
        turnText.text = "Turno: " + players[currentPlayerIndex].playerName;
    }
}
