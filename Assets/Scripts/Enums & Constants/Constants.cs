using System.Collections.Generic;
using UnityEngine.UIElements;

public static class Constants
{
    public static List<int> EASY_SCORE_ARRAY = new List<int> { 0, 0, 1, 2, 3 };

    public static List<int> HARD_SCORE_ARRAY = new List<int> { -3, -2, -1, 1, 2, 3 };

    public static List<Card> Cards = new List<Card>
    {
        new Card(JokeTypesEnum.DadJokes, "I’m scared for the calendar, its days are numbered."),
        new Card(JokeTypesEnum.DadJokes, "What do you call a factory that makes average products? A satisfactory"),
        new Card(JokeTypesEnum.DadJokes, "How much does a polar bear weigh? Enough to BREAK THE ICE!"),
        new Card(JokeTypesEnum.CountryJokes, "What's the best thing about Switzerland? I don't know, but the flag is a big plus."),
        new Card(JokeTypesEnum.CountryJokes, "I only have one shoe tied… why’s that? Well it’s because the label says TAIWAN!"),
        new Card(JokeTypesEnum.CountryJokes, "My friend likes Patrick likes to party, I always say “It’s time to break it DownPatrick!"),
        new Card(JokeTypesEnum.ReligiousJokes, "God made humanity… took one look and said “What a mistake”"),
        new Card(JokeTypesEnum.ReligiousJokes, "Jesus hanging on the cross looks down… “Hanging for these $%&!”"),
        new Card(JokeTypesEnum.ReligiousJokes, "Guy lives in house during flood, his son says get out dad, man says no and that God will save him... boat appears, dad says no... dad drowns... he enters heaven and asks God \"Why?\", God said Bro I gave you a boat and a supported son c'mon."),
        new Card(JokeTypesEnum.ProgrammerJokes, "Let’s JAVA good time"),
        new Card(JokeTypesEnum.ProgrammerJokes, "My eyesight is top tier, because I can C#"),
        new Card(JokeTypesEnum.ProgrammerJokes, "Game James always promote UNITY"),
        new Card(JokeTypesEnum.WittyJokes, "What did the Christmas Cracker say to the Fortune Cookie? Man you've got fortunes and well wishes, I've just got shitty jokes"),
        new Card(JokeTypesEnum.WittyJokes, "Santa lost a bet to the mafia... the mob boss said \"YULE be sorry if ya don't pay up"),
        new Card(JokeTypesEnum.WittyJokes, "My son is dyslexic, he wrote his Christmas list for SATAN "),
    };

}