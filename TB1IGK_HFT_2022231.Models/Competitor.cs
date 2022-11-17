using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB1IGK_HFT_2022231.Models
{
    public class Competitor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        public int Age { get; set; }
        [ForeignKey(nameof(Competition))]
        public int CompetitonID { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }
        [StringLength(3)]
        public string Nation { get; set; }

        public Competitor()
        {
        }

        public Competitor(int id, string name, int age, int competitionID, int categoryID, string nation)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.CompetitonID = competitionID;
            this.CategoryID = categoryID;
            this.Nation = nation;
        }

        public override string ToString()
        {
            return $"ID: {Id}\tName: {Name}\tAge: {Age}\tCompetition ID: {CompetitonID}\tCategory ID: {CategoryID}\tNation: {Nation}";
        }
    }
}
