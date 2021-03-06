﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Build
{
    public interface IBuildProvider
    {
        Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync();

        Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync(int sinceHours);
    }
}
