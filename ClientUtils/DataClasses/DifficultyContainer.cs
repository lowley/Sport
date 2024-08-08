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
    public partial class DifficultyContainer : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
        public int _difficultyLevel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowMe))]
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

    }
}
