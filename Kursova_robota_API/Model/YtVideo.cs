using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursova_robota_API.Model
{
    public class Items
    {
        public List<Item> items { get; set; }
    }
    public class Item
    {
        public Id id { get; set; }
    }
    public class Id
    {
        public string videoId { get; set; }
    }
}

