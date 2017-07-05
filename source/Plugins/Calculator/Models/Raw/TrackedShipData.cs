using Calculator.ViewModels;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using System;

namespace Calculator.Models.Raw
{
    class SavedShip
    {
        public int ship_id;
        public int target_level;
        public string selected_map;
        public string selected_rank;
        public bool is_flagship;
        public bool is_mvp;
    }

    class TrackedShip
    {
        public SavedShip export_data;
        public int remaining_Exp;
        public int remaining_Battles;
        private ToolViewModel plugin;
        private Ship CurrentShip;

        public TrackedShip(int ship_id, int target_level, string selected_map, string selected_rank, bool is_flagship, bool is_mvp, ToolViewModel plugin)
        {
            this.export_data.ship_id = ship_id;
            this.export_data.target_level = target_level;
            this.export_data.selected_map = selected_map;
            this.export_data.selected_rank = selected_rank;
            this.export_data.is_flagship = is_flagship;
            this.export_data.is_mvp = is_mvp;
            this.plugin = plugin;   
        }

        public void Delete()
        {
            this.plugin.TrackedShips.Remove(this);
        }

        public void Update()
        {
            if(this.plugin.homeport.Organization.Ships != null)
            {
                this.CurrentShip = this.plugin.homeport.Organization.Ships[export_data.ship_id];

                double multiplier = (this.export_data.is_flagship ? 1.5 : 1) * (this.export_data.is_mvp ? 2 : 1) * (this.export_data.selected_rank == "S" ? 1.2 : (this.export_data.selected_rank == "C" ? 0.8 : (this.export_data.selected_rank == "D" ? 0.7 : (this.export_data.selected_rank == "E" ? 0.5 : 1))));

                var sortieExperience = (int)Math.Round(plugin.sortieExperienceTable[this.export_data.selected_map] * multiplier);
                this.remaining_Exp = this.plugin.experienceTable[this.export_data.target_level] - this.CurrentShip.Exp;
                this.remaining_Battles = (int)Math.Ceiling(this.remaining_Exp / (double)sortieExperience);
            }
            
        }
    }
}
