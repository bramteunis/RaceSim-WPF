﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {
        public IParticipant? Left { get; set; }
        public IParticipant? Right { get; set; }
        public int DistanceLeft { get; set; }
        public int DistanceRight { get; set; }



    }
}
