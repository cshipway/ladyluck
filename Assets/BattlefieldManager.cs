using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldManager : MonoBehaviour
{
    public static BattlefieldManager Instance { get; private set; }

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
    [SerializeField] private Image hudEnemyPortrait;
    [SerializeField] private TextMeshProUGUI hudEnemyHpText;
    [SerializeField] private TextMeshProUGUI hudEnemyMpText;

    private bool isAutobattling;

    [SerializeField] public Champion hero;
    [SerializeField] public Champion enemy;

    private ScenarioDefinition scenario;

    private void Awake()
    {
        Instance = this;
    }

    public void StartScenario(ScenarioDefinition scenario)
    {
        this.scenario = scenario;

        if(scenario.hasBattle)
        {
            hero = new Champion("Hero", scenario.heroDeckDefinition.startingHp, new Deck() { cards = new List<CardDefinition>(scenario.heroDeckDefinition.deck.cards) });
            enemy = new Champion(scenario.enemyDeckDefinition.name, scenario.enemyDeckDefinition.startingHp, new Deck() { cards = new List<CardDefinition>(scenario.enemyDeckDefinition.deck.cards) });

            if (scenario.enemyDeckDefinition.portrait != null)
                hudEnemyPortrait.sprite = scenario.enemyDeckDefinition.portrait;

            DeckBuilderManager.Instance.SetHeroDeck(hero, hero.deck);
            DeckBuilderManager.Instance.SetEnemyDeck(enemy, enemy.deck);
        }

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

        //Skip cheat
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha0))
            FlowManager.NextScenario();
    }

    public void StartBattle()
    {
        StartCoroutine(Autobattle());
    }

    private IEnumerator Autobattle()
    {
        MusicManager.Instance.PlayHowlingWind();

        ScreenFadeManager.Instance.SetFade(1);

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(FadeSceneIn());

        yield return new WaitForSeconds(1);

        foreach(string s in scenario.scenarioIntroStrings)
            yield return StartCoroutine(ShowReasonableText(s));

        if(scenario.hasBattle)
        {
            MusicManager.Instance.PlayMusic(scenario.scenarioMusicTrack);
            MusicManager.Instance.PlayBoxingRingBell();

            yield return StartCoroutine(ShowBigText("Match Start!"));

            if(FlowManager.scenarioIndex == 1)
            {
                string[] tutorialStrings = new string[7]
                {
                    "(TUTORIAL: At the beginning of each battle, you'll be presented with the decklist for both the hero and his foe.)",
                    "(When they draw cards, they'll draw them from the top-left of their respective decks.)",
                    "(As the Goddess of Luck, click and drag cards to stack them however you see fit! Try to give your champion an edge while taking his foe down a peg!)",
                    "(When you're done manipulating chance, press the [D] key to return to the normal course of battle.)",
                    "(Oh, and this is really important: At any point during the battle, press the [D] key again to bring up the decklist!)",
                    "(That's right! You can alter fate whenever you want, even after the fight has already started.)",
                    "(Try not to get your hapless disciple killed! Good luck!)"
                };
                foreach (string ts in tutorialStrings)
                    yield return StartCoroutine(ShowReasonableText(ts));
            }

            yield return StartCoroutine(ShowReasonableText("Fortuna twists the threads of fate..."));

            DeckBuilderManager.Instance.OnMatchStart();

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
            {
                MusicManager.Instance.PlayBoxingRingBell();
                MusicManager.Instance.PlayVictoryMusic();

                yield return StartCoroutine(ShowBigText($"{hero.name} wins!"));

                foreach (string s in scenario.scenarioOutroStrings)
                    yield return StartCoroutine(ShowReasonableText(s));

                yield return new WaitForSeconds(1);

                yield return StartCoroutine(FadeSceneOut());

                yield return new WaitForSeconds(1);

                FlowManager.NextScenario();
            }

            else
            {
                MusicManager.Instance.StopPlayingMusic();

                yield return StartCoroutine(ShowBigText($"{hero.name} loses!"));
                yield return StartCoroutine(ShowReasonableText($"Fortuna is made a mockery of this day."));
                yield return StartCoroutine(ShowReasonableText($"Click to Try Again."));

                yield return StartCoroutine(FadeSceneOut());

                yield return new WaitForSeconds(1);
                FlowManager.RestartScenario();
            }
        }
        else
        {
            yield return new WaitForSeconds(1);

            yield return StartCoroutine(FadeSceneOut());

            yield return new WaitForSeconds(1);

            FlowManager.NextScenario();
        }
    }

    public IEnumerator ShowBigText(string message)
    {
        bigText.transform.parent.gameObject.SetActive(true);
        bigText.text = message;

        yield return null;

        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && !DeckBuilderManager.Instance.deckBuilding);

        bigText.transform.parent.gameObject.SetActive(false);
        MusicManager.Instance.PlayProceed();
    }

    public IEnumerator ShowReasonableText(string message, bool bottom = false)
    {
        reasonableText.transform.parent.gameObject.SetActive(true);
        reasonableText.text = message;

        if (bottom)
            reasonableText.transform.parent.parent.localPosition = new Vector3(0, -150f, 0);
        else
            reasonableText.transform.parent.parent.localPosition = Vector3.zero;


        float anim = 1;
        while(anim > 0)
        {
            reasonableText.transform.parent.localPosition = new Vector3(0, 10 * anim, 0);
            anim = Mathf.Clamp(anim - (Time.deltaTime * 8), 0, anim);
            yield return null;
        }
        reasonableText.transform.parent.localPosition = new Vector3(0, 0, 0);

        yield return null;

        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && !DeckBuilderManager.Instance.deckBuilding);

        reasonableText.transform.parent.gameObject.SetActive(false);
        MusicManager.Instance.PlayProceed();
    }

    public void PutCardInHand(Champion champion, CardDefinition card)
    {
        if (champion.name == "Hero")
            heroHand.PutCardInHand(champion, card);
        else
            enemyHand.PutCardInHand(champion, card);
    }

    public void RemoveCardFromHand(Champion champion, CardDefinition card)
    {
        if (champion.name == "Hero")
            heroHand.RemoveCardFromHand(card);
        else
            enemyHand.RemoveCardFromHand(card);
    }

    public void ShowCard(CardDefinition card, Champion champion)
    {
        showCardAnchor.gameObject.SetActive(true);
        Instantiate(cardPrefab, showCardAnchor).Bootup(card, false, champion);
    }

    public void StopShowingCard()
    {
        if (showCardAnchor.childCount > 0)
            Destroy(showCardAnchor.GetChild(0).gameObject);
        showCardAnchor.gameObject.SetActive(false);
    }

    private IEnumerator FadeSceneIn()
    {
        float time = 0;
        float duration = 2.5f;

        while (time < duration)
        {
            ScreenFadeManager.Instance.SetFade(1 - (time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        ScreenFadeManager.Instance.SetFade(0);
    }

    private IEnumerator FadeSceneOut()
    {
        float time = 0;
        float duration = 2.5f;

        while (time < duration)
        {
            ScreenFadeManager.Instance.SetFade(time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        ScreenFadeManager.Instance.SetFade(1);
    }
}
