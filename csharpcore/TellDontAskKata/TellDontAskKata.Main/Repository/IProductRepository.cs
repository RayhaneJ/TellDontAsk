using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Repository
{
    public interface IProductRepository
    {
        Product GetByName(string name);
    }
}
