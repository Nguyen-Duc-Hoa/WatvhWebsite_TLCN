﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchWebsite_TLCN.DTO;
using WatchWebsite_TLCN.Entities;
using WatchWebsite_TLCN.Intefaces;
using WatchWebsite_TLCN.IRepository;
using WatchWebsite_TLCN.Models;
using WatchWebsite_TLCN.Utilities;

namespace WatchWebsite_TLCN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _product;

        public ProductsController(IUnitOfWork unitOfWork, IProductsRepository product, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _product = product;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _unitOfWork.Products.GetAll();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _unitOfWork.Products.Get(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Products.Update(product);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ProductExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _unitOfWork.Products.Insert(product);
            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (!(await ProductExists(product.Id)))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await _unitOfWork.Products.Get(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            await _unitOfWork.Products.Delete(id);
            await _unitOfWork.Save();

            return product;
        }

        private Task<bool> ProductExists(string id)
        {
            return _unitOfWork.Products.IsExist<string>(id);
        }

        //GET: api/products/PopularProduct
        [HttpGet]
        [Route("PopularProduct")]
        public IEnumerable<Product1DTO> GetPopularProducts()
        {
            return _product.GetPopularProduct().ToList();
        }

        //Get product detail
        [HttpGet]
        [Route("ProductDetail/{id}")]
        public ProductDetail GetProductDetail(string id)
        {
            return _product.GetProductDetail(id);
        }

        // GET: api/Products/Search&currentPage=1&searchKey=abc
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchProducts(int currentPage, string searchKey)
        {
            Expression<Func<Product, bool>> expression = null;
            expression = p => p.Name.Contains(searchKey);

            var result = await _unitOfWork.Products.GetAllWithPagination(
                expression: expression,
                orderBy: p => p.OrderBy(x => x.Name),
                pagination: new Pagination { CurrentPage = currentPage });

            var listProductDTO = _mapper.Map<List<ProductDTO>>(result.Item1);

            return Ok(new
            {
                Products = listProductDTO,
                CurrentPage = result.Item2.CurrentPage,
                TotalPage = result.Item2.TotalPage
            });
        }

        // POST: /api/Products&currentPage=1
        [HttpPost]
        [Route("FilterProduct")]
        public async Task<IActionResult> Filter(int currentPage, [FromBody] FilterProduct filter)
        {
            // Specify Max, Min
            double[] limit = new double[2];
            limit[0] = 0;
            limit[1] = int.MaxValue;
            if (filter.Prices != null)
            {
                /*
                 * Ex:
                 * 30/90 (tu 30 toi 90)
                 * 90/200 (tu 90 toi 200)
                 * 200/-1 (lon hon 200)
                 */
                limit = Array.ConvertAll(filter.Prices.Split('/'), Double.Parse);
                if (limit[1] == -1)
                {
                    limit[1] = int.MaxValue;
                }
            }

            Expression<Func<Product, bool>> expression = PredicateBuilder.True<Product>();

            // Filter price
            if (filter.Prices != null)
            {
                expression = expression.And(p => p.Price > limit[0] && p.Price <= limit[1]);
            }
            var result1 = await _unitOfWork.Products.GetAllWithPagination(
                expression: expression,
                pagination: new Pagination { CurrentPage = currentPage });

            // Filter gender (1: Male, 0: Female)
            if (filter.Gender != -1)
            {
                expression = expression.And(p => p.Gender == filter.Gender);
            }

            //Fitler brands
            foreach (var b in filter.Brands)
            {
                expression = expression.And(p => p.Brand.Name == b);
            }

            //Fitler sort by
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;

            switch (filter.SortBy)
            {
                case Constant.alphabetically:
                    orderBy = p => p.OrderBy(x => x.Name);
                    break;
                case Constant.nonAlphabetically:
                    orderBy = p => p.OrderByDescending(x => x.Name);
                    break;
                case Constant.priceAscending:
                    orderBy = p => p.OrderBy(x => x.Price);
                    break;
                case Constant.priceDesending:
                    orderBy = p => p.OrderByDescending(x => x.Price);
                    break;
                default:
                    orderBy = p => p.OrderByDescending(x => x.Sold);
                    break;
            }

            var result = await _unitOfWork.Products.GetAllWithPagination(
                expression: expression,
                orderBy: orderBy,
                pagination: new Pagination { CurrentPage = currentPage },
                includes: new List<String>() { "Brand" });

            var listProductDTO = _mapper.Map<List<ProductDTO>>(result.Item1);

            return Ok(new
            {
                Products = listProductDTO,
                CurrentPage = result.Item2.CurrentPage,
                TotalPage = result.Item2.TotalPage
            });
        }
    }
}