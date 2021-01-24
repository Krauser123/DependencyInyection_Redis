using System;
using System.Net.Http;
using System.Timers;

namespace SensorBox
{
    public class Sensor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CurrentValue { get; set; }
        public int MsToSendInfo { get; set; }
        private readonly HttpClient Client;

        private Timer tmr;

        public Sensor(int id, string name, int msToSendInfo, HttpClient client)
        {
            this.ID = id;
            this.Name = name;
            this.MsToSendInfo = msToSendInfo;
            this.Client = client;
            SetTimer();
        }

        public void Connect()
        {
            tmr.Start();
        }

        /// <summary>
        /// Setup timer
        /// </summary>
        private void SetTimer()
        {
            tmr = new Timer(this.MsToSendInfo);

            tmr.Elapsed += OnTimedEvent;
            tmr.AutoReset = true;
            tmr.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Get value from sensor
            RefreshValue();

            //Send to central service
            string queryString = string.Format("?sensorId={0}&sensorName={1}&currentValue={2}", this.ID, this.Name, this.CurrentValue);
            HttpResponseMessage response = Client.PostAsync(Client.BaseAddress + queryString, null).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("{0} - Data send to {1}", DateTime.Now.ToString(), Client.BaseAddress + queryString));
            }
            else
            {
                Console.WriteLine(string.Format("{0} - Error at sensor: {1} when try to send data to central service.", DateTime.Now.ToString(), this.ID));
            }

        }

        private void RefreshValue()
        {
            this.CurrentValue = new Random().Next(0, 9999);
        }
    }
}
