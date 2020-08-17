using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Net.Http;
using System.Threading;

namespace IroiroMonitor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "いろいろ計測モニター";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private double _temperature;
        private double _humidity;
        private string _cpuTemp;

        public double Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }
        public double Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }
        public string CpuTemp
        {
            get { return _cpuTemp; }
            set { SetProperty(ref _cpuTemp, value); }
        }

        public MainWindowViewModel()
        {
            Thread thread = new Thread(new ThreadStart(async () => {
                try
                {
                    HttpClient client = new HttpClient();
                    while (true)
                    {
                        HttpResponseMessage response = await client.GetAsync("http://192.168.1.15:8000/");
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Sensor sensor = JsonConvert.DeserializeObject<Sensor>(responseBody);
                        double temp = 0;
                        double hum = 0;
                        double.TryParse(sensor.temperature,out temp);
                        double.TryParse(sensor.humidity, out hum);
                        Temperature = temp;
                        Humidity = hum;
                        CpuTemp = sensor.cputemp;
                        Thread.Sleep(1000 * 60);
                    }
                }
                catch (Exception ex)
                {
                }
            }));

            thread.Start();
        }
    }
}
