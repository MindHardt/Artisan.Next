using Arklens.Next.Entities;
using Bogus;

namespace Artisan.Next.Client.Contracts.Chits;

public record MonsterChitModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required HealthModel Health { get; set; }
    public required CharacteristicsModel Characteristics { get; set; }
    public required DefenseModel Defense { get; set; }
    public required StatsModel Stats { get; set; }
    public required string Attacks { get; set; }
    public required string Features { get; set; }
    public required string ImageUrl { get; set; }
    public required Alignment Alignment { get; set; }

    public static MonsterChitModel Fake(Faker faker) => new()
    {
        Id = Guid.NewGuid().ToString(),
        Name = faker.Name.FirstName(),
        Health = new HealthModel
        {
            MaxHealth = Random.Shared.Next(1, 101),
            CurrentHealth = Random.Shared.Next(1, 101)
        },
        Characteristics = new CharacteristicsModel
        {
            Str = Random.Shared.Next(-5, 6),
            Dex = Random.Shared.Next(-5, 6),
            Con = Random.Shared.Next(-5, 6),
            Int = Random.Shared.Next(-5, 6),
            Wis = Random.Shared.Next(-5, 6),
            Cha = Random.Shared.Next(-5, 6)
        },
        Defense = new DefenseModel
        {
            Primary = Random.Shared.Next(10, 21),
            Touch = Random.Shared.Next(10, 21),
            Unaware = Random.Shared.Next(10, 21)
        },
        Stats = new StatsModel
        {
            Fort = Random.Shared.Next(0, 11),
            Reac = Random.Shared.Next(0, 11),
            Will = Random.Shared.Next(0, 11),
            Conc = Random.Shared.Next(0, 11),
            Perc = Random.Shared.Next(0, 11),
            Init = Random.Shared.Next(0, 11)
        },
        Attacks = string.Join(' ', faker.Lorem.Words(Random.Shared.Next(10, 30))),
        Features = string.Join(' ', faker.Lorem.Words(Random.Shared.Next(10, 30))),
        ImageUrl = faker.Image.LoremFlickrUrl(),
        Alignment = Alignment.AllValues.ElementAt(Random.Shared.Next(9))
    };
}