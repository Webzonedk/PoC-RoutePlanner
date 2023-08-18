﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class TimeFrame
    {
        public int Id { get; set; }
        public DateTime TimeFrameStart { get; set; }
        public DateTime TimeFrameEnd { get; set; }
    }
}