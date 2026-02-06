using TandmSprinklerCount.Services;

namespace TandmSprinklerCount.Models.Responses
{
    public class FireDesignResponse
    {
        public int NumberOfSprinklers { get; set; }
        public List<SprinklerLayout> Sprinklers { get; set; } = new();
    }
}
