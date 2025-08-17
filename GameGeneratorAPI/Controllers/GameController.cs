using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GameGeneratorAPI.Models;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    [HttpGet("generate")]
    public ActionResult<List<List<Match>>> GenerateSchedule([FromQuery] int numberOfTeams)
    {
        if (numberOfTeams < 2)
        {
            return BadRequest("Antal lag måste vara minst 2.");
        }

        // Hantera udda antal lag genom att lägga till ett "Bye"-lag
        bool isOdd = numberOfTeams % 2 != 0;
        int teamsCount = isOdd ? numberOfTeams + 1 : numberOfTeams;
        
        var teams = Enumerable.Range(1, numberOfTeams).Select(i => $"Lag {i}").ToList();
        if (isOdd)
        {
            teams.Add("Bye"); // Vilande lag
        }

        var rounds = new List<List<Match>>();
        int numRounds = teamsCount - 1;

        // "Round-robin" algoritmen
        for (int round = 0; round < numRounds; round++)
        {
            var currentRoundMatches = new List<Match>();
            for (int i = 0; i < teamsCount / 2; i++)
            {
                var homeTeam = teams[i];
                var awayTeam = teams[teamsCount - 1 - i];

                // Inkludera inte matcher mot "Bye"-laget
                if (homeTeam != "Bye" && awayTeam != "Bye")
                {
                    currentRoundMatches.Add(new Match { HomeTeam = homeTeam, AwayTeam = awayTeam });
                }
            }
            rounds.Add(currentRoundMatches);

            // Rotera lagen (alla utom det första)
            var lastTeam = teams.Last();
            teams.RemoveAt(teamsCount - 1);
            teams.Insert(1, lastTeam);
        }

        return Ok(rounds);
    }
}