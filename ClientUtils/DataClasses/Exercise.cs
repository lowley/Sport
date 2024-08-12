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
        public Guid Id { get; set; }
        
        [ObservableProperty]
        private string _exerciseName;
        
        [ObservableProperty]
        private ObservableCollection<ExerciceDifficulty> _exerciseDifficulties;
        
        public Exercise()
        {
            ExerciseName = string.Empty;
            ExerciseDifficulties = [];
            Id = Guid.NewGuid();
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

        /**
         * Ne compare que les noms
         */
        public bool SameAs(Exercise? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(ExerciseName, other.ExerciseName, StringComparison.CurrentCultureIgnoreCase);
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
