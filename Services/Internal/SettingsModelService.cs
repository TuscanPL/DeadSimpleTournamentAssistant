using BLL.Interfaces.Settings;
using BLL.Model.Settings;

namespace Services.Internal
{
    public class SettingsModelService : ISettingsModelFactory, ISettingsModelUpdate
    {
        private TournamentAssistantOptions _tournamentAssistantOptions;

        public SettingsModelService(TournamentAssistantOptions options)
        {
            _tournamentAssistantOptions = options;
        }
        public TournamentAssistantOptions GetSettings()
        {
            return _tournamentAssistantOptions;
        }

        public void UpdateSettings(TournamentAssistantOptions options)
        {
            _tournamentAssistantOptions = options;
        }
    }
}
