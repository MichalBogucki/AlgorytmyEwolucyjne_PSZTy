namespace Knapsack_Problem
{
    public class DescendantSecond : IDescendantMethods
    {
        public int RealizationId { get; set; }
        public int RoomId { get; set; }
        public double FitnessFuntionValue { get; set; }

        public DescendantSecond(int realizationId, int roomId)
        {
            RealizationId = realizationId;
            RoomId = roomId;
        }


        public double CalculateFitnessFunction()
        {
            //---- value = (profit - loss) / number of occupied hours
            //---- profit = participant number * realization cost
            //---- loss = hiring lecturer cost + cost for room leasing for 1 hour * realization length
            var thisRealization = Scheduler.Realizations[RealizationId];
            var thisRoom = Scheduler.Rooms[RoomId];

            var profit = thisRealization.EnrolledParticipantAmount * thisRealization.CourseCost;
            var loss = thisRealization.CostForHiringLecturer + thisRoom.LeaseCost * thisRealization.CourseLength;

            FitnessFuntionValue = profit - loss;

            thisRealization.FitnessFunction = FitnessFuntionValue;

            return FitnessFuntionValue;
        }
    }
}