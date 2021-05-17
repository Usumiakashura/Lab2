using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double min = double.MaxValue;
        double max = double.MinValue;
        private object obj = null;

        public MainWindow()
        {
            InitializeComponent();
            textBoxX.GotFocus += textBox_GotFocus;
            textBoxP.GotFocus += textBox_GotFocus;
        }
        void textBox_GotFocus(object sender, EventArgs e)
        {
            obj = sender;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            labelResult.Content = "";
            if (textBoxX.Text == "") textBoxX.Text = "0";
            if (textBoxP.Text == "") textBoxP.Text = "0";
            double x = Convert.ToDouble(textBoxX.Text);
            double p = Convert.ToDouble(textBoxP.Text);
            double res = 0;
            double fx = 0;
            bool fl4 = false;

            if (radioButtonSin.IsChecked == true)
                fx = Math.Sin(x);
            if (radioButtonCos.IsChecked == true)
                fx = Math.Cos(x);
            if (radioButtonTan.IsChecked == true)
                fx = Math.Tan(x);

            string str = "";

            if (x > Math.Abs(p))
            {
                str += "Ветвь 1:\tx > |p|";
                res = 2 * fx * Math.Pow(x, 3) + 3 * Math.Pow(p, 2);
            }
            else if (x > 3 && x < Math.Abs(p))
            {
                str += "Ветвь 2:\t3 < x < |p|";
                res = Math.Abs(fx - p);
            }
            else if (x == Math.Abs(p))
            {
                str += "Ветвь 3:\t x = |p|";
                res = Math.Pow((fx - p), 2);
            }
            else
            {
                str += "Значение не определено";
                fl4 = true;
            }

            if (fl4 == false)
            {
                labelResult.Content = $"f(x) = {fx:0.###}\n";
                if (res < min) min = res;
                if (res > max) max = res;
            }
            labelResult.Content += str;
            if (checkBoxMin.IsChecked == true)
                labelResult.Content += $"\nmin = {min:0.###}";
            if (checkBoxMax.IsChecked == true)
                labelResult.Content += $"\nmax = {max:0.###}";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (obj != null)
            {
                ((TextBox)obj).MaxLength = 9; //максимальное кол-во вводимых вручную символов                
                if ((((TextBox)obj).Text == "" || ((TextBox)obj).Text == "0")     //если попытаемся в начале ввести много нулей - оставить один "0"
                    && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    ((TextBox)obj).Text = "0";
                else  //если в текстбоксе есть ноль и мы пытаемся ввести другое число - убрать ноль перед числом
                if (((TextBox)obj).Text == "0" && (e.Key != Key.D0 || e.Key != Key.NumPad0))
                    ((TextBox)obj).Text = "";
                else  //если вводим с клавиатуры "," когда на текстбоксе пусто или ноль - вывести "0,"
                if (((e.Key == Key.OemComma || e.Key == Key.Decimal) && ((TextBox)obj).Text == "0") ||
                    ((e.Key == Key.OemComma || e.Key == Key.Decimal) && ((TextBox)obj).Text == ""))
                    ((TextBox)obj).Text = "0,";
                ((TextBox)obj).Select(((TextBox)obj).Text.Length, 0); //курсор в конец текстбокса
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (obj != null)
            {
                if (e.Text != "," && e.Text != "." && IsNumber(e.Text) == false) //если введенный символ в текстбокс не "," и не число
                    e.Handled = true;
                else if (e.Text == "0" && ((TextBox)obj).Text == "0")  //если введенный символ - "0", в текстбоксе - "0", то запретить дальнейший ввод "0" 
                    e.Handled = true;
                else if (e.Text == "," || e.Text == ".") //если вводим "," или "."
                {
                    if (((TextBox)sender).Text.IndexOf(",") > -1)    //если "," уже есть, то запретить дальнейщий ввод "," и "."
                        e.Handled = true;
                    else if (e.Text == ".") //иначе если вводится точка, то сделать ее запятой
                    {
                        e.Handled = true;
                        ((TextBox)obj).Text += ",";
                    }
                }
                ((TextBox)obj).Select(((TextBox)obj).Text.Length, 0); //курсор в конец текстбокса
            }
        }

        private bool IsNumber(string text)  //метод для проверки является ли вводимый текст числом
        {
            int output;
            return int.TryParse(text, out output);
        }
    }
}
