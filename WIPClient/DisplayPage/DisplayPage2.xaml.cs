namespace WIPClient.DisplayPage;

public partial class DisplayPage2 : ContentPage
{
    public int Counter { get; set; }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Counter++;
        OnPropertyChanged(nameof(Counter));
    }

    public DisplayPage2()
	{
		InitializeComponent();
	}

    
}