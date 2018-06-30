
namespace Knapsack_Problem
{
    public class Realization
    {
        public int RealizationId { get; set; }
        public int CourseTypeId { get; set; }
        public int CourseLength { get; set; }                 // in hours
        public int MaxParticipantAmount { get; set; }
        public int EnrolledParticipantAmount { get; set; }
        public int CostForHiringLecturer { get; set; }
        public int CourseCost { get; set; }                  // for all semester
        public int RoomId { get; set; }                       // room where course will take place
        public double FitnessFunction { get; set; }          //value of fitness function for second match

        public static void GenerateRealization()
        {
            var temp = new Realization {RealizationId = Scheduler.Realizations.Count};
            var whichCourse = RandomInitializer.Rand.Next(Scheduler.CourseTypesAmount);
            temp.CourseTypeId = whichCourse;

            temp.CourseLength = Scheduler.CourseTypes[whichCourse].CourseLength;
            temp.MaxParticipantAmount = Scheduler.CourseTypes[whichCourse].MaxParticipantAmount;
            temp.EnrolledParticipantAmount = 0;
            temp.CostForHiringLecturer = Scheduler.CourseTypes[whichCourse].CostForHiringLecturer;
            temp.CourseCost = Scheduler.CourseTypes[whichCourse].CourseCost;
            temp.RoomId = -1;      // unassigned at the beginning

            Scheduler.Realizations.Add(temp);
        }

        public double RecalculateFitnessFunction()
        {
            if (RoomId == -1) { FitnessFunction = -10000; return FitnessFunction; }
            var thisRoom = Scheduler.Rooms[RoomId];

            var profit = EnrolledParticipantAmount * CourseCost;
            var loss = CostForHiringLecturer + thisRoom.LeaseCost * CourseLength;

            double fitnessFuntionValue = profit - loss;
            FitnessFunction = fitnessFuntionValue;

            return FitnessFunction;
        }


        public string PrintRealizationIo()
        {
            var output = ""
                    + "Realization:      " + RealizationId
                    + "\n\tFitness:      " + RecalculateFitnessFunction().ToString("#.##")
                    + "\n\tcourseType    " + CourseTypeId
                    + "\n\tcourseLength  " + CourseLength;
            if (RoomId != -1) output += "\n\tLease cost    " + Scheduler.Rooms[RoomId].LeaseCost;
            else output += "\n\tLease cost    " + "----";

            output = output
                    + "\n\tmaxPartAmount " + MaxParticipantAmount
                    + "\n\tenrolled      " + EnrolledParticipantAmount
                    + "\n\thiringCost    " + CostForHiringLecturer
                    + "\n\tcourseCost    " + CourseCost
                    + "\n\troomId        " + RoomId + "\n\n";
            return output;
        }


        public double GetFitnessFunction()
        {
            return FitnessFunction;
        }
    }
}