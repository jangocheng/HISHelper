﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductReleaseSystem.Models.ProductRelease
{
    public partial class ResearchProjects
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectIntroduction { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ProjectProgress { get; set; }
    }
}
