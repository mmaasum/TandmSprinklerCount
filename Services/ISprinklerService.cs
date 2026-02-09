using static TandmSprinklerCount.Models.FireDesignModels;

namespace TandmSprinklerCount.Services
{
    public record SprinklerLayout(Point3D ConnectionPoint);
    public interface ISprinklerService
    {
        Task<IEnumerable<SprinklerLayout>> GenerateLayoutAsync();
    }
}
