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
using System.Text.RegularExpressions;

namespace Biorhythms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия кнопки "Рассчитать"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка введенных данных
            if (!int.TryParse(dayTextBox.Text, out int day) ||
                !int.TryParse(monthTextBox.Text, out int month) ||
                !int.TryParse(yearTextBox.Text, out int year))
            {
                resultTextBlock.Text = "Пожалуйста, введите корректные числовые значения.";
                return;
            }

            // Проверка корректности даты
            if (!IsValidDate(day, month, year))
            {
                resultTextBlock.Text = "Пожалуйста, введите корректную дату.";
                return;
            }

            // Создание объекта DateTime для даты рождения
            DateTime birthDate = new DateTime(year, month, day);
            DateTime today = DateTime.Today;

            // Проверка, что дата рождения не в будущем
            if (birthDate > today)
            {
                resultTextBlock.Text = "Дата рождения не может быть в будущем.";
                return;
            }

            // Расчет биоритмов
            var biorhythms = CalculateBiorhythms(birthDate, today);

            // Отображение результатов
            resultTextBlock.Text = $"Физический биоритм: {biorhythms.Physical:F2}\n" +
                                  $"Эмоциональный биоритм: {biorhythms.Emotional:F2}\n" +
                                  $"Интеллектуальный биоритм: {biorhythms.Intellectual:F2}";
        }

        // Метод для проверки корректности даты
        private bool IsValidDate(int day, int month, int year)
        {
            try
            {
                DateTime date = new DateTime(year, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Метод для расчета биоритмов
        private (double Physical, double Emotional, double Intellectual) CalculateBiorhythms(DateTime birthDate, DateTime currentDate)
        {
            // Вычисление количества дней между датами
            int days = (currentDate - birthDate).Days;

            // Периоды биоритмов в днях
            const int physicalCycle = 23;
            const int emotionalCycle = 28;
            const int intellectualCycle = 33;

            // Расчет значений биоритмов (от -1 до 1)
            double physical = Math.Sin(2 * Math.PI * days / physicalCycle);
            double emotional = Math.Sin(2 * Math.PI * days / emotionalCycle);
            double intellectual = Math.Sin(2 * Math.PI * days / intellectualCycle);

            return (physical, emotional, intellectual);
        }

        // Валидация ввода только чисел
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}