using Final_E_Commerce.ViewModels;

namespace Final_E_Commerce.İnterfaces
{
    public interface IHomeRepository
    {
        HomeVM Index(string? username);
    }
}
