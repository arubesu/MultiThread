using Newtonsoft.Json;

namespace Multithreading
{
	public class Pokemon
	{
		public int Id { get; set; }
		public PokemonName Name { get; set; }
		public PokemonBase Base { get; set; }
	}

	public class PokemonName
	{
		public string English { get; set; }
		public string Japanese { get; set; }
		public string Chinese { get; set; }
		public string French { get; set; }
	}

	public class PokemonBase
	{
		public int HP { get; set; }
		public int Attack { get; set; }
		public int Defense { get; set; }
		[JsonProperty("Sp. Attack")]
		public int SPAttack { get; set; }
		[JsonProperty("Sp. Defense")]
		public int SPDefense { get; set; }
		public int Speed { get; set; }
	}
}
