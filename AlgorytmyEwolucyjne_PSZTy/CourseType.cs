namespace Knapsack_Problem
{
    public class CourseType
    {
        public int CourseTypeId { get; set; }
        public int CourseLength{ get; set; }                // in hours
        public int MaxParticipantAmount { get; set; }
        public int CostForHiringLecturer { get; set; }
        public int CourseCost { get; set; }                   // for full course

        public CourseType(int courseLength, int maxParticipantAmount, int costForHiringLecturer, int courseCost)
        {
            CourseTypeId = Scheduler.CourseTypes.Count;
            CourseLength = courseLength;
            MaxParticipantAmount = maxParticipantAmount;
            CostForHiringLecturer = costForHiringLecturer;
            CourseCost = courseCost;
        }

        public static void GetCourseTypes()
        {

            Scheduler.CourseTypes.Add(new CourseType(7, 30, 10000, 900));
            Scheduler.CourseTypes.Add(new CourseType(6, 20, 8000, 750));
            Scheduler.CourseTypes.Add(new CourseType(5, 35, 8000, 540));
            Scheduler.CourseTypes.Add(new CourseType(4, 60, 8000, 500));
            Scheduler.CourseTypes.Add(new CourseType(3, 45, 6000, 540));
            Scheduler.CourseTypes.Add(new CourseType(2, 40, 3000, 300));
        }
    }
}
