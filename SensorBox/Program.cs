using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SensorBox
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting sensors!");

            //We only se the client one time
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/Sensor");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Sensor sensor1 = new Sensor(1, "SensorA", 500, client);
            Sensor sensor2 = new Sensor(2, "SensorB", 500, client);
            Sensor sensor3 = new Sensor(3, "SensorC", 500, client);

            sensor1.Connect();
            sensor2.Connect();
            sensor3.Connect();

            Console.ReadLine();
        }
    }
}
