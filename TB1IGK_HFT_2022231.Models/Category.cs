using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB1IGK_HFT_2022231.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(10)]
        public string AgeGroup { get; set; }
        [StringLength(20)]
        public string BoatCategory { get; set; }

        public Category()
        {
        }
        public Category(int id, string ageGroup, string boatCategory)
        {
            this.ID = id;
            this.AgeGroup = ageGroup;
            this.BoatCategory = boatCategory;
        }

        public override string ToString()
        {
            return $"ID: {ID}\t Age Group: {AgeGroup}\tBoat Category: {BoatCategory}";
        }
    }
}
