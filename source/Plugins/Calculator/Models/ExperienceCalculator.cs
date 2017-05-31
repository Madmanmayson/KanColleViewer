using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;

namespace Calculator.Models
{
    class ExperienceCalculator
    {
        private int[] experienceTable;
        private Dictionary<string, int> sortieExperienceTable;
        public string[] battleResults = { "S", "A", "B", "C", "D", "E" };

        private string selectedMap;
        private int remainingExperience;
        private bool isFlagship;
        private bool isMVP;
        private int remainingBattles;

        public ExperienceCalculator()
        {
            experienceTable = ExperienceTable.experienceByLevel;
            sortieExperienceTable = SortieExperienceTable.SortieExperience;

            isFlagship = false;
            isMVP = false;

            selectedMap = sortieExperienceTable.Keys.FirstOrDefault();
        }
    }
}
