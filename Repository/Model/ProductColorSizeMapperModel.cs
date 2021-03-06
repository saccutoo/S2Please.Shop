﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ProductColorSizeMapperModel : BaseModel
    {
        public long COLOR_ID { get; set; }
        public long SIZE_ID { get; set; }
        public long PRODUCT_ID { get; set; }
        public long AMOUNT { get; set; }
        public float PRICE { get; set; }
        public float PRICE_IMPORT { get; set; }
        public long RATE { get; set; }
        public bool IS_MAIN { get; set; }
        public float DISCOUNT_RATE { get; set; }
        public string IMG { get; set; }
        public string SIZE_NAME { get; set; }
        public string COLOR { get; set; }
        public int QUANTITY_SOLD { get; set; }
        public string SIZE { get; set; }
        public long INDEX { get; set; }
        public string FILE_NAME { get; set; }

    }
}
