using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoji : MonoBehaviour
{
    public Sprite HighScoreSprite;

    public Sprite MidScoreSprite;

    public Sprite LowScoreSprite;

    public Dictionary<AudienceScoreEnum, Sprite> ScoreSprites;

    // Start is called before the first frame update
    void Start()
    {
        ScoreSprites = new Dictionary<AudienceScoreEnum, Sprite>() {

        { AudienceScoreEnum.High, HighScoreSprite },
        { AudienceScoreEnum.Medium, MidScoreSprite },
        { AudienceScoreEnum.Low, LowScoreSprite },
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(AudienceScoreEnum audienceScoreEnum)
    {
        GetComponent<SpriteRenderer>().sprite = ScoreSprites[audienceScoreEnum];
    }
}
