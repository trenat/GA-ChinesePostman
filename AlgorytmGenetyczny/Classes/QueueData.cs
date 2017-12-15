using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class QueueData : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
            get => _name;
        }
        public List<Result> Results { set; get; }
        public int PopulationCount { set; get; }
        public int GenerationCount { set; get; }
        public ISelectionMethod SelectionMethod { set; get; }
        public bool CrossoverMix { set; get; }        
        public double CrossOver { set; get; }
        public double Mutation { set; get; }
        public bool Migration { set; get; }
        public int IslandsCount { set; get; }
        public int MigrationCount { set; get; }
        public int MigrationTime { set; get; }
        public double RandomSelectionPortion { get; internal set; }
        public bool AutoShufling { get; internal set; }
        public bool KillBothParents { get; internal set; }

        public QueueData(string Name, List<Result> Results, int PopulationCount, int GenerationCount, ISelectionMethod SelectionMethod,
                                     bool CrossoverMix, double CrossOver, bool KillBothParents, double Mutation, bool AutoShufling,
                                     double RandomSelectionPortion, bool Migration, int IslandsCount, int MigrationCount, int MigrationTime)
        {
            this.Name = Name;
            this.Results = Results;
            this.PopulationCount = PopulationCount;
            this.GenerationCount = GenerationCount;
            this.SelectionMethod = SelectionMethod;
            this.CrossoverMix = CrossoverMix;
            this.CrossOver = CrossOver;
            this.Mutation = Mutation;
            this.Migration = Migration;
            this.IslandsCount = IslandsCount;
            this.MigrationCount = MigrationCount;
            this.MigrationTime = MigrationTime;
            this.RandomSelectionPortion = RandomSelectionPortion;
            this.AutoShufling = AutoShufling;
            this.KillBothParents = KillBothParents;

        }

    #region notify
    protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
