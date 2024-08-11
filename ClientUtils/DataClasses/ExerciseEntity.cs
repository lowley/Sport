using ClientUtilsProject.DataClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;

namespace ClientUtilsProject.DataClasses
{
    public partial class ExerciseEntity : ObservableObject, IEquatable<ExerciseEntity>
    {
        [ObservableProperty]
        private string _exerciseName;
        
        [ObservableProperty]
        private ObservableCollection<DifficultyContainer> _exerciseDifficulties;
        
        public ExerciseEntity()
        {
            ExerciseName = string.Empty;
            ExerciseDifficulties = [];
        }

        #region equality check

        public virtual bool Equals(Object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            return this.Equals(other as ExerciseEntity);
        }

        public bool Equals(ExerciseEntity? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ExerciseName == other.ExerciseName
                && ExerciseDifficulties.Equals(other.ExerciseDifficulties);
        }

        public static bool operator ==(ExerciseEntity left, ExerciseEntity right)
        {
            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ExerciseEntity left, ExerciseEntity right) => !(left == right);

        #endregion equality check

    }
}
