using BLL.Interfaces;
using BLL.Model;
using BLL.Model.DTO;
using BLL.Model.RequestModel;
using BLL.Model.Settings;
using RestSharp;
using System;

namespace Services.External
{
    public class ChallongeApiService : ITournamentApiControlService
    {
        private readonly RestClient _restClient;
        private readonly TournamentAssistantOptions _appSettings;
        public ChallongeApiService(TournamentAssistantOptions appSettings)
        {
            _appSettings = appSettings;
            _restClient = new RestClient(appSettings.Api.Address);
        }

        public Tournament GetTournament(string tournamentId, bool includeParticipants, bool includeMatches)
        {
            var request = PrepareRestRequest($"tournaments/{tournamentId}.json", Method.GET);
            request.AddParameter("include_matches", includeMatches ? 1 : 0);
            request.AddParameter("include_participants", includeParticipants ? 1 : 0);

            var result = _restClient.Get<TournamentRequest>(request);

            if (!result.IsSuccessful)
                throw new Exception("Coś poszło nie tak przy pobieraniu turnieju.");

            return new Tournament(result.Data);
        }

        /// <summary>
        /// Update match on Challonge
        /// </summary>
        /// <param name="match">Has to enter in original form, not side-switched!</param>
        /// <returns></returns>
        public bool UpdateMatch(Match match)
        {
            var restRequest = PrepareRestRequest($"tournaments/{match.TournamentId}/matches/{match.Id}.json", Method.PUT);
            var winnerId = DetermineWinner(match);
            var scoreString = PrepareScoreString(match);

            restRequest.AddJsonBody(new UpdateMatchRequest(scoreString, winnerId));

            var result = _restClient.Put(restRequest);

            if (!result.IsSuccessful)
                throw new Exception("Coś poszło nie tak przy aktualizacji wyniku.");

            return result.IsSuccessful;
        }

        private RestRequest PrepareRestRequest(string resourceUrl, Method httpMethod)
        {
            var restRequest = new RestRequest(resourceUrl, httpMethod);
            restRequest.AddQueryParameter("api_key", _appSettings.Api.ApiKey);

            return restRequest;
        }

        private int DetermineWinner(Match match)
        {
            if (match.Player1Score >= _appSettings.FirstTo)
                return match.Player1Id;

            if (match.Player2Score >= _appSettings.FirstTo)
                return match.Player2Id;

            return 0;
        }

        private string PrepareScoreString(Match match)
        {
            return $"{match.Player1Score}-{match.Player2Score}";
        }
    }
}
