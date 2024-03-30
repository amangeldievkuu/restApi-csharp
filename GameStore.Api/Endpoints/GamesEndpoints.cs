using GameStore.Api.Dtos;
namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
  const string GetGameEndPointName = "GetGame";
  private static readonly List<GameDto> games = [
  new (
    1,
    "Street Fighter 2",
    "Fighting",
    19.99M,
    new DateOnly(1992,7,15)),
    new (
    2,
    "Final Fantasy 123",
    "Roleplaying",
    59.99M,
    new DateOnly(2010,9,30)),
    new (
    3,
    "Fifa 23",
    "Sports",
    69.99M,
    new DateOnly(1922,9,27)),
];

  public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
  {
    var group = app.MapGroup("games").WithParameterValidation();

    // GET /games
    group.MapGet("/", () => games);

    //GET /games/3
    group.MapGet("/{id}", (int id) =>
    {
      GameDto? game = games.Find(game => game.Id == id);
      return game is null ? Results.NotFound() : Results.Ok(game);
    }).WithName(GetGameEndPointName);

    //POST /games
    group.MapPost("/", (CreateGamesDto newGame) =>
    {
      GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

      games.Add(game);

      return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
    });

    //PUT games/2
    group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
    {
      var index = games.FindIndex(game => game.Id == id);

      if (index == -1)
      {
        return Results.NotFound();
      }

      games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
        );

      return Results.NoContent();
    });

    //DELETE /games/2
    group.MapDelete("/{id}", (int id) =>
    {
      games.RemoveAll(game => game.Id == id);
      return Results.NoContent();
    });

    return group;
  }
}
