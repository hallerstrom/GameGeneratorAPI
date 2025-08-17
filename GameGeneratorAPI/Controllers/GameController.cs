using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GameGeneratorAPI.Models;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    [HttpPost("generate")]
    public ActionResult<List<List<Match>>> GenerateSchedule([FromBody] List<string> teamNames)
    {
        if (teamNames == null || teamNames.Count < 2)
        {
            return BadRequest("Minst två lagnamn måste anges.");
        }

        var teams = new List<string>(teamNames);
        
        
        bool isOdd = teams.Count % 2 != 0;
        int teamsCount = teams.Count;
        if (isOdd)
        {
            teams.Add("Bye");
        }
        
        var rounds = new List<List<Match>>();
        int numRounds = teams.Count - 1;

        for (int round = 0; round < numRounds; round++)
        {
            var currentRoundMatches = new List<Match>();
            for (int i = 0; i < teams.Count / 2; i++)
            {
                var homeTeam = teams[i];
                var awayTeam = teams[teams.Count - 1 - i];

                if (homeTeam != "Bye" && awayTeam != "Bye")
                {
                    currentRoundMatches.Add(new Match { HomeTeam = homeTeam, AwayTeam = awayTeam });
                }
            }
            rounds.Add(currentRoundMatches);

            var lastTeam = teams.Last();
            teams.RemoveAt(teams.Count - 1);
            teams.Insert(1, lastTeam);
        }

        return Ok(rounds);
    }
}