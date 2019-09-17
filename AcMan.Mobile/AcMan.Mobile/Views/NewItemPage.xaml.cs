using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AcMan.Mobile.Models;
using AcMan.HttpApiClient;

namespace AcMan.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Activity Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Activity
            {
                Caption = "",
				Start = DateTime.Now
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }
    }
}