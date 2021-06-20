namespace BLL.Model.RequestModel
{
    public class UpdateMatchRequest
    {
        public UpdateMatchRequest() { }
        public UpdateMatchRequest(UpdateMatchRequestValues updateMatchRequestValues)
        {
            match = updateMatchRequestValues;
        }

        public UpdateMatchRequest(string scoreString, int winnerId = 0)
        {
            match = new UpdateMatchRequestValues(scoreString, winnerId);
        }

        public UpdateMatchRequestValues match { get; set; }
    }

    public class UpdateMatchRequestValues
    {
        public UpdateMatchRequestValues() { }
        public UpdateMatchRequestValues(string scoreString, int winnerId = 0)
        {
            scores_csv = scoreString;
            if (winnerId > 0)
                winner_id = winnerId;
        }
        public string scores_csv { get; set; }
        public int winner_id { get; set; }
    }
}
