﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Knapsack_Problem
{
    public static class Scheduler
    {
        public static List<Participant> Participants = new List<Participant>();
        public static List<CourseType> CourseTypes = new List<CourseType>();
        public static List<Realization> Realizations = new List<Realization>();
        public static List<Room> Rooms = new List<Room>();

        public static int ParticipantsAmount { get; set; } = 500;
        public static int CourseTypesAmount { get; set; } = 6;
        public static int RealizationsAmount { get; set; } = 40;
        public static int RoomsAmount { get; set; } = 8;

        public static int ParticipantsIndexJump { get; set; } = ParticipantsAmount / 8;
        public static int RealizationsIndexJump { get; set; } = RealizationsAmount / 8;
        public static int RoomsIndexJump { get; set; } = RoomsAmount / 4;
        public static double FinalProfit { get; set; }

        public static void GenerateInputData()
        {
            for (var i = 0; i < ParticipantsAmount; i++)
            {
                Participant.GenerateParticipants();
            }

            for (var i = 0; i < ParticipantsAmount; i++)
            {
                Participants[i].GeneratePreferences();
            }

            CourseType.GetCourseTypes();

            for (var i = 0; i < RealizationsAmount; i++)
            {
                Realization.GenerateRealization();
            }

            for (var i = 0; i < RoomsAmount; i++)
            {
                Room.GenerateRoom();
            }
        }


        public static double CalculateFinalProfit()
        {
            FinalProfit = 0;
            foreach (var r in Realizations)
            {
                if (r.RoomId != -1)
                {
                    FinalProfit += r.GetFitnessFunction();
                }
            }

            return FinalProfit;
        }



        public static void WriteRealizationsToFile() 
        {

            var fileName = "Realizations.txt";
            var path = @"..\..\_ResultsTxt\" + fileName;

            var realizationsAssigned = 0;
            var output = String.Empty;

            foreach (var r in Realizations)
            {
                output = output + r.PrintRealizationIo() + "\n";
            }

            foreach (var r in Realizations)
            {
                if (r.RoomId != -1) realizationsAssigned++;
            }

            output = output + "\n\n\n" + "Realizations assigned: " + realizationsAssigned + "/" + RealizationsAmount;

            File.WriteAllText(path, output);
        }

        public static void WriteScheduleToFile() 
        {
            var fileName = "Room_Schedule.txt";
            var path = @"..\..\_ResultsTxt\" + fileName;

            var output = String.Empty;


            foreach (var r in Rooms)
            {
                output += r.PrintRoomIo() + "\n";
                output += "\n-----------------------------------------------------\n";
            }

            File.WriteAllText(path, output);
        }

        public static void WriteFinalProfitToFile(string output) 
        {
            var fileName = "Final_Profit.txt";
            var path = @"..\..\_ResultsTxt\" + fileName;

            File.WriteAllText(path, output);
        }

        public static void WriteParticipantsToFile() 
        {
            var fileName = "Participants_Courses.txt";
            var path = @"..\..\_ResultsTxt\" + fileName;

            var output = String.Empty;

            foreach (var p in Participants)
            {
                output = output + p.PrintParticipantIo() + "\n";
            }

            File.WriteAllText(path, output);
        }
        
    }
}