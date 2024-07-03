using BackendForFrontend202401.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendForFrontend202401.Models.Interfaces
{
    public interface ICartsDetailsRepo
	{
      
        List<CartDetailsDto> Search(int productId); 

        CartDetailsDto Get(int id); 
        void Delete(int id);
        void Update(CartDetailsDto dto);
        int Create(CartDetailsDto dto);

        void UpdateQuantity(CartDetailsDto dto);
    }
}