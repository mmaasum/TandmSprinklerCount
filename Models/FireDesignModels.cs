namespace TandmSprinklerCount.Models
{
    public class FireDesignModels
    {
        public record Point3D(double X, double Y, double Z);
        public record Pipe(Point3D Start, Point3D End);
        public record Room(Point3D C1, Point3D C2, Point3D C3, Point3D C4);
    }
}
