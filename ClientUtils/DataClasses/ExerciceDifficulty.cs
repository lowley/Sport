using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using LanguageExt;

namespace ClientUtilsProject.DataClasses
{
    public partial class ExerciceDifficulty : SportEntity, IEquatable<ExerciceDifficulty>
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        [NotifyPropertyChangedFor(nameof(DisplayedName))]
        public Int32? _difficultyLevel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        [NotifyPropertyChangedFor(nameof(ShowName))]
        [NotifyPropertyChangedFor(nameof(DisplayedName))]
        public Option<string> _difficultyName;
        
        public Guid ExerciceId { get; set; }
        
        public string DisplayedName
            => string.IsNullOrEmpty(ShowMeShort) ? "Nouvelle" : ShowMeShort;
        
        [ObservableProperty]
        public Exercise _exercice;
        
        public ExerciceDifficulty(
            Int32? difficultyLevel = default, 
            Exercise? exercice = default,
            string? difficultyName = default)
        {
            DetectShowMeShortChanges();
            var theExercise = exercice ?? new Exercise();  
            
            DifficultyLevel = difficultyLevel;
            Exercice = theExercise;
            DifficultyName = difficultyName == null ? Option<string>.None : Option<string>.Some(difficultyName);
            Id = Guid.NewGuid();
            ExerciceId = theExercise.Id;
        }

        public ExerciceDifficulty()
        {
            DetectShowMeShortChanges();
            Exercice = new Exercise();
            DifficultyName = Option<string>.None;
            Id = Guid.NewGuid();
            ExerciceId = Guid.NewGuid();
        }

        public ExerciceDifficulty(Exercise exercice)
        {
            DetectShowMeShortChanges();
            DifficultyName = "Kg";
            DifficultyLevel = 0;
            Exercice = exercice;
            Id = Guid.NewGuid();
            ExerciceId = exercice.Id;
        }
        
        public ExerciceDifficulty(string exerciceName, int level)
        {
            DetectShowMeShortChanges();
            DifficultyName = "Kg";
            DifficultyLevel = level;
            Exercice = new Exercise
            {
                ExerciseName = exerciceName,
                Id = Guid.NewGuid()
            };
            Id = Guid.NewGuid();
            ExerciceId = Exercice.Id;
        }
        
        public ExerciceDifficulty(int level, string difficultyName)
        {
            DetectShowMeShortChanges();
            DifficultyName = difficultyName;
            DifficultyLevel = level;
            Exercice = new Exercise();
            Id = Guid.NewGuid();
            ExerciceId = Exercice.Id;
        }

        public void DetectShowMeShortChanges()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(DifficultyLevel))
                {
                    if (Exercice is null)
                        return;
                    
                    Exercice.RaisePropertyChanged(nameof(Exercice.ExerciseDifficulties));
                }
            };
        }
        

        public string ShowMe
        {
            get {
                var name = DifficultyName.Match(
                    Some: v => v,
                    None: () => string.Empty
                );
                return $"Difficulté: {DifficultyLevel}{name}";
            }
        }

        public string ShowMeShort
        {
            get
            {
                var name = DifficultyName.Match(
                    Some: v => v,
                    None: () => string.Empty
                );
                return $"{DifficultyLevel}{name}";
            }
        }

        public string ShowName
        {
            get
            {
                var name = DifficultyName.Match(
                    Some: v => v,
                    None: () => string.Empty
                );
                return $"{name}";
            }
        }

        public string ToString() => $"{DifficultyLevel}{(DifficultyName.IsDefault() ? string.Empty : " " + DifficultyName)}";

        #region equality check
        public virtual bool Equals(Object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            return Equals((ExerciceDifficulty)other);
        }

        public bool Equals(ExerciceDifficulty other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Exercice is null)
                return false;
            
            return DifficultyLevel == other.DifficultyLevel
                && DifficultyName.Equals(other.DifficultyName)
                && Exercice.ExerciseName.Equals(other.Exercice.ExerciseName);
        }

        public static bool operator ==(ExerciceDifficulty left, ExerciceDifficulty right)
        {
            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ExerciceDifficulty left, ExerciceDifficulty right) => !(left == right);

        public override int GetHashCode()
        {
            return HashCode.Combine(DifficultyLevel, DifficultyName, Exercice.ExerciseName);
        }

        #endregion equality check
    }
}
