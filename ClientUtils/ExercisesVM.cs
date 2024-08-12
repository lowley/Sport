using ClientUtilsProject.DataClasses;
using ClientUtilsProject.DataClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.VM;

public partial class ExercisesVM : ObservableObject
{
    [ObservableProperty] public static ObservableCollection<Exercise> _exercices = [];


    public ExercisesVM()
    {
       
    }
}
