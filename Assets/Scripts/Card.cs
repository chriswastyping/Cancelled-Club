public class Card
{
	public JokeTypesEnum JokeType { get; set; }

	public string Joke {  get; set; }

	public Card(JokeTypesEnum jokeType, string joke)
	{
		JokeType = jokeType; 
		Joke = joke;
	}
}