using System;
namespace HRM_Track_Merger.ExerciseData {
    interface IExercise {
        System.Collections.Generic.List<HRM_Track_Merger.ExerciseData.DataPoint> GetDataPoints();
        System.Collections.Generic.List<HRM_Track_Merger.ExerciseData.DataPoint> GetDataPointsWithPartialData();
        System.Collections.Generic.List<HRM_Track_Merger.ExerciseData.Lap> GetLaps();
        HRM_Track_Merger.ExerciseData.UserData GetUserData();
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
