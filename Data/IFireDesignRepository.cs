using System.IO.Pipelines;
using static TandmSprinklerCount.Models.FireDesignModels;
using Pipe = TandmSprinklerCount.Models.FireDesignModels.Pipe;

namespace TandmSprinklerCount.Data
{
    public interface IFireDesignRepository
    {
        Room GetRoomLayout();
        IEnumerable<Pipe> GetAvailablePipes();
    }
}
