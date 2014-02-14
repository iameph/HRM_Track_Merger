using System.Collections.Generic;

namespace HRM_Track_Merger.ExerciseData {
    interface IExerciseCollection {
        List<ExerciseData.IExercise> GetExercises();
    }
}
