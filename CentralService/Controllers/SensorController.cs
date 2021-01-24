using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DependencyInyection_Redis.Controllers
{
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public SensorController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public bool Post([FromQuery] int sensorId, [FromQuery] string sensorName, [FromQuery] int currentValue)
        {
            bool success = false;

            try
            {
                string dataToSave = string.Format("Name: {0} - Value: {1} - Updated at: {2}", sensorName, currentValue, DateTime.Now.ToShortTimeString());

                _distributedCache.SetString(sensorId.ToString(), dataToSave);
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at Sensor API POST ->" + ex.ToString());
            }

            return success;
        }

        [HttpGet]
        public string Get([FromQuery] int sensorId)
        {
            string dataToReturn = null;
            try
            {
                dataToReturn = _distributedCache.GetString(sensorId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at Sensor API GET ->" + ex.ToString());
            }

            return dataToReturn;
        }
    }
}