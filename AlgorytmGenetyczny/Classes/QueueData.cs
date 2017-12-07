using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class QueueData
    {
        public string Name { set; get; }
        public List<Result> Results { set; get; }
        public int PopulationCount { set; get; }
        public int GenerationCount { set; get; }
        public ISelectionMethod SelectionMethod { set; get; }
        public double CrossOver { set; get; }
        public double Mutation { set; get; }
        public bool Migration { set; get; }
        public int IslandsCount { set; get; }
        public int MigrationCount { set; get; }
        public int MigrationTime { set; get; }


        public QueueData(string Name, List<Result> Results, int PopulationCount, int GenerationCount, ISelectionMethod SelectionMethod,
                                     double CrossOver, double Mutation, bool Migration,
                                     int IslandsCount, int MigrationCount, int MigrationTime)
        {
            this.Name = Name;
            this.Results = Results;
            this.PopulationCount = PopulationCount;
            this.GenerationCount = GenerationCount;
            this.SelectionMethod = SelectionMethod;
            this.CrossOver = CrossOver;
            this.Mutation = Mutation;
            this.Migration = Migration;
            this.IslandsCount = IslandsCount;
            this.MigrationCount = MigrationCount;
            this.MigrationTime = MigrationTime;
        }
    }
}
