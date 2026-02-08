
using TandmSprinklerCount.Data;
using static TandmSprinklerCount.Models.FireDesignModels;

namespace TandmSprinklerCount.Services
{
    /// <summary>
    /// Service that calculates sprinkler positions and connects them to the nearest water pipes.
    /// </summary>
    public class SprinklerService : ISprinklerService
    {
        private readonly IFireDesignRepository _repo;

        public SprinklerService(IFireDesignRepository repo) => _repo = repo;

        /// <summary>
        /// Generates async sprinkler layouts for the room and calculates nearest pipe connections.
        /// </summary>
        public async Task<IEnumerable<SprinklerLayout>> GenerateLayoutAsync()
        {
            var room = _repo.GetRoomLayout();
            if (room == null)
                throw new InvalidOperationException("Room layout is not available.");

            var pipes = _repo.GetAvailablePipes()?.ToList();
            if (pipes == null || !pipes.Any())
                throw new InvalidOperationException("No water pipes are available.");

            const double spacing = 2500.0;
            var layouts = new List<SprinklerLayout>();

            // Room dimensions
            double length = Math.Sqrt(
                (room.C2.X - room.C1.X) * (room.C2.X - room.C1.X) +
                (room.C2.Y - room.C1.Y) * (room.C2.Y - room.C1.Y));

            double width = Math.Sqrt(
                (room.C3.X - room.C2.X) * (room.C3.X - room.C2.X) +
                (room.C3.Y - room.C2.Y) * (room.C3.Y - room.C2.Y));

            // Validate dimensions
            if (length <= 0 || width <= 0)
                throw new InvalidOperationException("Invalid room dimensions.");

            if (length < spacing * 2 || width < spacing * 2)
                return Enumerable.Empty<SprinklerLayout>(); // Not enough space for sprinklers

            // Unit vectors
            double ux = (room.C2.X - room.C1.X) / length;
            double uy = (room.C2.Y - room.C1.Y) / length;
            double vx = (room.C3.X - room.C2.X) / width;
            double vy = (room.C3.Y - room.C2.Y) / width;

            // Number of sprinklers
            int countLength = Math.Max(0, (int)(length / spacing) - 1);
            int countWidth = Math.Max(0, (int)(width / spacing) - 1);

            double sx;
            double sy;
            double sz;

            Point3D sprinklerPosition;
            int total = countLength * countWidth;

            for (int index = 0; index < total; index++)
            {
                int i = (index / countWidth) + 1;
                int j = (index % countWidth) + 1;

                // Sprinkler position
                sx = Math.Round(room.C1.X + (i * spacing * ux) + (j * spacing * vx), 2);
                sy = Math.Round(room.C1.Y + (i * spacing * uy) + (j * spacing * vy), 2);

                // Interpolated ceiling height
                sz = Math.Round(
                    room.C1.Z +
                    (j * spacing / width) *
                    (room.C3.Z - room.C2.Z),
                    2);

                sprinklerPosition = new Point3D(sx, sy, sz);
                layouts.Add(new SprinklerLayout(sprinklerPosition));
            }

            return layouts;
        }

    }
}
