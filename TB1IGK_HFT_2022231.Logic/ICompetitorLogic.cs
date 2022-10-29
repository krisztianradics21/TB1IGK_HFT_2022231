using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.Logic
{
    interface ICompetitorLogic : ILogic<Competitor>
    {
        double AVGAge();
        IEnumerable<object> CompetitorWithAllRelevantData();
    }
}
