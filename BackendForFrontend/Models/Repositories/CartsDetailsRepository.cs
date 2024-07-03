using BackendForFrontend.Models.EFModels;
using BackendForFrontend202401.Models.Dtos;
using BackendForFrontend202401.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend202401.Models.Repositories
{
    public class CartsDetailsRepository : ICartsDetailsRepo
    {
        private AppDbContext db = new AppDbContext();


        //抓取一個資料
        public CartDetailsDto Get(int id)
        {
            var model = db.CartDetails.FirstOrDefault(x => x.ProductId == id);
            if (model == null)
            {
                return null;
            }
            var dto = new CartDetailsDto
            {
                ProductName = model.Product.Name,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
            };
            return dto;
        }

        //查詢
        public List<CartDetailsDto> Search(int CartsId)
        {
            return db.CartDetails
               .Include(x => x.Product)
                .Where(x => x.CartId == CartsId)
                                .Select(x => new CartDetailsDto()
                                {
                                    ProductName = x.Product.Name,
                                    Quantity = x.Quantity,
                                    UnitPrice = x.UnitPrice,
                                }).ToList();
        }

        //刪除
        public void Delete(int id)
        {
            var db = new AppDbContext();
            var model = db.CartDetails.Find(id);
            db.CartDetails.Remove(model);
            db.SaveChanges();
        }

        //更新細項
        public void Update(CartDetailsDto dto)
        {

            var model = db.CartDetails.Find(dto.Id);
            model.Quantity = dto.Quantity;
            if (model.Quantity == 0)
            {
                Create(dto);
                return;
            }

            db.SaveChanges();

        }

        //更新數量
        public void UpdateQuantity(CartDetailsDto dto)
        {

            var model = db.CartDetails.Find(dto.Id);
            model.Quantity = dto.Quantity;
            if (model.Quantity == 0)
            {

                Delete((int)dto.Id);
                return;
            }

            db.SaveChanges();

        }

        //創立一個新的
        public int Create(CartDetailsDto dto)
        {
            var model = new CartDetail
            {
                Id = (int)dto.Id,
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
            };
            db.CartDetails.Add(model);
            db.SaveChanges();
            return model.Id;

        }
    }
}