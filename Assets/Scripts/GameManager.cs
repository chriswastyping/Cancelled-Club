using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour

{
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TMP_InputField playerName;
    public bool gameStarted = false;
    public GameObject titleScreen;
    public GameObject Background;
    private int Totalscore = 0;
    public float timeLeft = 300;
    List<Audience> Audience = new List<Audience>();
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    List<Card> CurrentAvailableCards;
  



    // Start is called before the first frame update
    void Start()
    {
        CheckHighScore();
        UpdateHighScoreDisplay();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted == true)
        {
            RunTimer();

        }

        if (timeLeft <= 0) 
        { 
            gameStarted = false;
        
        }
    }  
        

    public void StartGame(bool isHard)
    {
        CurrentAvailableCards = Constants.Cards.ToList();
        gameStarted = true;
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        scoreText.text = playerName.text + "'s Score : " + Totalscore;
        // LoadScene

        titleScreen.gameObject.SetActive(false);
        Background.gameObject.SetActive(false);
        GenerateAudience(isHard);
        var cards = GetCards();
        SetCards(cards);
        
        
    }

    private void SetCards(List<Card> chosenCards)
    {
        Card1.SetActive(true);
        Card2.SetActive(true);
        Card3.SetActive(true);

        var UICards = GameObject.FindGameObjectsWithTag("Card");

        for(int i = 0; i < chosenCards.Count; i++)
        {
           UICards[i].GetComponent<TextMeshPro>().text = chosenCards[i].Joke;
           UICards[i].transform.parent.gameObject.GetComponent<UICard>().JokeType = chosenCards[i].JokeType;  
        }
    }

    private void GenerateAudience(bool isHardDifficulty)
    {
        var maxEnum = Enum.GetNames(typeof(JokeTypesEnum)).Length;
        var audienceObjects = GameObject.FindGameObjectsWithTag("Audience");

        for (int i = 0; i < audienceObjects.Length; i++)
        {
            var jokeDictionary = new Dictionary<JokeTypesEnum, int>();
            List<int> arrayCopy;

            if (!isHardDifficulty)
            {
                arrayCopy = Constants.EASY_SCORE_ARRAY.ToList();
            }
            else
            {
                arrayCopy = Constants.HARD_SCORE_ARRAY.ToList();
            }

            for (int y = 0; y < maxEnum; y++)
            {
                if (arrayCopy.Count > 0)
                {
                    var nextIndex = UnityEngine.Random.Range(0, arrayCopy.Count);
                    var jokeValue = arrayCopy[nextIndex];
                    jokeDictionary.Add((JokeTypesEnum)y, jokeValue);
                    arrayCopy.RemoveAt(nextIndex);
                }
                else
                {
                    jokeDictionary.Add((JokeTypesEnum)y, 0);
                }
            }

            var newAudience = audienceObjects[i].AddComponent<Audience>();
            newAudience.JokePreferences = jokeDictionary;
            Audience.Add(newAudience);
        }
    }

    public List<Card> GetCards()
    {
        List<Card> cardsToPlay = new List<Card>();
        var tempList = CurrentAvailableCards.ToList();

        while(cardsToPlay.Count < 3)
        {
            var selectedCard = tempList[UnityEngine.Random.Range(0, tempList.Count - 1)];
            cardsToPlay.Add(selectedCard);
            tempList.RemoveAll(x => x.JokeType == selectedCard.JokeType);
            CurrentAvailableCards.Remove(selectedCard);
        }

        return cardsToPlay;
    }

    public void PlayCard(JokeTypesEnum joke)
    {
        int score = 0;

        foreach (var audience in Audience)
        {
            score = score + audience.JokePreferences[joke];
        }

        Totalscore += score;

        scoreText.text = playerName.text + "'s Score : " + Totalscore;

        CheckHighScore();
        UpdateHighScoreDisplay();

        if(gameStarted)
        {
           var newCards = GetCards();
           SetCards(newCards);
        }

    }

    // Checks current Totalscore against saved Highscore
    void CheckHighScore()
    {
        if (Totalscore > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Totalscore);
            PlayerPrefs.SetString("Playername", playerName.text);
        }
    }

    // Updates high Totalscore display with high Totalscore & name of player who scored it
    void UpdateHighScoreDisplay()
    {
        highscoreText.text = $"Highscore: {PlayerPrefs.GetInt("Highscore")} {PlayerPrefs.GetString("Playername")}";
    }
    void RunTimer()
    {
        timeLeft -= Time.deltaTime;
        var timeSpan = TimeSpan.FromSeconds(timeLeft);
        timerText.SetText($"Time: {timeSpan.Minutes}: {timeSpan.Seconds}");
    }
    //Clears high score & player name when GUI button pressed
    public void ClearHighScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.SetString("Playername", "");
        UpdateHighScoreDisplay();
    }

}


      