using ClientUtilsProject.DataClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUtilsProject.Entities
{
    public partial class ExerciseEntity : ObservableObject
    {
        [ObservableProperty]
        public string _exerciseName;

        [ObservableProperty]
        public DifficultyContainer<string> _exerciseDifficulty;

        public ExerciseEntity()
        {
            ExerciseName = string.Empty;
            ExerciseDifficulty = new DifficultyContainer<string>(10, "Kg");
        }
    }
}
