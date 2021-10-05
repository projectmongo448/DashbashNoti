using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace Website.Server
{
   public interface ILiveDataService
   {
      IObservable<string> Download { get; }
      IObservable<string> Upload { get; }
      IObservable<string> Latency { get; }

      IObservable<string> Users { get; }
      IObservable<int[]> Traffic { get; }
      IObservable<int[]> ServerUsage { get; }
      IObservable<int[]> Utilization { get; }
      IObservable<Activity> RecentActivity { get; }
   }

   public class Activity
   {
      //private static readonly Dictionary<int, string> _activities = new Dictionary<int, string> {
      //      {1, "Offline"},
      //      {2, "Active"},
      //      {3, "Busy"},
      //      {4, "Away"},
      //      {5, "In a Call"}
      //  };

      public int Id { get; set; }
      public string PersonName { get; set; }
      public int StatusId { get; set; }
      //public string Status => _activities[StatusId];
   }

   public class MockLiveDataService : ILiveDataService
   {
        
        private readonly Random _random = new Random();
        private readonly IMongoCollection<Product> _download;
        private readonly IMongoCollection<List> _list;
        private readonly IMongoCollection<Mech> _Mech;

        public IObservable<string> Download { get; }

      public IObservable<string> Upload { get; }

      public IObservable<string> Latency { get; }
        public IObservable<string> Users { get; }


        public IObservable<int[]> Traffic { get; }

      public IObservable<int[]> ServerUsage { get; }

      public IObservable<int[]> Utilization { get; }

      public IObservable<Activity> RecentActivity { get; }





        public MockLiveDataService(ICustomerRepository customerRepository)
      {
            var clientMS = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var databaseMS = clientMS.GetDatabase("MyShop");
            _download = databaseMS.GetCollection<Product>("Order");

            //var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            //var database = clientMS.GetDatabase("Manufacturing");
            //_list = database.GetCollection<List>("warehouse");

            //var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            //var database = client.GetDatabase("ALLtrainMescla5D");
            //_Mech = database.GetCollection<Mech>("machine");
            var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var database = client.GetDatabase("MyShop");
            _Mech = database.GetCollection<Mech>("Order2");


            Download = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => $"{volt()}");

         Upload = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => $"{rotate()}");

         Latency = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => $"{pressure()}");

            Users = Observable
           .Interval(TimeSpan.FromSeconds(2))
           .StartWith(0)
           .Select(_ => $"{vibration()}");


            Traffic = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => new int[] { GetData()});

            ServerUsage = Observable
               .Interval(TimeSpan.FromSeconds(2))
               .StartWith(0)
               .Select(_ => new int[] { volt(), rotate(), pressure(), vibration() });

            Utilization = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => new int[] { volt(), rotate(), pressure(), vibration() });




            RecentActivity = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => GetRandomCustomer(customerRepository))
            .Select(customer => new Activity
            {
               PersonName = ""
            })
            .StartWith(
               Enumerable.Range(1, 4)
               .Select(_ => GetRandomCustomer(customerRepository))
               .Select(customer => new Activity
               {

                  PersonName = "....."

               })
               .ToArray()
            );
      }

      private Customer GetRandomCustomer(ICustomerRepository customerRepository)
      {
         Customer record;
         while ((record = customerRepository.Get(_random.Next(1, 20))) == null);
         return record;
      }

        private int GetData()
        {
            //var codeProduct = _random.Next(1, 9);
            //var definevolt = _random.Next(170, 180);
            //var definerotate = _random.Next(450, 460);
            //var definepressure = _random.Next(100, 110);
            //var definevibration = _random.Next(40, 50);

            //_Mech.InsertOne(new Mech { machineId = $"Machine-{codeProduct}", voltmean = definevolt, rotatemean = definerotate, pressuremean = definepressure, vibrationmean = definevibration });

            //var Cost = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Cost).Sum());
            //var Income = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Income).Sum());
            int showvolt;
            try
            {
                showvolt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.voltmean).Last());
            }
            catch (Exception ex) {
                return 0;
            }



            if (showvolt > 176)
            {
                string message = "แจ้งเตือน! แรงความดันไฟฟ้าตอนนี้มากกว่า 176 ("+showvolt+")";
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + "VuD8OOZwFAgHlTswbKm6eo9ZsBN6IFRE2dhYgZkQrsU");
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return showvolt;
            }
            else {
                return showvolt;
            }


        }
        private int volt()
        {
            int volt;
            try {
                volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.voltmean).Last());
            } catch (Exception ex) {
                return 0;
            }


            return volt;
        }
        private int rotate()
        {
            int rotate;
            try
            {
                rotate = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.rotatemean).Last());
            }
            catch (Exception ex) {
                return 0;
            }

            return rotate;
        }
        private int pressure()
        {
            int pressure;
            try
            {
                pressure = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.pressuremean).Last());
            }
            catch (Exception ex) {
                return 0;
            }

            return pressure;
        }
        private int vibration()
        {
            int vibration;
            try
            {
                vibration = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.vibrationmean).Last());
            }
            catch (Exception ex) {
                return 0;
            }
            return vibration;
        }

    }
}