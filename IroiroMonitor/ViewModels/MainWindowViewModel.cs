using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Net.Http;
using System.Text;
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

        private const int CommandSwitch = 1;

        private double _temperature;
        private double _humidity;
        private string _cpuTemp;
        private string _gpuTemp;
        private string _dateTime;

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
        public string GpuTemp
        {
            get { return _gpuTemp; }
            set { SetProperty(ref _gpuTemp, value); }
        }
        public string DateTime
        {
            get { return _dateTime; }
            set { SetProperty(ref _dateTime, value); }
        }

        public DelegateCommand ButtonClickCommand { get; }

        public MainWindowViewModel()
        {
            ButtonClickCommand = new DelegateCommand(async () =>
            {
                Command cmd = new Command();
                cmd.number = CommandSwitch;
                var json = JsonConvert.SerializeObject(cmd);
                try
                {
                    HttpClient client = new HttpClient();
                    var content = new StringContent(json, Encoding.UTF8);
                    await client.PostAsync("http://192.168.1.15:8000/", content);
                }
                catch (Exception ex)
                {
                }
            });

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
                        GpuTemp = sensor.gputemp;
                        DateTime = sensor.datetime;
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
