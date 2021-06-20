using BLL.Model.Settings;

namespace BLL.Interfaces.Settings
{
    public interface ISettingsModelUpdate
    {
        void UpdateSettings(TournamentAssistantOptions options);
    }
}
