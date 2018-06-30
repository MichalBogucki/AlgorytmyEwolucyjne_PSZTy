using System;

namespace Knapsack_Problem
{
    public class Room
    {
        public int MaxHoursADay = 10;

        public int RoomId { get; set; }
        public int SeatsNumber { get; set; }
        public int LeaseCost { get; set; }  // for 1 hour
        public int OccupiedHoursNumber { get; set; }    // number of hours when there are courses in the room
        public int DayQuantity { get; set; } = 3;
        public int[,] HoursSchedule = new int[3,10]; //new int[dayQuantity][maxHoursADay];    // if -1, room is free in that hour
        public int[] FreeHoursInEachDay;

        public static int[] PossibleSeatsNumbersInRooms = { 30, 35, 40, 55, 60 };
        public static int[] PossibleLeasingCost = { 150, 200, 250, 300, 350 };


        public Room()
        {
            OccupiedHoursNumber = 0;
            FreeHoursInEachDay = new[] { MaxHoursADay, MaxHoursADay, MaxHoursADay };
            for (var i = 0; i < DayQuantity; i++)
            {
                for (var j = 0; j < MaxHoursADay; j++)
                {
                    HoursSchedule[i,j] = -1;
                }
            }
        }

        public void CountOccupiedHoursNumber()
        {
            OccupiedHoursNumber = 0;
            FreeHoursInEachDay = new[] { MaxHoursADay, MaxHoursADay, MaxHoursADay };
            var k = 0;

            for (var i = 0; i < DayQuantity; i++)
            {
                for (var j = 0; j < MaxHoursADay; j++)
                {
                    if (HoursSchedule[i,j] != -1)
                    {
                        OccupiedHoursNumber += 1;
                        FreeHoursInEachDay[k] -= 1;
                    }
                }
                k += 1;
            }
        }

        public static void GenerateRoom()
        {
            var temp = new Room
            {
                RoomId = Scheduler.Rooms.Count,
                OccupiedHoursNumber = 0,
                FreeHoursInEachDay = new[] {10, 10, 10}
            };

            var n = RandomInitializer.Rand.Next(5);
            temp.SeatsNumber = PossibleSeatsNumbersInRooms[n];
            var m = RandomInitializer.Rand.Next(5);
            temp.LeaseCost = PossibleLeasingCost[m];

            Scheduler.Rooms.Add(temp);
        }

        public String PrintRoomIo()
        {
            var output = ""
                + "Room #" + RoomId
                + "\n\tSeats: " + SeatsNumber
                + "\n\tLease cost: " + LeaseCost
                + "\n\tOccupied hours: " + OccupiedHoursNumber + "\n"
                + "Hours schedule:";

            for (var i = 0; i < DayQuantity; i++)
            {
                output += "\nDay " + (i + 1);
                for (var j = 0; j < MaxHoursADay; j++)
                {
                    output += "\n\t" + (j + 8) + ":15 \t->\t#" + HoursSchedule[i,j];
                    if (HoursSchedule[i,j] != -1)
                    {
                        output += "\t\tppl " + Scheduler.Realizations[HoursSchedule[i,j]].EnrolledParticipantAmount
                                + "/" + SeatsNumber;
                    }
                }
            }
            return output;
        }
    }
}