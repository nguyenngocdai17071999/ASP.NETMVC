﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Framwork;

namespace HocWeb.Models
{
    [Serializable]
    public class CartItem
    {
        
        public Product product { get; set; }
        public int Quantity { get; set; }
       

    }
}