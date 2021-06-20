using BLL.Interfaces.Settings;
using BLL.Model.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly string _defaultBrowseDiskLocation = "C:\\Users";
        private TournamentAssistantOptions _options;
        private SingleWindow _singleWindow;
        private MainWindow _mainWindow;
        private ISettingsModelFactory _settingsModelFactory;
        private ISettingsModelUpdate _settingsUpdateService;
        public SettingsWindow(ISettingsModelFactory settingsModelFactory, ISettingsModelUpdate settingsUpdateService, SingleWindow singleWindow, MainWindow mainWindow)
        {
            _settingsModelFactory = settingsModelFactory;
            _settingsUpdateService = settingsUpdateService;
            _singleWindow = singleWindow;
            _mainWindow = mainWindow;
            RefreshSettings();

            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            IsOfflineCheckbox.IsChecked = _options.IsOffline;

            FillTextBox(ApiKeyTextBox, _options.Api.ApiKey);
            FillTextBox(EventIdTextBox, _options.EventId);
            FillTextBox(FirstToTextBox, _options.FirstTo);
            FillTextBox(BasePathTextBox, _options.Files.FilesBasePath);
            FillTextBox(Player1NameFilenameTextBox, _options.Files.Player1NicknameFileName);
            FillTextBox(Player2NameFilenameTextBox, _options.Files.Player2NicknameFileName);
            FillTextBox(Player1ScoreFilenameTextBox, _options.Files.Player1ScoreFileName);
            FillTextBox(Player2ScoreFilenameTextBox, _options.Files.Player2ScoreFileName);
            FillTextBox(RoundTextFilenameTextBox, _options.Files.RoundTextFileName);
            FillTextBox(PlayerNicknameCutoffTextBox, _options.PlayerNicknameCutoff);

            DisableApiSettings(_options.IsOffline);
        }

        private void DisableApiSettings(bool isOffline)
        {
            ApiKeyTextBox.IsEnabled = !isOffline;
            EventIdTextBox.IsEnabled = !isOffline;
            FirstToTextBox.IsEnabled = !isOffline;
        }

        private void RefreshSettings()
        {
            _options = _settingsModelFactory.GetSettings();
        }

        private void FillTextBox(TextBox textBox, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
                textBox.Text = content;
        }

        private void FillTextBox(TextBox textBox, int content)
        {
            if (content > 0) textBox.Text = content.ToString();
        }

        private TournamentAssistantOptions PrepareNewOptions()
        {
            var newOptions = new TournamentAssistantOptions();
            newOptions.EventId = EventIdTextBox.Text;
            newOptions.FirstTo = ParseInt(FirstToTextBox.Text);
            newOptions.IsOffline = (bool)IsOfflineCheckbox.IsChecked;
            newOptions.PlayerNicknameCutoff = ParseInt(PlayerNicknameCutoffTextBox.Text);
            
            newOptions.Api = new ApiOptions();
            newOptions.Api.ApiKey = ApiKeyTextBox.Text;

            newOptions.Files = new FilePathOptions();
            newOptions.Files.FilesBasePath = BasePathTextBox.Text;
            newOptions.Files.Player1NicknameFileName = Player1NameFilenameTextBox.Text;
            newOptions.Files.Player2NicknameFileName = Player2NameFilenameTextBox.Text;
            newOptions.Files.Player1ScoreFileName = Player1ScoreFilenameTextBox.Text;
            newOptions.Files.Player2ScoreFileName = Player2ScoreFilenameTextBox.Text;
            newOptions.Files.RoundTextFileName = RoundTextFilenameTextBox.Text;

            return newOptions;
        }

        private int ParseInt(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return -1;
            var intToReturn = int.Parse(content);

            return intToReturn > 0 ? intToReturn : -1;
        }

        private void UpdateSettings()
        {
            var newOptions = PrepareNewOptions();
            _settingsUpdateService.UpdateSettings(newOptions);

            RefreshSettings();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSettings();

            if (_options.IsOffline)
            {
                _singleWindow.Show();
            }
            else
            {
                _mainWindow.Show();
            }

            this.Hide();
        }

        private void IsOfflineCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            DisableApiSettings((bool)checkBox.IsChecked);
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = _defaultBrowseDiskLocation;
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    BasePathTextBox.Text = dialog.FileName;
                }
            }
        }
    }
}
