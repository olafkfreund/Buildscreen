﻿using System.Collections.Generic;

namespace ParkSquare.BuildScreen.Web.Services.AzureDevOps.Dto
{
    public class RefsResponseDto
    {
        public List<RefDto> Value { get; set; }

        public int Count { get; set; }
    }
}