namespace Knapsack_Problem
{
    public class FirstMatch
    {
        private DescendantFirst _current;
        private DescendantFirst _next;
        private double _sigmaParticipant;
        private double _sigmaRealization;

        private int _successAmount;           // amount of finding better descendant in last M matches
        private int _mIteration;              // counted number of matches, MAX to 10 and reset
        private double _successRatio;         // successAmount / m (max value of M)

        private int _successAmountThread;            // amount of finding any acceptable descendant in each calling algorithm
        private int _mThread;                        // counted number of calling algorithm
        private double _successfulDescendantRatioThread;    // successAmountThread / mThread

        private const double C1 = 0.82;
        private const double C2 = 1.2;

        public void StartAlgorithmLoop()
        {
            _successfulDescendantRatioThread = 1;
            var iterationCounter = 0;

            while (_successfulDescendantRatioThread > 0.01 || iterationCounter < 25)
            {
                _mThread = 500;
                _successAmountThread = 0;
                _successfulDescendantRatioThread = 0;

                for (var i = 0; i < _mThread; i++)
                {
                    var temp = EvolutionaryAlgorithm();
                    if (temp != null)
                    {
                        ++_successAmountThread;
                        AssignDescendant(temp);
                    }
                }
                _successfulDescendantRatioThread = _successAmountThread / (double)_mThread;
                iterationCounter++;
            }
        }


        public DescendantFirst EvolutionaryAlgorithm()
        {
            SetAlgorithmVariables();
            TryGeneratingBetterDescendant();

            if (_current == null)
            {
                return null;
            }

            while (_sigmaRealization > 0.2)
            {
                _successRatio = 0;
                _successAmount = 0;

                for (var i = 0; i < _mIteration; i++)
                {
                    var newParticipantId = GenerateNewParticipantIdFromCurrentDescendant();
                    var newRealizationId = GenerateNewRealizationIdFromCurrentDescendant();

                    GenerateNextDescendant(newParticipantId, newRealizationId);
                    AssignNextToCurrentIfBecameBetter();
                }
                _successRatio = _successAmount / (double)_mIteration;
                SetSigmas();
            }

            return _current;
        }


        private void SetAlgorithmVariables()
        {
            _successAmount = 0;
            _mIteration = 200;
            _sigmaParticipant = 3;
            _sigmaRealization = 3;
            _successRatio = 0.0;
        }

        private void TryGeneratingBetterDescendant()
        {
            for (var i = 0; i < _mIteration; i++)
            {
                _current = new DescendantFirst(RandomInitializer.Rand.Next(Scheduler.ParticipantsAmount),
                    RandomInitializer.Rand.Next(Scheduler.RealizationsAmount));
                _current.CalculateFitnessFunction();

                if (!IsValidDescendant(_current))
                    _current = null;
            }
        }


        private int GenerateNewParticipantIdFromCurrentDescendant()
        {
            var newParticipantId = _current.ParticipantId +
                                   (int)(_sigmaParticipant * (RandomInitializer.Rand.NextDouble() * Scheduler.ParticipantsIndexJump));

            if (newParticipantId > (Scheduler.ParticipantsAmount - 1))
            {
                newParticipantId %= Scheduler.ParticipantsAmount;
            }

            return newParticipantId;
        }


        private int GenerateNewRealizationIdFromCurrentDescendant()
        {
            var newRealizationId = _current.RealizationId +
                                   (int)(_sigmaRealization * (RandomInitializer.Rand.NextDouble() * Scheduler.RealizationsIndexJump));

            if (newRealizationId > (Scheduler.RealizationsAmount - 1))
            {
                newRealizationId %= Scheduler.RealizationsAmount;
            }

            return newRealizationId;
        }
        

        private void GenerateNextDescendant(int newParticipantId, int newRealizationId)
        {
            _next = new DescendantFirst(newParticipantId, newRealizationId);
            _next.CalculateFitnessFunction();
        }


        private void AssignNextToCurrentIfBecameBetter()
        {
            if (_next.FitnessFuntionValue > _current.FitnessFuntionValue && IsValidDescendant(_next))
            {
                _current = _next;
                ++_successAmount;
            }
        }

        public bool IsValidDescendant(DescendantFirst descendant)
        {
            var thisParticipant = Scheduler.Participants[descendant.ParticipantId];
            var thisRealization = Scheduler.Realizations[descendant.RealizationId];

            if ((!thisParticipant.Declarations.ContainsKey(thisRealization.CourseTypeId)))//Course has already  been chosen
                return false;

            if (thisParticipant.Declarations[thisRealization.CourseTypeId] != -1)
                return false;

            if ((thisRealization.MaxParticipantAmount - thisRealization.EnrolledParticipantAmount <= 0))
                return false;
            // no available seats for realization

            return true;

        }


        public void AssignDescendant(DescendantFirst descendant)
        {
            var thisParticipant = Scheduler.Participants[descendant.ParticipantId];
            var thisRealization = Scheduler.Realizations[descendant.RealizationId];
            thisRealization.EnrolledParticipantAmount += 1;
            //thisParticipant.declarations.Add(thisRealization.courseTypeId, thisRealization.realizationId);
            thisParticipant.Declarations[thisRealization.CourseTypeId] = thisRealization.RealizationId;
        }



        public void SetSigmas()
        {
            if (_successRatio > 0.2)
            {
                _sigmaRealization = C2 * _sigmaRealization;
                _sigmaParticipant = C2 * _sigmaRealization;
            }
            else if (_successRatio < 0.2)
            {
                _sigmaRealization = C1 * _sigmaRealization;
                _sigmaParticipant = C1 * _sigmaRealization;
            }
        }

    }
}