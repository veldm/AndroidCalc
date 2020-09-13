using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidCalc
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Input.WidthRequest = ProceedButton.Width - 30;
        }

        private void Calculate(object sender, EventArgs e)
        {
            
        }
    }
}
