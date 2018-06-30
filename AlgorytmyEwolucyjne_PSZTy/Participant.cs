using System.Collections.Generic;

namespace Knapsack_Problem
{
    public class Participant
    {
       // static Random rand = new Random();

        public int ParticipantId { get; set; }
        public Dictionary<int, int> Declarations; // id of course type, id of course realization (-1 if none)
        public List<int> Preferences; // id of other participants with whom, participant wants to be in group


        public Participant()
        {
            Declarations = new Dictionary<int, int>();
            Preferences = new List<int>();
        }

        public static void GenerateParticipants()
        {
            var temp = new Participant {ParticipantId = Scheduler.Participants.Count};


            for (var i = 0; i < Scheduler.CourseTypesAmount; i++)
            {
                var courseType = RandomInitializer.Rand.Next(12) - 6;
                if (courseType >= 0)
                {
                    if (!temp.Declarations.ContainsKey(courseType))
                        temp.Declarations.Add(courseType, -1);
                }
            }

            if (temp.Declarations.Count == 0)
                GenerateParticipants();
            else
                Scheduler.Participants.Add(temp);

        }

        public void GeneratePreferences()
        {
            var friendsAmount = RandomInitializer.Rand.Next(6);

            for (var i = 0; i < friendsAmount; i++)
            {
                var preferedParticipant = RandomInitializer.Rand.Next(Scheduler.ParticipantsAmount + 100) - 100;
                if (preferedParticipant >= 0)
                {
                    if (!Preferences.Contains(preferedParticipant))
                        Preferences.Add(preferedParticipant);
                }
            }
        }


        public string PrintParticipantIo()
        {
            var output = ""
                    + "Participant ID #" + ParticipantId + "\nCourse you are enrolled on:";
            foreach (var m in Declarations)
            {
                if (m.Value != -1)
                {
                    var tempRoomId = Scheduler.Realizations[m.Value].RoomId;
                    if (tempRoomId != -1)
                    {
                        output += "\n\t\t Realization: #" + m.Value + ",";
                        output += "\tRoom: #" + tempRoomId;
                    }
                }
            }
            return output + "\n";
        }

    }
}