namespace BLL.Model.Settings
{
    public class TournamentAssistantOptions
    {
        public string EventId { get; set; }
        public int FirstTo { get; set; }
        public int PlayerNicknameCutoff { get; set; }
        public bool IsOffline { get; set; }
        public ApiOptions Api { get; set; }
    }
}
