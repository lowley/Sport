﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace WIPClientVM
{
    public partial class MainPageVM : ObservableObject
    {
        [ObservableProperty]
        public int _zcounter;

        [RelayCommand]
        public async Task AddOne()
        {
            Zcounter++;
        }



    }
}
