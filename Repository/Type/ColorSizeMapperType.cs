using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Type
{
    public class ColorSizeMapperType
    {
        public long ID { get; set; }
        public string COLOR { get; set; }
        public string SIZE { get; set; }
        public long AMOUNT { get; set; }
        public float PRICE { get; set; }
        public long RATE { get; set; }
        public string IMG { get; set; }
        public bool IS_MAIN { get; set; }
        public long INDEX { get; set; }

    }
}
