﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Person
    {
        public int Key { get; set; }
        public string Name { get; set; }    
        public DateTime BirthDate { get; set; }

        public int? SomeValue { get; set; }
    }
}
