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
    public partial class Exercise : ObservableObject, IEquatable<Exercise>
    {
        [ObservableProperty]
        private string _exerciseName;
        
        [ObservableProperty]
        private ObservableCollection<ExerciceDifficulty> _exerciseDifficulties;
        
        public Exercise()
        {
            ExerciseName = string.Empty;
            ExerciseDifficulties = [];
        }

        #region equality check

        public virtual bool Equals(Object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            return this.Equals(other as Exercise);
        }

        public bool Equals(Exercise? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ExerciseName == other.ExerciseName
                && ExerciseDifficulties.Equals(other.ExerciseDifficulties);
        }

        public static bool operator ==(Exercise left, Exercise right)
        {
            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Exercise left, Exercise right) => !(left == right);

        #endregion equality check

    }
}
