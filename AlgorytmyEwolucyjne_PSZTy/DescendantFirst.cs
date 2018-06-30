namespace Knapsack_Problem
{
    public class DescendantFirst : IDescendantMethods
    {
        public int ParticipantId { get; set; }
        public int RealizationId { get; set; }
        public double FitnessFuntionValue { get; set; }

        public DescendantFirst(int participantId, int realizationId)
        {
            ParticipantId = participantId;
            RealizationId = realizationId;
        }


        public double CalculateFitnessFunction()
        {
            var preferedCoursesNumber = Scheduler.Participants[ParticipantId].Declarations.Count;
            var preferedParticipantsInGroup = 0;

            var thisParticipant = Scheduler.Participants[ParticipantId];
            var thisRealization = Scheduler.Realizations[RealizationId];

            FitnessFuntionValue = 0;

            if (!thisParticipant.Declarations.ContainsKey(thisRealization.CourseTypeId)) return 0;
            // this realization is not interesting for participant


            for (var i = 0; i < thisParticipant.Preferences.Count; i++)
            {
                var preferedFriend = Scheduler.Participants[thisParticipant.Preferences[i]];
                if (preferedFriend.Declarations.ContainsValue(thisRealization.CourseTypeId)) ++preferedParticipantsInGroup;
            }

            FitnessFuntionValue = (preferedCoursesNumber * 590 + (preferedParticipantsInGroup ^ 2) * 30);

            return FitnessFuntionValue;
        }
    }
}