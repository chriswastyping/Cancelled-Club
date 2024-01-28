using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour

{
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TMP_InputField playerName;
    public bool gameStarted = false;
    public bool gameOver = false;
    public bool gameWin = false;
    public bool gameLose = false;
    public GameObject titleScreen;
    public GameObject Background;
    private int Totalscore = 0;
    public float timeLeft = 300;
    List<Audience> Audience = new List<Audience>();
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject winScreen;
    public GameObject loseScreen;
    public static GameManager Instance;
    List<Card> CurrentAvailableCards;
    bool isHardestDifficulty;
    bool gameEnded;

    public Sprite Dad;

    public Sprite Programmer;

    public Sprite Country;

    public Sprite Witty;

    public Sprite Religious;

    public AudioClip Title;

    public AudioClip PickingCard;

    public AudioClip GameOver;

    public AudioClip Win;

    public AudioClip Boss;

    public AudioClip BRUH;

    public AudioClip GetOffTheStage;

    public AudioClip GodDoesNotApprove;

    public AudioClip Hiss;

    public AudioClip HolyCrap;

    public AudioClip IsThatIt;

    public AudioClip ApplaudTheFunnyPerson;

    public AudioClip CelebrateFunny;

    public AudioClip DamnnThatsFunny;

    public AudioClip GodApproves;

    public AudioClip IGetIt;

    public AudioClip JollyGoodShow;

    public AudioClip OnYouMadeIt;

    public AudioClip PerfectApplause;

    public AudioClip YourPrettyGood;

    public List<AudioClip> BadSounds = new List<AudioClip>();
    public List<AudioClip> GoodSounds = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        BadSounds.Add(Boss);
        BadSounds.Add(BRUH);
        BadSounds.Add(GetOffTheStage);
        BadSounds.Add(Hiss);
        BadSounds.Add(HolyCrap);
        BadSounds.Add(IsThatIt);
        GoodSounds.Add(ApplaudTheFunnyPerson);
        GoodSounds.Add(CelebrateFunny);
        GoodSounds.Add(DamnnThatsFunny);
        GoodSounds.Add(GodApproves);
        GoodSounds.Add(IGetIt);
        GoodSounds.Add(JollyGoodShow);
        GoodSounds.Add(OnYouMadeIt);
        GoodSounds.Add(PerfectApplause);
        GoodSounds.Add(YourPrettyGood);

        Camera.main.GetComponent<AudioSource>().clip = Title;
        Camera.main.GetComponent<AudioSource>().Play();

        CheckHighScore();
        UpdateHighScoreDisplay();

        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted == true)
        
        {
            if (!gameOver) 
            {
                RunTimer();
            }
        
        }


        if (timeLeft <= 0 || isHardestDifficulty && Totalscore < 0) 
        { 
            gameOver = true;
        
        }

        if (gameOver && !gameEnded)
        {
            gameEnded = true;

            if (Totalscore > 100)
            {
                gameWin = true;
                GameWin();

            }


            else
            {
                gameLose = true;
                GameLose();
            }
        }
    }  
        

    public void StartGame(bool isHard)
    {
        Camera.main.GetComponent<AudioSource>().clip = PickingCard;
        Camera.main.GetComponent<AudioSource>().Play();

        CurrentAvailableCards = Constants.Cards.ToList();
        gameStarted = true;
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        scoreText.text = playerName.text + "'s Score : " + Totalscore;
        // LoadScene

        isHardestDifficulty = isHard;
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
        isHardDifficulty = isHardestDifficulty;
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
            newAudience.Balloon = audienceObjects[i].transform.GetChild(0).gameObject;
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
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Balloon"))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        }

        int score = 0;
        int highestPreference = 0;
        int lowestPreference = isHardestDifficulty ? Constants.HARD_SCORE_ARRAY.Last() : Constants.EASY_SCORE_ARRAY.Last();
        int midPointPreference = 0;
        Audience higherPointAudience = null;
        Audience lowerPointAudience = null;
        Audience midPointAudience = null;
        List<int> preferences =new List<int>();

        foreach (var audience in Audience)
        {
            score = score + audience.JokePreferences[joke];
            preferences.Add(audience.JokePreferences[joke]);

            if(highestPreference < audience.JokePreferences[joke])
            {
                highestPreference = audience.JokePreferences[joke];
                higherPointAudience = audience;
            }

            if (lowestPreference > audience.JokePreferences[joke])
            {
                lowestPreference = audience.JokePreferences[joke];
                lowerPointAudience = audience;
            }
        }

        preferences.RemoveAll(x => x == 0);
        preferences.Sort();
        preferences.Reverse();

        midPointPreference = preferences[(int)(preferences.Count - 1) / 2];

        if (Audience.Any(x => x.JokePreferences[joke] == midPointPreference && x != higherPointAudience && x != lowerPointAudience))
        {
            midPointAudience = Audience.First(x => x.JokePreferences[joke] == midPointPreference && x != higherPointAudience && x != lowerPointAudience);
        }

        higherPointAudience.Balloon.GetComponent<Renderer>().enabled = true;
        higherPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
        higherPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Emoji>().SetLevel(AudienceScoreEnum.High);

        if (midPointAudience != null)
        {
            midPointAudience.Balloon.GetComponent<Renderer>().enabled = true;
            midPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
            midPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Emoji>().SetLevel(AudienceScoreEnum.Medium);
        }

        lowerPointAudience.Balloon.GetComponent<Renderer>().enabled = true;
        lowerPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
        lowerPointAudience.Balloon.transform.GetChild(0).gameObject.GetComponent<Emoji>().SetLevel(AudienceScoreEnum.Low);
  
        if(isHardestDifficulty)
        {
            if (score > Constants.HARD_SCORE_ARRAY.Max() * Audience.Count / 2)
            {
                var clip = GoodSounds[UnityEngine.Random.Range(0, GoodSounds.Count)];
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                var clip = BadSounds[UnityEngine.Random.Range(0, BadSounds.Count)];
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else
        {
            if (score > Constants.EASY_SCORE_ARRAY.Max() * Audience.Count / 2)
            {
                var clip = GoodSounds[UnityEngine.Random.Range(0, GoodSounds.Count)];
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                var clip = BadSounds[UnityEngine.Random.Range(0, BadSounds.Count)];
                var audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
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
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   

    void GameWin() 
    {
        Background.gameObject.SetActive(true);
        winScreen.gameObject.SetActive(true);
        winText.SetText("CONGRATULATIONS, " + playerName.text);

        Card1.SetActive(false);
        Card2.SetActive(false);
        Card3.SetActive(false);

        Camera.main.GetComponent<AudioSource>().clip = Win;
        Camera.main.GetComponent<AudioSource>().Play();
    }

    void GameLose()
    {
        Camera.main.GetComponent<AudioSource>().clip = GameOver;
        Camera.main.GetComponent<AudioSource>().Play();

        Card1.SetActive(false);
        Card2.SetActive(false);
        Card3.SetActive(false);

        Background.gameObject.SetActive(true);
        loseScreen.gameObject.SetActive(true);
        loseText.SetText("SORRY, " + playerName.text);
    }


}


