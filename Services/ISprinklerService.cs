using static TandmSprinklerCount.Models.FireDesignModels;

namespace TandmSprinklerCount.Services
{
    public record SprinklerLayout(Point3D coordinates);
    public interface ISprinklerService
    {
        Task<IEnumerable<SprinklerLayout>> GenerateLayoutAsync();
    }
}
