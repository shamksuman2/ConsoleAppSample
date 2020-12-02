using Microsoft.Extensions.Configuration;
using NuGet.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisCacheService obj = new RedisCacheService();
            //obj.Save(817e68ea-95a8-9b4b-6c3c-add369278853, {\IndexName\:\site\,\Documents\:[{\active\:true,\address\:\AAAA\,\backOfficeId\:\F0001\,\city\:\Bangalore\,\countryCode\:\IN\,\countryName\:\India\,\description\:null,\devices\:[],\explicitFlags\:[\True\,\True\,\False\,\True\,\True\,\True\,\True\],\ftpAddress\:null,\ftpUserId\:null,\id\:\c9a78302-0676-46e1-9df5-087386e350d5\,\isMediaSupported\:false,\deviceTypes\:[],\lastUpdated\:\2020-03-31T18:00:09.7148346+00:00\,\location\:{\Latitude\:13.0078444,\Longitude\:77.557662999999991,\IsEmpty\:false,\Z\:null,\M\:null,\CoordinateSystem\:{\EpsgId\:4326,\Id\:\4326\,\Name\:\WGS84\}},\name\:\Fleet Site Local Draft 42\,\organization\:\71c8abc5-7784-4fff-8eb3-fc74a99a31ab\,\parentGroups\:[\00000000-0000-0000-0000-000000000000\,\521065b0-bf94-454b-87d7-fdef44893e75\,\521065b0-bf94-454b-87d7-fdef44893e75\,\78a926f2-eedd-4b9e-9233-d2ef43574abc\,\521065b0-bf94-454b-87d7-fdef44893e75\,\521065b0-bf94-454b-87d7-fdef44893e75\,\521065b0-bf94-454b-87d7-fdef44893e75\],\shortName\:\Fleet\,\siteActivityStatus\:\Unknown\,\stateCode\:\KA\,\stateName\:\Karnataka\,\status\:\Created\,\timeZone\:\India Standard Time\,\userGroups\:[\521065b0-bf94-454b-87d7-fdef44893e75\,\71c8abc5-7784-4fff-8eb3-fc74a99a31ab\,\78a926f2-eedd-4b9e-9233-d2ef43574abc\,\87c52109-9159-4f5c-9fb4-0ec72d9ddc56\,\a18943db-7236-4338-9e2f-76d05bd13d60\,\bf15270c-e1ae-4194-8509-16d9c0ffca4c\,\e1ae6bfd-a979-43d4-800c-28f81c23e339\],\zipCode\:\560055\,\createdTime\:\2020-03-31T17:08:37.8978131+00:00\,\activeFor\:[\isense\],\products\:[\isense\],\timestamp\:\2020-03-31T18:00:10Z\}]});
            //obj.Clear();
            obj.GetAllRedisKeys();


        }

    }

    public class RedisCacheService  
    {
        private IDatabase _cache;
        private ConnectionMultiplexer _connectionMultiplexer;
        //string connection  = redis-fleet-qa2.redis.cache.windows.net:6380,password=7rDD9OPSKhzNMWvn8edtEp7jRUzgdrvMROq1VTaeTRQ=,ssl=True,abortConnect=False;
        string connection = "redis-fleet-qa.redis.cache.windows.net:6380,password=0AnJ+z3avNVaOoTSYhMkhz0VTATqCaMqRcSv7xUKHS0=,ssl=True,abortConnect=False";
        //string connection = redis-fleet-dev.redis.cache.windows.net:6380,password=BOOUW5HhLu49w9kM8zJ60FxTtA3OiQ1w6CGcSjYMkd4=,ssl=True,abortConnect=False;
        public RedisCacheService()
        {
            //var connection = ConfigurationManager.AppSettings[RedisConnection];
            ConfigurationOptions options = ConfigurationOptions.Parse(connection);
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);

        }
        public void GetAllRedisKeys()
        {
            //
            //ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
            //IDatabase db = connection.GetDatabase();
            //EndPoint endPoint = connection.GetEndPoints().First();


            while (true)
            {
                _cache = _connectionMultiplexer.GetDatabase();

                EndPoint endPoint = _connectionMultiplexer.GetEndPoints().First();
                RedisKey[] keys = _connectionMultiplexer.GetServer(endPoint).Keys(pattern: "*").ToArray();

                foreach (var item in keys)
                {
                    
                    Console.WriteLine(item);
                }
                Console.WriteLine("/n------------------------------------------------------------/n");
                Thread.Sleep(6000);
            }

        }

        public bool Exists(string key)
        {
            return _cache.KeyExists(key);
        }

        public void Save(string key, string value)
        {
            var ts = TimeSpan.FromMinutes(10);
            _cache.StringSet(key, value, ts);
        }

        public string Get(string key)
        {
            return _cache.StringGet(key);
        }

        public void Remove(string key)
        {
            _cache.KeyDelete(key);
        }

        public void Clear()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
    }
}
