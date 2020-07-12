﻿using ExerciseCuration.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExerciseCuration.Data
{
    public class InMemoryExerciseDb : IExerciseData
    {
        public int[] timeAmounts = { 15, 60 };
        public int[] amountRange = { 5, 30};
        public int[] exerciseAmount = { 5, 10 };
        private List<exerciseSnippet> likedExercises = new List<exerciseSnippet>();
        private List<exerciseSnippet> dislikedExercises = new List<exerciseSnippet>();
        /*
        Cardio,
        Aerobic,
        ActiveRecovery,
        StrengthTraining,
        HIIT,
        Isometric,
        Plyometric
         */
        private readonly Dictionary<workoutTypes, double> workoutPrefs = new Dictionary<workoutTypes, double>()
        {
            {workoutTypes.Cardio, 0.0 },
            {workoutTypes.Aerobic, 0.0 },
            {workoutTypes.ActiveRecovery, 0.0 },
            {workoutTypes.StrengthTraining, 0.0 },
            {workoutTypes.HIIT, 0.0 },
            {workoutTypes.Isometric, 0.0 },
            {workoutTypes.Plyometric, 0.0 },
        };
        public List<workoutTypes> workoutHistory = new List<workoutTypes>();
        private List<Exercise> exercises { get; set; }
        public InMemoryExerciseDb()
        {
            exercises = new List<Exercise>();
        }
        public Exercise add(Exercise exercise)
        {
            exercises.Add(exercise);
            return exercise;
        }

        public Exercise delete(int id)
        {
            Exercise exercise = exercises.FirstOrDefault(r => r.id == id);
            if(exercise != null)
            {
                exercises.Remove(exercise);
            }
            return exercise;
        }

        public Exercise getById(int id)
        {
            Exercise exercise = exercises.FirstOrDefault(r => r.id == id);
            if (exercise != null)
            {
                return exercise;
            }
            return null;
        }
        public int commit()
        {
            return 0;
        }

        public Exercise generateNewWorkout(difficulty difficulty)
        {
            List<workoutTypes> applicableWorkoutTypes = workoutPrefs.Keys.Where(r => workoutPrefs[r] > staticRandom.Instance.NextDouble()).ToList();
            //Set defaults if applicableGroups are null
            workoutTypes workoutType = workoutTypes.ActiveRecovery;
            if(applicableWorkoutTypes != null)
            {
                workoutType = applicableWorkoutTypes[staticRandom.Instance.Next(0, applicableWorkoutTypes.Count - 1)];
            }
            Exercise exercise = new Exercise(workoutType,timeAmounts,amountRange,staticRandom.Instance.Next(exerciseAmount[0],exerciseAmount[1]), difficulty, likedExercises, dislikedExercises);
            return exercise;
        }

        public void updateDict(exerciseSnippet target, double increment)
        {
            workoutPrefs[target.workoutType] += workoutPrefs[target.workoutType] == 0.0 && increment < 0.0 ? increment * workoutHistory.Where(r => r == target.workoutType).Count() : 0;
            if(increment > 0)
            {
                likedExercises.Add(target);
            }
            else
            {
                dislikedExercises.Add(target);
            }
        }
        public int getMax()
        {
            return exercises.Max(r => r.id);
        }
    }
}
