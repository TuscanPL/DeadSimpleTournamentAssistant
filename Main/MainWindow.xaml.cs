using BLL.Interfaces;
using BLL.Interfaces.Settings;
using BLL.Internal;
using BLL.Model;
using BLL.Model.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITournamentApiControlService _challongeService;
        private readonly ISettingsModelFactory _settingsModelFactory;
        private readonly IFileService _fileService;
        private TournamentAssistantOptions _options => _settingsModelFactory.GetSettings();
        private Tournament _tournament;
        private IEnumerable<Match> _sets;
        private Match _currentSet;
        public MainWindow(ITournamentApiControlService challongeService, IFileService fileService, ISettingsModelFactory settingsModelFactory)
        {
            _challongeService = challongeService;
            _fileService = fileService;
            _settingsModelFactory = settingsModelFactory;
            InitializeComponent();
            ConfigureMainWindow(_options);
        }

        private void ConfigureMainWindow(TournamentAssistantOptions options)
        {
            _tournament = _challongeService.GetTournament(options.EventId, true, true);

            UpdateSets(_tournament);
            _currentSet = _sets.FirstOrDefault();

            PopulateGridMenu();
            PopulateDetailsMenu();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += RecurrentSetsUpdate;
            timer.Start();
        }

        private void SetsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SetsDataGrid.SelectedItem as SetListViewModel;
            if (selectedItem != null)
            {
                _currentSet = _sets.FirstOrDefault(c => c.Id == selectedItem.SetId);
                PopulateDetailsMenu();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            App.ServiceProvider.GetService<SettingsWindow>().Show();
        }

        private void AppendDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSet == null) return;

            var appendedSet = _currentSet;
            if (appendedSet.IsSideSwitched)
                appendedSet = SwitchSides(appendedSet);

            var fileDto = new FileDataDTO()
            {
                Player1Name = FormatPlayerNames(Player1TextBox.Text),
                Player1Score = Player1ScoreTextBox.Text,
                Player2Name = FormatPlayerNames(Player2TextBox.Text),
                Player2Score = Player2ScoreTextBox.Text,
                RoundText = RoundNameTextBox.Text
            };

            _fileService.AppendData(fileDto);
            if (IsChallongeUpdate.IsChecked.Value)
                _challongeService.UpdateMatch(appendedSet);
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
                    throw new Exception("Something went wrong. No event to bind.");
            }

            PopulateDetailsMenu();
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

        private IEnumerable<SetListViewModel> PrepareSetListView(IEnumerable<Match> sets)
        {
            var filledViews = new List<SetListViewModel>();

            foreach (var set in sets)
            {
                if (set?.Id <= 0) continue;

                filledViews.Add(new SetListViewModel(set));
            }

            return filledViews;
        }

        private void PopulateGridMenu()
        {
            SetsDataGrid.ItemsSource = PrepareSetListView(_sets);
        }

        private void PopulateDetailsMenu()
        {
            if (_currentSet == null) return;

            Player1TextBox.Text = _currentSet.Player1?.Name;
            Player1ScoreTextBox.Text = _currentSet.Player1Score.ToString();
            Player2TextBox.Text = _currentSet.Player2?.Name;
            Player2ScoreTextBox.Text = _currentSet.Player2Score.ToString();
        }

        

        private void RecurrentSetsUpdate(object sender, EventArgs e)
        {
            _tournament = _challongeService.GetTournament(_options.EventId, true, true);
            UpdateSets(_tournament);

            PopulateGridMenu();
        }

        private void UpdateSets(Tournament tournament)
        {
            if (_tournament.Matches != null && _tournament.Matches.Any())
                _sets = _tournament.Matches;
            else
                _sets = new List<Match>();
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
