using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AndroidCalc
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Input.WidthRequest = DeviceDisplay.MainDisplayInfo.Width - 50;
            Input.Focus();
        }

        private void Calculate(object sender, EventArgs e)
        {
            
        }

        #region Подстановка знаков в ТБ
        private void _Add(object sender, EventArgs e)
        {
            Input.Text += "+";
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Sub(object sender, EventArgs e)
        {
            Input.Text += "—"; //Это не минус, а тире! (ascii 0151)
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Mul(object sender, EventArgs e)
        {
            Input.Text += "*";
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Div(object sender, EventArgs e)
        {
            Input.Text += "/";
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Pow(object sender, EventArgs e)
        {
            Input.Text += "^";
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _uSub(object sender, EventArgs e)
        {
            Input.Text += "-"; // Унарный минус
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }
        #endregion
    }
}
