using System;
using System.Collections.Generic;

namespace HRM_Track_Merger.ExerciseData {
    public interface IExercise {
        List<DataPoint> GetDataPoints();
        List<DataPoint> GetDataPointsWithPartialData();
        List<Lap> GetLaps();
        UserData GetUserData();
        Summary GetTotals();
        bool IsAirPressureDataAvailable { get; }
        bool IsAltitudeDataAvailable { get; }
        bool IsBalanceDataAvailable { get; }
        bool IsCadenceDataAvailable { get; }
        bool IsCyclingDataAvailable { get; }
        bool IsImperialSystemUsed { get; }
        bool IsPedallingIndexDataAvailable { get; }
        bool IsPowerDataAvailable { get; }
        bool IsSpeedDataAvailable { get; }
    }
}
