using System.Collections.Generic;

namespace WebApi.DTO.Product_DTOs
{
    public class ProductListDto
    {
        public int Total { get; set; }
        public List<ProductReturnDto>? Items { get; set; }
    }
}
