using Final_E_Commerce.ViewModels;

namespace Final_E_Commerce.İnterfaces
{
    public interface IHomeRepository
    {
        HomeVM Index(string? username);

        DetailVM Detail(int? id, string? username);
        bool ExistProducts(int? id);
        bool UserBanned(string? username);
        void CorrectDiscountTime();
        ListProductsVM Brands(int? id, string? username);

        object Rate(int Rating, int ProductId, string? username);
        string RemoveRating(int id, string ReturnUrl, string? username);
        object DeleteComment(int id, string? username);
        CommentsVM LoadComments(int skip, int? BlogId, string? username);
        CommentsVM PostComment(int ProductId, string comment, string? author, string? username);


    }
}
