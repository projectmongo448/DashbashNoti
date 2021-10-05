﻿using System.Collections.Generic;
using System.Linq;
using DotNetify.Elements;
using Newtonsoft.Json;


namespace Website.Server
{
   public interface IMovieService
   {
      IEnumerable<MovieRecord> GetAFITop100();

      MovieRecord GetMovieByAFIRank(int rank);
   }

   public class MovieRecord
   {
      public int Rank { get; set; }
      public string Movie { get; set; }
      public int Year { get; set; }
      public string Cast { get; set; }
      public string Director { get; set; }
   }

   public class MovieService : IMovieService
   {
      public IEnumerable<MovieRecord> GetAFITop100() => JsonConvert.DeserializeObject<List<MovieRecord>>(
         Utils.GetResource("Website.Server.Services.AFITop100.json", GetType().Assembly).Result);

      public MovieRecord GetMovieByAFIRank(int rank) => GetAFITop100().FirstOrDefault(i => i.Rank == rank);
   }
}