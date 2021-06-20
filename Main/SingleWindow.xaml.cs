using BLL.Interfaces;
using BLL.Internal;
using BLL.Model;
using BLL.Model.Settings;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SingleWindow : Window
    {
        private readonly IFileService _fileService;
        private readonly TournamentAssistantOptions _options;
        private Match _currentSet;
        public SingleWindow(TournamentAssistantOptions options, IFileService fileService)
        {
            _fileService = fileService;
            _options = options;
            InitializeComponent();
            ConfigureMainWindow();
        }

        private void ConfigureMainWindow()
        {
            _currentSet = new Match();
            _currentSet.Player1 = new Participant();
            _currentSet.Player2 = new Participant();

            PopulateDetailsMenu();
        }

        private void PopulateDetailsMenu()
        {
            if (_currentSet == null) return;

            Player1TextBox.Text = _currentSet.Player1?.Name;
            Player1ScoreTextBox.Text = _currentSet.Player1Score.ToString();
            Player2TextBox.Text = _currentSet.Player2?.Name;
            Player2ScoreTextBox.Text = _currentSet.Player2Score.ToString();
        }

        private void ChangeScoreButton_Click(object sender, RoutedEventArgs e)
        {
            var castedSender = sender as Button;
            if (castedSender == null || _currentSet == null) return;

            switch (castedSender.Name)
            {
                case nameof(AddScoreP1Button):
                    _currentSet.Player1Score += 1;
                    break;
                case nameof(AddScoreP2Button):
                    _currentSet.Player2Score += 1;
                    break;
                case nameof(RemoveScoreP1Button):
                    _currentSet.Player1Score -= 1;
                    break;
                case nameof(RemoveScoreP2Button):
                    _currentSet.Player2Score -= 1;
                    break;
                case nameof(SwitchSidesButton):
                    _currentSet = SwitchSides(_currentSet);
                    break;
                default:
                    throw new Exception("Something went wrong while attaching events to buttons. No event for this button.");
            }

            PopulateDetailsMenu();
        }

        private void PlayerNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var castedSender = sender as TextBox;
            if (castedSender == null || _currentSet == null) return;

            switch (castedSender.Name)
            {
                case nameof(Player1TextBox):
                    _currentSet.Player1.Name = Player1TextBox.Text;
                    break;
                case nameof(Player2TextBox):
                    _currentSet.Player2.Name = Player2TextBox.Text;
                    break;
                default:
                    throw new Exception("Something went wrong while attaching events to textboxes. No event for this textbox.");
            }
        }

        private Match SwitchSides(Match set)
        {
            var player = set.Player1;
            var playerScore = set.Player1Score;

            set.Player1 = set.Player2;
            set.Player1Score = set.Player2Score;
            set.Player2 = player;
            set.Player2Score = playerScore;
            set.IsSideSwitched = !set.IsSideSwitched;

            return set;
        }

        private void AppendDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSet == null) return;

            var fileDto = new FileDataDTO()
            {
                Player1Name = FormatPlayerNames(Player1TextBox.Text),
                Player1Score = Player1ScoreTextBox.Text,
                Player2Name = FormatPlayerNames(Player2TextBox.Text),
                Player2Score = Player2ScoreTextBox.Text,
                RoundText = RoundNameTextBox.Text
            };

            _fileService.AppendData(fileDto);
        }

        private string FormatPlayerNames(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName)) return string.Empty;

            var cutOff = _options.PlayerNicknameCutoff;
            if (playerName.Length >= cutOff)
                return $"{playerName.Substring(0, cutOff)}...";

            return playerName;
        }
    }
}
