using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application.Gleam
{
    [Serializable]
    class GleamGiveaway
    {
        public string url;
        public DateTime created = DateTime.Now;
        public bool hasDailyEntry;
        public DateTime lastEntry;
    }


}
