using ClientUtilsProject.DataClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUtilsProject.DataClasses
{
    public partial class ExerciseEntity : ObservableObject
    {
        [ObservableProperty]
        public string _exerciseName;

        [ObservableProperty]
        public ObservableCollection<DifficultyContainer> _exerciseDifficulties;

        public ExerciseEntity()
        {
            ExerciseName = string.Empty;
            ExerciseDifficulties = [];
        }
    }
}
