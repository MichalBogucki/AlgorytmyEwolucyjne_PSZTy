using System;

namespace Knapsack_Problem
{
    public class SecondMatch
    {
        private DescendantSecond _current;
        private DescendantSecond _next;
        private DescendantSecond _swappingDescendant;
        private double _sigmaRealization;
        private double _sigmaRoom;

        private int _successAmount;            // amount of finding better descendant in last M matches
        private int _mIteration;                        // counted number of matches, MAX to 10 and reset
        private double _successRatio;          // successAmount / m (max value of M)

        private int _successAmountThread;                   // amount of finding any acceptable descendant in each calling algorithm
        private int _mThread;                               // counted number of calling algorithm
        private double _successfulDescendantRatioThread;    // successAmountThread / mThread

        private const double C1 = 0.82;
        private const double C2 = 1.2;

        public void StartAlgorithmLoop()
        {
            var output = "";
            _successfulDescendantRatioThread = 1;
            var iterationCounter = 0;
            while (_successfulDescendantRatioThread >= 0.1 || iterationCounter < 25)
            {
                _mThread = 10;
                _successAmountThread = 0;
                _successfulDescendantRatioThread = 0;
                for (var i = 0; i < _mThread; i++)
                {
                    var temp = EvolutionaryAlgorithm();
                    if (temp != null)
                    {
                        if (temp.CalculateFitnessFunction() > 1000)
                        {
                            ++_successAmountThread;
                            AssignDescendant(temp);
                        }
                    }
                }
                _successfulDescendantRatioThread = _successAmountThread / (double)_mThread;
                iterationCounter++;
                output += "Loop " + iterationCounter + ":\tFinalProfit is " + Scheduler.CalculateFinalProfit() + "\n";
                Console.WriteLine($"Final profit: {Scheduler.CalculateFinalProfit()}  i:{iterationCounter}");
            }

            //writing final profits output to a file
            try
            {
                Scheduler.WriteFinalProfitToFile(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public DescendantSecond EvolutionaryAlgorithm()
        {

            _successAmount = 0;
            _mIteration = 200;
            _sigmaRealization = 3;
            _sigmaRoom = 3;
            _successRatio = 0;

            //Random rand = new Random();
            for (var i = 0; i < _mIteration; i++)
            {
                _current = new DescendantSecond(RandomInitializer.Rand.Next(Scheduler.RealizationsAmount), 
                    RandomInitializer.Rand.Next(Scheduler.RoomsAmount));

                _current.CalculateFitnessFunction();
                if (ValidateDescendant(_current)) { break; }

                _swappingDescendant = _current;
                _current = null;
            }

            if (_current == null) return null;

            while (_sigmaRealization > 0.01)
            {
                _successRatio = 0;
                _successAmount = 0;

                for (var i = 0; i < _mIteration; i++)
                {
                    var newRealizationId = _current.RealizationId +
                            (int)(_sigmaRealization * (RandomInitializer.Rand.NextDouble() * Scheduler.RealizationsIndexJump));
                    if (newRealizationId > (Scheduler.RealizationsAmount - 1)) newRealizationId %= Scheduler.RealizationsAmount;

                    var newRoomId = _current.RoomId +
                            (int)(_sigmaRoom * (RandomInitializer.Rand.NextDouble() * Scheduler.RoomsIndexJump));
                    if (newRoomId > (Scheduler.RoomsAmount - 1)) newRoomId %= Scheduler.RoomsAmount;
                    _next = new DescendantSecond(newRealizationId, newRoomId);
                    _next.CalculateFitnessFunction();

                    if ((_next.FitnessFuntionValue > _current.FitnessFuntionValue) && ValidateDescendant(_next))
                    {
                        _current = _next;
                        ++_successAmount;
                    }
                }
                _successRatio = _successAmount / (double)_mIteration;

                SetSigmas();
            }

            return _current;
        }

        public bool ValidateDescendant(DescendantSecond descendant)
        {
            var thisRealization = Scheduler.Realizations[descendant.RealizationId];
            var thisRoom = Scheduler.Rooms[descendant.RoomId];

            thisRoom.CountOccupiedHoursNumber();

            if (thisRealization.RoomId != -1) return false;
            // this realization is already assigned to a room

            if (thisRealization.EnrolledParticipantAmount > thisRoom.SeatsNumber) return false;
            // not enough seats for participants

            var fitFlag = false;

            for (var i = 0; i < thisRoom.FreeHoursInEachDay.Length; i++)
            {
                if (thisRoom.FreeHoursInEachDay[i] >= thisRealization.CourseLength)
                {
                    fitFlag = true;
                    break;
                }

            }

            if (!fitFlag) return false;
            // not enough free hours for running realization in the room
            return true;
        }


        public void AssignDescendant(DescendantSecond descendant)
        {
            var thisRealization = Scheduler.Realizations[descendant.RealizationId];
            var thisRoom = Scheduler.Rooms[descendant.RoomId];

            thisRealization.RoomId = thisRoom.RoomId;

            var k = 0;
            var assignmentSuccessfulFlag = false;
            foreach (var daysHours in thisRoom.FreeHoursInEachDay)
            {

                if (daysHours >= thisRealization.CourseLength)
                //if(thisRoom.getFreeHoursInEachDay()[dayHours] >= thisRealization.courseLength)
                {

                    for (var j = 0; j <= (thisRoom.MaxHoursADay - thisRealization.CourseLength); j++)
                    {
                        if (thisRoom.HoursSchedule[k,j] == -1)
                        {
                            for (var x = 0; x < thisRealization.CourseLength; x++)
                            {
                                thisRoom.HoursSchedule[k,j + x] = thisRealization.RealizationId;
                            }
                            assignmentSuccessfulFlag = true;
                        }
                        if (assignmentSuccessfulFlag) break;
                    }

                    break;
                }
                ++k;
            }
            thisRoom.CountOccupiedHoursNumber();
            thisRealization.RecalculateFitnessFunction();
        }


        public void SetSigmas()
        {
            if (_successRatio > 0.2)
            {
                _sigmaRealization = C2 * _sigmaRealization;
                _sigmaRoom = C2 * _sigmaRoom;
            }
            else if (_successRatio < 0.2)
            {
                _sigmaRealization = C1 * _sigmaRealization;
                _sigmaRoom = C1 * _sigmaRoom;
            }

        }
    }
}