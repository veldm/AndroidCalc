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
        //bool sqrt = false;
        //int Length;
        //bool SelfChange = false;

        public MainPage()
        {
            InitializeComponent();
            Input.WidthRequest = DeviceDisplay.MainDisplayInfo.Width - 50;
            Input.Focus();
        }

        private void Calculate(object sender, EventArgs e)
        {
            try
            {
                ResultOutputLabel.Text = "Результат: ";
                double Result = AndroidCalc.Logic.Calculate(Input.Text);
                ResultOutputLabel.Text += Result.ToString();
            }
            catch(Exception Ex)
            {
                DisplayAlert("Ошибка", Ex.Message, "OK");
            }            
        }

        #region Подстановка знаков в ТБ
        private void _Add(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "+");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Sub(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "—");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Mul(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "*");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Div(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "/");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Pow(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "^");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _Sqrt(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "√");
            //sqrt = true;
            //Length = Input.Text.Length;
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _OS(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, "(");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }

        private void _CS(object sender, EventArgs e)
        {
            Input.Text = Input.Text.Insert(Input.CursorPosition, ")");
            Input.Focus();
            Input.CursorPosition = Input.Text.Length;
        }
        #endregion

        //private void InputTextChanged(object sender, EventArgs e)
        //{
        //    if (SelfChange) SelfChange = false;
        //    else if (sqrt) try
        //        {
        //            if (char.IsDigit(Input.Text[Length]) || Input.Text[Length] == '.')
        //            {
        //                string s = "" + Input.Text[Length] + (char)772;
        //                Input.Text.Remove(Length - 1);
        //                Input.Text += s;
        //                SelfChange = true;
        //            }
        //        }
        //        catch { sqrt = false; }
        //}

        private void RetFocus(object sender, EventArgs e)
        {
            Input.Focus();
        }
    }
}
