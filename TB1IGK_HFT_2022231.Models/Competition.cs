using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB1IGK_HFT_2022231.Models
{
    public class Competition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey(nameof(Competitor))]
        public int CompetitorID { get; set; }
        [ForeignKey(nameof(Competitor))]
        public int OpponentID { get; set; }
        public int NumberOfRacesAgainstEachOther { get; set; }
        [StringLength(20)]
        public string Location { get; set; }
        [Range(200,1000)]
        public int Distance { get; set; }
        
        public Competition()
        {
        }

        public Competition(int iD, int competitorId, int opponentID, int numberofracescagainsteachother, string location, int distance)
        {
            this.ID = iD;
            this.CompetitorID = competitorId;
            this.OpponentID = opponentID;
            this.NumberOfRacesAgainstEachOther = numberofracescagainsteachother;
            this.Location = location;
            this.Distance = distance;
        }

        public override string ToString()
        {
            return $"ID: {ID}\tCompetitor ID: {CompetitorID}\tOpponent ID: {OpponentID}\tNumber Of Races Against Each Other: {NumberOfRacesAgainstEachOther}\tLocation: {Location}";
        }
    }
}
