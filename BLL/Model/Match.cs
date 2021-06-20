using BLL.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Model
{
    public class Match
    {
        public Match() { }
        public Match(MatchRequest matchRequest)
        {
            var match = matchRequest.match;

            Id = match.id;
            TournamentId = match.tournament_id;
            TextIdentifier = match.identifier;
            Player1Id = match.player1_id;
            Player2Id = match.player2_id;
            Round = match.round;
            State = match.state;

            if (!string.IsNullOrWhiteSpace(match.scores_csv))
            {
                var deserializedScore = DeserializeScore(match.scores_csv);
                Player1Score = deserializedScore.score1;
                Player2Score = deserializedScore.score2;
            }
        }

        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TextIdentifier { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public Participant Player1 { get; set; }
        public Participant Player2 { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public int Round { get; set; }
        public string State { get; set; }
        public bool IsSideSwitched { get; set; }

        private (int score1, int score2) DeserializeScore(string scoreString)
        {
            var scores = scoreString.Split(',');
            var deserializedScores = scores.FirstOrDefault().Split('-');

            var p1Score = int.Parse(deserializedScores[0]);
            var p2Score = int.Parse(deserializedScores[1]);

            return (p1Score, p2Score);
        }
    }
}
