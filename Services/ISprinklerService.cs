using static TandmSprinklerCount.Models.FireDesignModels;

namespace TandmSprinklerCount.Services
{
    public record SprinklerLayout(Point3D coordinates, Point3D ConnectionPoint);
    public interface ISprinklerService
    {
        Task<IEnumerable<SprinklerLayout>> GenerateLayoutAsync();
        //IEnumerable<SprinklerLayout> GenerateLayout();
    }
}
