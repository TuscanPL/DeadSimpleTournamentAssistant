using BLL.Model;

namespace BLL.Interfaces
{
    public interface ITournamentApiControlService
    {
        public Tournament GetTournament(string tournamentId, bool includeParticipants, bool includeMatches);
        public bool UpdateMatch(Match match);
    }
}
