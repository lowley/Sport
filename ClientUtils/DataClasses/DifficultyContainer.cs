using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class DifficultyContainer : ObservableObject, IEquatable<DifficultyContainer>
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        public int _difficultyLevel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        [NotifyPropertyChangedFor(nameof(ShowMeShort))]
        public Option<string> _difficultyName;

        public DifficultyContainer(int difficultyLevel, Option<string> difficultyName = default)
        {
            DifficultyLevel = difficultyLevel;
            DifficultyName = difficultyName;
        }

        public DifficultyContainer()
        {
            DifficultyName = default;
            _difficultyLevel = 1;
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

            return Equals((DifficultyContainer)other);
        }

        public bool Equals(DifficultyContainer other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return DifficultyLevel == other.DifficultyLevel
                && DifficultyName.Equals(other.DifficultyName);
        }

        public static bool operator ==(DifficultyContainer left, DifficultyContainer right)
        {
            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(DifficultyContainer left, DifficultyContainer right) => !(left == right);
        
        #endregion equality check
    }
}
