using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleViewer.ViewModels.Catalogs;
using Grabacr07.KanColleWrapper.Models;
using System.Reactive.Subjects;
using System.Reactive;
using Livet;

namespace Calculator.Models
{
    class ExperienceCalculator : NotificationObject
    {
        private readonly Subject<Unit> updateSource = new Subject<Unit>();

        private int[] experienceTable;

        private IEnumerable<string> sortieList;
        private Dictionary<string, int> sortieExperienceTable;

        public string[] battleResults = { "S", "A", "B", "C", "D", "E" };
        private int sortieExperience;
        private int currentLevel;
        private int currentExperience;

        #region CurrentShip

        public Ship CurrentShip
        {
            get { return this.CurrentShip; }
            set
            {
                if(this.CurrentShip != value)
                {
                    this.CurrentShip = value;
                    this.targetLevel = Math.Min(this.CurrentShip.Level + 1, 155);
                    this.currentLevel = CurrentShip.Level;
                    this.currentExperience = CurrentShip.Exp;
                    this.UpdateExperience();
                }
            }
        }

        #endregion

        #region TargetLevel

        public int targetLevel
        {
            get { return this.targetLevel; }
            set
            {
                if(this.targetLevel != value)
                {
                    this.targetLevel = value;
                    this.target
                }
            }
        }

        #endregion

        public string selectedMap;
        public bool isFlagship;
        public bool isMVP;
        public int remainingBattles;
        public int remainingExperience;
        public string battleResult;

        public ExperienceCalculator()
        {
            this.experienceTable = ExperienceTable.experienceByLevel;
            this.sortieExperienceTable = SortieExperienceTable.SortieExperience;
            this.sortieList = sortieExperienceTable.Keys;

            this.CurrentShip = KanColleClient.Current.Homeport.Organization.Ships.Values.First();

            this.isFlagship = true;
            this.isMVP = true;

            battleResult = battleResults.FirstOrDefault();
            selectedMap = sortieExperienceTable.Keys.FirstOrDefault();
        }


        public void UpdateExperience()
        {
            //taken from YunYun's calculator plugin
            double multiplier = (this.isFlagship ? 1.5 : 1) * (this.isMVP ? 2 : 1) * (this.battleResult == "S" ? 1.2 : (this.battleResult == "C" ? 0.8 : (this.battleResult == "D" ? 0.7 : (this.battleResult == "E" ? 0.5 : 1))));

            this.sortieExperience = (int)Math.Round(sortieExperienceTable[selectedMap] * multiplier);
            this.remainingExperience = this.experienceTable[targetLevel] - this.currentExperience;
            this.remainingBattles = (int)Math.Ceiling(this.remainingExperience / (double)this.sortieExperience);
        }
    }
}
