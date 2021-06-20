using BLL.Model.Settings;

namespace BLL.Interfaces.Settings
{
    public interface ISettingsModelFactory
    {
        TournamentAssistantOptions GetSettings();
    }
}
