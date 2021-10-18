﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchWebsite_TLCN.DTO;
using WatchWebsite_TLCN.Entities;

namespace WatchWebsite_TLCN.Intefaces
{
    public interface ICartsRepository
    {
        IEnumerable<CartDTO> GetCart(int userId);
        bool ProductExists(Cart cart);
        bool AddToCart(Cart cart);
        bool IncreaseQuantity(Cart cart);
        bool DecreaseQuantity(Cart cart);
        bool DeleteFromCart(Cart cart);
        bool Save();
    }
}
