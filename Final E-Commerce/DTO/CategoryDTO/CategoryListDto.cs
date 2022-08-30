using System.Collections.Generic;
namespace WebApi.DTO.CategoryDTO
{
    public class CategoryListDto
    {
        public int Total { get; set; }
        public List<CategoryReturnDto>? Items { get; set; }
    }
}