using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<Audience> Audience = new List<Audience>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame(bool isHard)
    {
        GenerateAudience(isHard);
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

    public int PlayCard(Card card)
    {
        int score = 0;

        foreach (var audience in Audience)
        {
            score = score + audience.JokePreferences[card.JokeType];
        }

        return score;
    }
}


      