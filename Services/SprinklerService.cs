using System.IO.Pipelines;
using System.Threading.Tasks;
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
            Point3D connectionPoint;
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
                connectionPoint = FindNearestPipePoint(sprinklerPosition, pipes);

                layouts.Add(new SprinklerLayout(sprinklerPosition, connectionPoint));
            }

            return layouts;
        }

        /// <summary>
        /// Finds the closest point on the nearest water pipe to a sprinkler.
        /// </summary>
        private Point3D FindNearestPipePoint(Point3D sprinkler, IEnumerable<Models.FireDesignModels.Pipe> pipes)
        {
            if (sprinkler == null)
                throw new ArgumentNullException(nameof(sprinkler));

            if (pipes == null)
                throw new ArgumentNullException(nameof(pipes));

            double minDistance = double.MaxValue;
            Point3D? nearestPoint = null;


            double dx;
            double dy;
            double t;
            double px;
            double py;
            double lengthSquared;
            double distance;

            foreach (var pipe in pipes)
            {
                if (pipe?.Start == null || pipe.End == null)
                    continue;

                dx = pipe.End.X - pipe.Start.X;
                dy = pipe.End.Y - pipe.Start.Y;
                lengthSquared = dx * dx + dy * dy;

                // Skip zero-length pipes
                if (lengthSquared <= 0)
                    continue;

                // Projection factor
                t = Math.Clamp(
                    ((sprinkler.X - pipe.Start.X) * dx +
                     (sprinkler.Y - pipe.Start.Y) * dy) / lengthSquared,
                    0.0, 1.0);

                // Closest point on pipe segment
                px = pipe.Start.X + t * dx;
                py = pipe.Start.Y + t * dy;

                distance = Math.Sqrt(
                    (sprinkler.X - px) * (sprinkler.X - px) +
                    (sprinkler.Y - py) * (sprinkler.Y - py));

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = new Point3D(px, py, pipe.Start.Z);
                }
            }

            if (nearestPoint == null)
                throw new InvalidOperationException("No valid pipe found to connect.");

            return nearestPoint;
        }

    }
}
