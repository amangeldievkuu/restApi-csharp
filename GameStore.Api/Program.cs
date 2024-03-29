using GameStore.Api;
using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
const string GetGameEndPointName = "GetGame";

List<GameDto> games = [
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

// GET /games
app.MapGet("games", () => games);


//GET /games/3
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndPointName);


//POST /games
app.MapPost("games", (CreateGamesDto newGame) =>
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
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
{
  var index = games.FindIndex(game => game.Id == id);
  games[index] = new GameDto(
    id,
    updatedGame.Name,
    updatedGame.Genre,
    updatedGame.Price,
    updatedGame.ReleaseDate
    );

  return Results.NoContent();
});


app.Run();
