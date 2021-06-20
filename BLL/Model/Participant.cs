using BLL.Model.DTO;

namespace BLL.Model
{
    public class Participant
    {
        public Participant() { }
        public Participant(ParticipantRequest participantRequest)
        {
            var participant = participantRequest.participant;
            Id = participant.id;
            Name = participant.name;
            Seed = participant.seed;
            Active = participant.active;
            DisplayName = participant.display_name;
            CheckedIn = participant.checked_in;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Seed { get; set; }
        public bool Active { get; set; }
        public string DisplayName { get; set; }
        public bool CheckedIn { get; set; }
    }
}
