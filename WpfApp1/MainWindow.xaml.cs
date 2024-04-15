using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertCurrency_Click(object sender, RoutedEventArgs e)
        {
            string amount = txtAmount.Text; //сумма
            string sourceCurrency = (cmbSourceCurrency.SelectedItem as ComboBoxItem)?.Content.ToString().Split()[0]; //исходная валюта
            string targetCurrency = (cmbTargetCurrency.SelectedItem as ComboBoxItem)?.Content.ToString().Split()[0]; //целевая валюта

            if (string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(sourceCurrency) || string.IsNullOrEmpty(targetCurrency))
            {
                MessageBox.Show("Пожалуйста, введите действительную сумму и выберите исходную и целевую валюты.");
                return;
            }

            try
            {
                WebClient client = new WebClient();
                string responseBody = client.DownloadString($"https://api.exchangerate-api.com/v4/latest/{sourceCurrency}");
                JObject jsonResponse = JObject.Parse(responseBody);

                double rate = Convert.ToDouble(jsonResponse["rates"][targetCurrency].ToString()); 
                double convertedAmount = Convert.ToDouble(amount) * rate;

                txtResult.Text = $"Конвертированная сумма: {convertedAmount} {targetCurrency}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}