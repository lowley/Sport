﻿using CommunityToolkit.Mvvm.ComponentModel;
using LanguageExt;
using LanguageExt.TypeClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUtilsProject.DataClasses
{
    public partial class ExerciceDifficulty : ObservableObject, IEquatable<ExerciceDifficulty>
    {
        public Guid Id;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        private Int32 _difficultyLevel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        [NotifyPropertyChangedFor(nameof(ShowName))]
        private Option<string> _difficultyName;

        [ObservableProperty]
        private Exercise _exercice;

        public ExerciceDifficulty(
            Int32 difficultyLevel, 
            Exercise exercice,
            string? difficultyName = default)
        {
            DifficultyLevel = difficultyLevel;
            DifficultyName = difficultyName == null ? Option<string>.None : Option<string>.Some(difficultyName);
            Exercice = exercice;
        }

        public ExerciceDifficulty(Exercise exercice)
        {
            DifficultyName = "Kg";
            DifficultyLevel = 0;
            Exercice = exercice;
            Id = Guid.NewGuid();
        }
        
        public ExerciceDifficulty(int level, string exerciceName)
        {
            DifficultyName = "Kg";
            DifficultyLevel = level;
            Exercice = new Exercise
            {
                ExerciseName = exerciceName
            };
            Id = Guid.NewGuid();
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
