using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Audience : MonoBehaviour
{
    Dictionary<JokeTypesEnum, int> _jokePreferences;
    


    public Dictionary<JokeTypesEnum, int> JokePreferences
    {
        get
        {
            return _jokePreferences;
        }

        set
        {
            _jokePreferences = value;
            SetImage();
        }

    }

    public GameObject Balloon { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetImage()
    {
        var higherJoke = JokePreferences.Max(x => x.Value);
        var higherJokeType = JokePreferences.First(x => x.Value == higherJoke).Key;

        switch(higherJokeType)
        {
            case JokeTypesEnum.DadJokes:

                GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<GameManager>().Dad;

                break;

            case JokeTypesEnum.WittyJokes:

                GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<GameManager>().Witty;

                break;

            case JokeTypesEnum.ProgrammerJokes:

                GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<GameManager>().Programmer;

                break;

            case JokeTypesEnum.ReligiousJokes:

                GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<GameManager>().Religious;

                break;

            case JokeTypesEnum.CountryJokes:

                GetComponent<SpriteRenderer>().sprite = GameObject.Find("GameManager").GetComponent<GameManager>().Country;

                break;

        }
    }
}
