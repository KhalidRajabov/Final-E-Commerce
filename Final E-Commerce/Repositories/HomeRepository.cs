using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.İnterfaces;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser>? _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;
        private IConfiguration _config { get; }

        public HomeRepository(AppDbContext context, UserManager<AppUser>? usermanager, SignInManager<AppUser>? signInManager, IConfiguration config)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
            _config = config;
        }
        public HomeVM Index(string? userName)
        {
            CorrectDiscountTime();
            HomeVM? homeVM = new HomeVM();
            List<Products> Bestsellers = _context.Products
                .OrderByDescending(p => p.Sold).Take(8)
                .Where(p => p.Status == ProductConfirmationStatus.Approved && !p.IsDeleted).Include(p => p.ProductImages).ToList();
            if (userName!=null&&!UserBanned(userName))
            {
                AppUser user = _usermanager.FindByNameAsync(userName).GetAwaiter().GetResult();
                homeVM.User = user;
                homeVM.Wishlists = _context?.Wishlists?.Where(w => w.AppUserId == user.Id).ToList();
                List<Products> Following = new List<Products>();
                List<UserSubscription>? followings = _context?.Subscription?.Where(s => s.SubscriberId == user.Id).ToList();
                foreach (var item in followings)
                {
                    AppUser profile = _usermanager.FindByIdAsync(item.ProfileId).GetAwaiter().GetResult();
                    List<Products>? ProfileProducts = _context.Products.Where(p => p.AppUserId == profile.Id)
                        .Include(p => p.ProductImages).ToList();
                    foreach (var products in ProfileProducts)
                    {
                        Following.Add(products);
                    }
                }
                homeVM.Following = Following;
            }
            homeVM.Bio = _context?.Bios?.FirstOrDefault();
            homeVM.Category = _context.Categories?.FirstOrDefault(c => c.Id == 1);
            homeVM.MostPopularProduct = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved && !p.IsDeleted)
                .OrderByDescending(p => p.Rating).Take(1).Include(p => p.ProductImages).FirstOrDefault();
            homeVM.PopularProducts = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p => p.Rating).Skip(1).Take(3).Include(p => p.ProductImages).ToList();
            homeVM.BestSellerProducts = Bestsellers;
            homeVM.Sliders = _context?.Sliders?.ToList();
            homeVM.Blogs = _context?.Blogs?.Where(b => !b.IsDeleted).OrderByDescending(b => b.ViewCount).Take(4).ToList(); 
            return homeVM;
        }

        public DetailVM Detail(int? id, string? username)
        {
            CorrectDiscountTime();
            DetailVM? detailVM = new DetailVM();
            Products? product = _context?.Products?
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                ?.Include(r => r.UserProductRatings)
                .FirstOrDefaultAsync(p => p.Id == id).GetAwaiter().GetResult();
            double rates = 0;
            List<UserProductRatings>? ratings = _context?.UserProductRatings?
                .Where(r => r.ProductId == product.Id).ToListAsync().GetAwaiter().GetResult();
            if (ratings.Count >= 1)
            {
                foreach (var item in ratings)
                {
                    rates += item.Rating;
                }
                product.Rating = rates / ratings.Count;
                detailVM.Just = rates / ratings.Count;
                detailVM.RatedBy = ratings.Count;
                _context.SaveChangesAsync().GetAwaiter().GetResult();
            }
            AppUser ProductOwner = _usermanager.FindByIdAsync(product.AppUserId).GetAwaiter().GetResult();
            if (ProductOwner != null)
            {
                detailVM.Owner = ProductOwner;
            }
            product.CommentCount = _context.ProductComments.Where(p => p.ProductId == product.Id && !p.IsDeleted).ToList().Count;
            bool ExistWishlist = false;
            bool IsRated = false;
            detailVM.DidUserBuyThis = false;
            if (username!=null&& !UserBanned(username))
            {
                AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
                bool IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).AnyAsync().GetAwaiter().GetResult();
                if (IsExist) detailVM.ExistWishlist = true;
                IsRated = _context.UserProductRatings.Where(r => r.ProductId == product.Id && r.AppUserId == user.Id).AnyAsync().GetAwaiter().GetResult();
                if (IsRated) detailVM.IsRated = true;
                detailVM.AppUserId = user.Id;
                int RightCounter = 0;
                var roles = _usermanager.GetRolesAsync(user).GetAwaiter().GetResult();
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin"))
                    {
                        RightCounter++;
                    }
                }
                detailVM.RightCounter = RightCounter;

                List<Orders>? UserOrders = _context?.Orders?.Where(o => o.AppUserId == user.Id).ToList();
                foreach (var order in UserOrders)
                {
                    bool didUserBuuy = _context.OrderItems.Where(o => o.OrderId == order.Id && o.ProductId == id).AnyAsync().GetAwaiter().GetResult();
                    if (didUserBuuy)
                    {
                        detailVM.DidUserBuyThis = true;
                        break;
                    }
                }
            }
            product.Views++;
            _context.SaveChangesAsync().GetAwaiter().GetResult();
            var UsersWantThis = _context.Wishlists?
                .Where(p => p.ProductId == id)
                .ToListAsync().GetAwaiter().GetResult();
            detailVM.Product = product;

            List<Products> related = _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Include(p => p.ProductImages)
                .Take(4)
                .ToListAsync().GetAwaiter().GetResult();

            detailVM.RelatedProducts = related;


            detailVM.UsersWantIt = UsersWantThis.Count;

            detailVM.Comments =  _context.ProductComments
                .Include(b => b.User)
                .Where(c => c.ProductId == id && !c.IsDeleted)
                .OrderByDescending(b => b.Id)
                .Take(10)
                .ToListAsync().GetAwaiter().GetResult();

            return detailVM;
        }

        public bool ExistProducts(int? id)
        {
            Products? product = _context?.Products?
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                ?.Include(r => r.UserProductRatings)
                .FirstOrDefaultAsync(p => p.Id == id).GetAwaiter().GetResult();
            if (product!=null)
            {
                return true;
            }
            return false;
        }

        public ListProductsVM Brands(int? id, string? username)
        {

            UserBanned(username);
            List<Products>? products = _context?.Products?
                .Where(p => p.BrandId == id)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .ToListAsync().GetAwaiter().GetResult();
            ListProductsVM listProducts = new ListProductsVM
            {
                Products = products,
                Brand = _context?.Brands?.FirstOrDefaultAsync(b => b.Id == id).GetAwaiter().GetResult()
            };

            return listProducts;
        }

        public object Rate(int Rating, int ProductId, string? username)
        {
            bool result = false;
            string? ProductName = "";
            string? ProductImage = "";
            Products? product = _context?.Products?
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == ProductId).GetAwaiter().GetResult();
            if (username != null && !UserBanned(username))
            {
                AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
                if (product.AppUserId != user.Id)
                {
                    UserProductRatings userProductRating = new UserProductRatings
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        Rating = Rating
                    };
                    _context.AddAsync(userProductRating).GetAwaiter().GetResult();
                    _context.SaveChanges();
                    result = true;
                    ProductName = product.Name;
                    foreach (var item in product.ProductImages)
                    {
                        if (item.IsMain)
                        {
                            ProductImage = item.ImageUrl;
                            break;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            var obj = new
            {
                result = result,
                image = ProductImage,
                name = ProductName
            };
            return obj;
        }

        public string RemoveRating(int id, string ReturnUrl, string? username)
        {
            if (username!= null&&!UserBanned(username))
            {
                AppUser AppUser = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
                UserProductRatings? rating = _context?.UserProductRatings?.Where(r => r.AppUserId == AppUser.Id && r.ProductId == id).FirstOrDefaultAsync().GetAwaiter().GetResult();
                _context.UserProductRatings.Remove(rating);
                _context?.SaveChangesAsync().GetAwaiter().GetResult();
            }
            return ReturnUrl;
        }

        public object DeleteComment(int id, string? username)
        {
            AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
            ProductComment? comment = _context?.ProductComments?
                .FirstOrDefaultAsync(bc => bc.Id == id).GetAwaiter().GetResult();
            if (comment.AppUserId == user.Id)
            {
                comment.IsDeleted = true;
                _context.SaveChangesAsync().GetAwaiter().GetResult();
            }
            else
            {
                var roles = _usermanager.GetRolesAsync(user).GetAwaiter().GetResult();
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                        comment.IsDeleted = true;
                        _context.SaveChangesAsync().GetAwaiter().GetResult();
                    }
                }
            }

            var obj = new
            {
                count = _context.ProductComments.Where(b => b.ProductId == comment.ProductId && !b.IsDeleted).ToList().Count
            };

            return obj;
        }

        public CommentsVM LoadComments(int skip, int? BlogId, string? username)
        {

            List<ProductComment>? comments = _context?.ProductComments?
                .Include(b => b.User)
                .Where(bc => bc.ProductId == BlogId && !bc.IsDeleted)
                .OrderByDescending(b => b.Id).Skip(skip).Take(2).ToListAsync().GetAwaiter().GetResult();
            CommentsVM commentsVM = new CommentsVM
            {
                ProductComments = comments
            };
            if (username != null && !UserBanned(username))
            {
                AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
                //seems like this comment is useless, I don't know whether delete it or not
                commentsVM.AppUserId = user.Id;
                int RightCounter = 0;
                var roles = _usermanager.GetRolesAsync(user).GetAwaiter().GetResult();
                //if requester is an admin or editor, he will be able to delete comment
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin") || item.ToLower().Contains("editor") || item.ToLower().Contains("moderator"))
                    {
                        RightCounter++;
                    }
                }

                //if the user is not an admin but a user and he finds any of his comment among those,
                //he will be able to delete his own comment
                commentsVM.UserId = user.Id;

                commentsVM.RightCounter = RightCounter;
            }
            return commentsVM;
        }

        public CommentsVM PostComment(int ProductId, string comment, string? author, string? username)
        {
            Products? product = _context?.Products?
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.Id == ProductId).GetAwaiter().GetResult();
            ProductComment NewComment = new ProductComment();
            CommentsVM commentVM = new CommentsVM();
            if (username != null && !UserBanned(username))
            {
                AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
                NewComment.AppUserId = user.Id;
                commentVM.UserId = user.Id;
                commentVM.User = user;
            }
            else
            {
                NewComment.Author = author;
            }
            NewComment.Content = comment;
            NewComment.ProductId = product.Id;
            NewComment.Date = DateTime.Now.AddHours(12);
            _context.AddAsync(NewComment).GetAwaiter().GetResult();
            _context.SaveChangesAsync().GetAwaiter().GetResult();
            commentVM.ProductComment = NewComment;

            return commentVM;
        }





        public bool UserBanned(string? username)
        {
            AppUser user = _usermanager.FindByNameAsync(username).GetAwaiter().GetResult();
            var userroles = _usermanager.GetRolesAsync(user).GetAwaiter().GetResult();
            foreach (var item in userroles)
            {
                if (item.ToLower() == "ban" || userroles == null)
                {
                    _signInManager.SignOutAsync().GetAwaiter().GetResult();
                    return true;
                }
            }
            return false;
        }

        public void CorrectDiscountTime()
        {
            List<Products>? AllProducts = _context.Products?
                .Where(p => p.DiscountPercent > 0).ToList();

            foreach (var product in AllProducts)
            {
                if (product.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    product.DiscountUntil = null;
                    product.DiscountPercent = 0;
                    product.DiscountPrice = 0;
                    _context?.SaveChanges();
                }
            }
        }
    }
}
