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
        public string _difficultyName;

        public DifficultyContainer(int difficultyLevel, string difficultyName = default)
        {
            DifficultyLevel = difficultyLevel;
            DifficultyName = difficultyName;
        }

        public DifficultyContainer()
        {
            DifficultyName = string.Empty;
            _difficultyLevel = 1;
        }

        public string ShowMe => $"Difficulté: {DifficultyLevel}{(DifficultyName.IsDefault() ? string.Empty : " " + DifficultyName)}";

        public string ToString() => $"{DifficultyLevel}{(DifficultyName.IsDefault() ? string.Empty : " " + DifficultyName)}";

    }
}
