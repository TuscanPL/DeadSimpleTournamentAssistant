using BLL.Interfaces;
using BLL.Interfaces.Settings;
using BLL.Internal;
using BLL.Model.Settings;
using System;
using System.IO;

namespace Services.External
{
    public class FileService : IFileService
    {
        private readonly ISettingsModelFactory _settingsModelFactory;
        private readonly string _baseDefaultDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Scores";
        private string _baseDirectory;
        private TournamentAssistantOptions _options => _settingsModelFactory.GetSettings();
        public FileService(ISettingsModelFactory settingsModelFactory)
        {
            _settingsModelFactory = settingsModelFactory;
            RefreshSettings();

            Directory.CreateDirectory(_baseDirectory);
        }
        public void AppendData(FileDataDTO fileData)
        {
            RefreshSettings();

            File.WriteAllText($"{_baseDirectory}\\{_options.Files.Player1NicknameFileName}.txt", fileData.Player1Name);
            File.WriteAllText($"{_baseDirectory}\\{_options.Files.Player2NicknameFileName}.txt", fileData.Player2Name);
            File.WriteAllText($"{_baseDirectory}\\{_options.Files.Player1ScoreFileName}.txt", fileData.Player1Score);
            File.WriteAllText($"{_baseDirectory}\\{_options.Files.Player2ScoreFileName}.txt", fileData.Player2Score);
            File.WriteAllText($"{_baseDirectory}\\{_options.Files.RoundTextFileName}.txt", fileData.RoundText);
        }

        private void RefreshSettings()
        {
            _baseDirectory = !string.IsNullOrWhiteSpace(_options.Files.FilesBasePath) ? _options.Files.FilesBasePath : _baseDefaultDirectory;
        }
    }
}
