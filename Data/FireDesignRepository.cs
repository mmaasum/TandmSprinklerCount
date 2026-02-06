using System.IO.Pipelines;
using static TandmSprinklerCount.Models.FireDesignModels;
using Pipe = TandmSprinklerCount.Models.FireDesignModels.Pipe;

namespace TandmSprinklerCount.Data
{
    /// <summary>
    /// Provides in-memory room layout and pipe data for sprinkler calculations.
    /// Async is not needed since no I/O is performed.
    /// </summary>
    public class FireDesignRepository : IFireDesignRepository
    {
        public Room GetRoomLayout() => new Room(
                new Point3D(97500.01, 34000.00, 2500.00),
                new Point3D(85647.67, 43193.61, 2500.00),
                new Point3D(91776.75, 51095.16, 2530.00),
                new Point3D(103629.07, 41901.55, 2530.00)
            );

        public IEnumerable<Pipe> GetAvailablePipes() => new List<Pipe> {
        new Pipe(new Point3D(98242.11, 36588.29, 3000.00), new Point3D(87970.10, 44556.09, 3000.00)),
        new Pipe(new Point3D(99774.38, 38563.68, 3000.00), new Point3D(89502.37, 46531.47, 3000.00)),
        new Pipe(new Point3D(101306.65, 40539.07, 3000.00), new Point3D(91034.63, 48507.01, 3000.00))
        };
    }
}
