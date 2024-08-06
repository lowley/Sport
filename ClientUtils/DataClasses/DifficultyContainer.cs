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
    public partial class DifficultyContainer<T> : ObservableObject 
    {
        [ObservableProperty]
        public int _difficultyLevel;
        
        [ObservableProperty]
        public T _difficultyName;

        public DifficultyContainer(int difficultyLevel, T difficultyName = default)
        {
            DifficultyLevel = difficultyLevel;
            DifficultyName = difficultyName;
        }

        public string ToString() => $"{DifficultyLevel}{(DifficultyName.IsDefault() ? string.Empty : " " + DifficultyName)}";

    }
}
