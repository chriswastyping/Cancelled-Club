using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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
    private int Totalscore = 0;
    public float timeLeft = 5;
    List<Audience> Audience = new List<Audience>();

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
        gameStarted = true;
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        scoreText.text = playerName.text + "'s Score : " + Totalscore;
        titleScreen.gameObject.SetActive(false);
        GenerateAudience(isHard);
        var cards = GetCards();
        Camera.main.GetComponent<CameraAnimation>().PlayAnimation();
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
        var tempList = Constants.Cards.ToList();

        while(cardsToPlay.Count < 3)
        {
            var selectedCard = tempList[UnityEngine.Random.Range(0, tempList.Count - 1)];
            cardsToPlay.Add(selectedCard);
            tempList.RemoveAll(x => x.JokeType == selectedCard.JokeType);
        }

        return cardsToPlay;
    }

    public int PlayCard(Card card)
    {
        int score = 0;

        foreach (var audience in Audience)
        {
            score = score + audience.JokePreferences[card.JokeType];
        }

        return score;

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

   
}


      