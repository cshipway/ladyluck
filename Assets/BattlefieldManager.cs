using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bigText;
    [SerializeField] private TextMeshProUGUI reasonableText;
    [SerializeField] private PhysicalChampionHand heroHand;
    [SerializeField] private PhysicalChampionHand enemyHand;
    [SerializeField] private Transform showCardAnchor;
    [SerializeField] private Card cardPrefab;

    [Header("Battlefield HUD")]
    [SerializeField] private GameObject battlefieldHud;
    [SerializeField] private TextMeshProUGUI hudHeroHpText;
    [SerializeField] private TextMeshProUGUI hudHeroMpText;
    [SerializeField] private TextMeshProUGUI hudEnemyHpText;
    [SerializeField] private TextMeshProUGUI hudEnemyMpText;

    private bool isAutobattling;

    [SerializeField] private Champion hero;
    [SerializeField] private Champion enemy;

    [SerializeField] private DeckDefinition heroStartingDeck;
    [SerializeField] private DeckDefinition enemyStartingDeck;

    private void Awake()
    {
        hero = new Champion("Hero", 10, new Deck() { cards = new List<CardDefinition>(heroStartingDeck.deck.cards) });
        enemy = new Champion("Enemy", 10, new Deck() { cards = new List<CardDefinition>(enemyStartingDeck.deck.cards) });

        StartBattle();
    }

    private void Update()
    {
        if(isAutobattling)
        {
            hudHeroHpText.text = hero.hp.ToString();
            hudHeroMpText.text = hero.mp.ToString();
            hudEnemyHpText.text = enemy.hp.ToString();
            hudEnemyMpText.text = enemy.mp.ToString();
        }
    }

    public void StartBattle()
    {
        StartCoroutine(Autobattle());
    }

    private IEnumerator Autobattle()
    {
        yield return StartCoroutine(ShowBigText("Match Start!"));

        hero.foe = enemy;
        enemy.foe = hero;

        isAutobattling = true;

        int round = 1;

        while (hero.hp > 0 && enemy.hp > 0)
        {
            //Put this here in case the while loop gets stuck.
            if (round > 100)
                break;

            yield return StartCoroutine(hero.TakeTurn(this, round));

            if (hero.hp <= 0 || enemy.hp <= 0)
                break;

            yield return StartCoroutine(enemy.TakeTurn(this, round));

            if (hero.hp <= 0 || enemy.hp <= 0)
                break;

            round++;
        }

        if (hero.hp > 0)
            yield return StartCoroutine(ShowBigText($"{hero.name} wins!"));
        else
            yield return StartCoroutine(ShowBigText($"{enemy.name} wins!"));
    }

    public IEnumerator ShowBigText(string message)
    {
        bigText.transform.parent.gameObject.SetActive(true);
        bigText.text = message;

        yield return null;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) && !DeckBuilderManager.Instance.deckBuilding);

        bigText.transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator ShowReasonableText(string message)
    {
        reasonableText.transform.parent.gameObject.SetActive(true);
        reasonableText.text = message;

        float anim = 1;
        while(anim > 0)
        {
            reasonableText.transform.parent.localPosition = new Vector3(0, 10 * anim, 0);
            anim = Mathf.Clamp(anim - (Time.deltaTime * 8), 0, anim);
            yield return null;
        }
        reasonableText.transform.parent.localPosition = new Vector3(0, 0, 0);

        yield return null;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) && !DeckBuilderManager.Instance.deckBuilding);

        reasonableText.transform.parent.gameObject.SetActive(false);
    }

    public void PutCardInHand(Champion champion, CardDefinition card)
    {
        if (champion.name == "Hero")
            heroHand.PutCardInHand(card);
        else
            enemyHand.PutCardInHand(card);
    }

    public void RemoveCardFromHand(Champion champion, CardDefinition card)
    {
        if (champion.name == "Hero")
            heroHand.RemoveCardFromHand(card);
        else
            enemyHand.RemoveCardFromHand(card);
    }

    public void ShowCard(CardDefinition card)
    {
        showCardAnchor.gameObject.SetActive(true);
        Instantiate(cardPrefab, showCardAnchor).Bootup(card, false);
    }

    public void StopShowingCard()
    {
        if (showCardAnchor.childCount > 0)
            Destroy(showCardAnchor.GetChild(0).gameObject);
        showCardAnchor.gameObject.SetActive(false);
    }
}
