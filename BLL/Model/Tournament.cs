using BLL.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Model
{
    public class Tournament
    {
        public Tournament() { }
        public Tournament(TournamentRequest tournamentRequest)
        {
            var tournament = tournamentRequest.tournament;
            Id = tournament.id;
            Name = tournament.name;
            Url = tournament.url;
            Description = tournament.description;
            TournamentType = tournament.tournament_type;
            State = tournament.state;
            ParticipantsCount = tournament.participants_count;
            Teams = tournament.teams;
            GameName = tournament.game_name;

            if (tournament.participants.Any())
                Participants = PrepareParticipants(tournament.participants);
            if(tournament.matches.Any())
                Matches = PrepareMatches(tournament.matches, Participants);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string TournamentType { get; set; }
        public string State { get; set; }
        public int ParticipantsCount { get; set; }
        public bool Teams { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Match> Matches { get; set; }
        public string GameName { get; set; }

        private List<Participant> PrepareParticipants(IEnumerable<ParticipantRequest> participants)
        {
            var preparedParticipants = new List<Participant>();
            foreach (var participant in participants)
            {
                preparedParticipants.Add(new Participant(participant));
            }

            return preparedParticipants;
        }

        private List<Match> PrepareMatches(IEnumerable<MatchRequest> matches, IEnumerable<Participant> participants)
        {
            var preparedMatches = new List<Match>();
            foreach (var match in matches)
            {
                preparedMatches.Add(new Match(match));
            }

            return MatchParticipantsWithMatches(preparedMatches, participants);
        }

        private List<Match> MatchParticipantsWithMatches(IEnumerable<Match> matches, IEnumerable<Participant> participants)
        {
            foreach (var match in matches)
            {
                var player1 = participants.FirstOrDefault(c => c.Id == match.Player1Id);
                if (player1 != null)
                    match.Player1 = player1;

                var player2 = participants.FirstOrDefault(c => c.Id == match.Player2Id);
                if (player2 != null)
                    match.Player2 = player2;
            }

            return matches.ToList();
        }
    }
}
