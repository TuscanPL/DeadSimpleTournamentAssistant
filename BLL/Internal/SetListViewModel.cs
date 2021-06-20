using BLL.Model;
using System;

namespace BLL.Internal
{
    public class SetListViewModel
    {
        public SetListViewModel(Match set)
        {
            if (set?.Id <= 0)
                throw new Exception($"Null set passed into View!");

            SetId = set.Id;
            Identifier = set.TextIdentifier;

            if (set.Player1 != null && set.Player2 != null)
            {
                Player1Name = set.Player1.Name;
                Player1Score = set.Player1Score;
                Player2Score = set.Player2Score;
                Player2Name = set.Player2.Name;
                SetSeed = set.Player1.Seed > set.Player2.Seed
                    ? set.Player2.Seed: set.Player1.Seed;

                IsSuggested = SetSeed <= 5;
            }
        }
        public bool IsSuggested { get; set; }
        public int SetId { get; set; }
        public string Identifier { get; set; }
        public string Player1Name { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public string Player2Name { get; set; }
        public int SetSeed { get; set; }
        public string RoundText { get; set; }
    }
}
