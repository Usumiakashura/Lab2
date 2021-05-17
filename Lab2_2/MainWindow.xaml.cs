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

namespace Lab2_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object obj = null;

        public MainWindow()
        {
            InitializeComponent();
            textBoxA.GotFocus += textBox_GotFocus;
            textBoxB.GotFocus += textBox_GotFocus;
            textBoxH.GotFocus += textBox_GotFocus;
        }
        void textBox_GotFocus(object sender, EventArgs e)   //для определения выделенного текстбокса
        {
            obj = sender;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            if (textBoxA.Text == "") textBoxA.Text = "0";   //заменит пустое поле на 0 для подсчетов
            if (textBoxB.Text == "") textBoxB.Text = "0";   //заменит пустое поле на 0 для подсчетов
            if (textBoxH.Text == "") textBoxH.Text = "0";   //заменит пустое поле на 0 для подсчетов
            double a = Convert.ToDouble(textBoxA.Text);
            double b = Convert.ToDouble(textBoxB.Text);
            double h = Convert.ToDouble(textBoxH.Text);
            double y = 0;
            for (double x = a; x <= b; x += h)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        y = Math.Pow(x, 2) - 10 * Math.Pow(Math.Sin(x), 2) + 2;
                        break;
                    case 1:
                        y = Math.Pow(x, 3) + 10 * Math.Pow(x, 2) - 50;
                        break;
                    case 2:
                        y = Math.Pow(x, 2) + 5 * Math.Cos(x) - 3;
                        break;
                    case 3:
                        y = Math.Pow(Math.Sin(x), 2) - 3 * Math.Cos(x);
                        break;
                }
                listBox.Items.Add($"x = {x:0.00}    y = {y:0.00}");
            }
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
