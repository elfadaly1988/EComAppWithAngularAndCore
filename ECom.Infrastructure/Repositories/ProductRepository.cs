﻿

using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entities.Product;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Core.Sharing;
using ECom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            //for future implementations

        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParam productParams)
        {
            var query = _context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();
            //filtering by word

            if (string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                m.Name.ToLower().Contains(word.ToLower()) 
                ||
                m.Description.ToLower().Contains(word.ToLower())
                ));
                    
            }



            //filtering by category ID 
            if (productParams.CategoryId.HasValue) query = query.Where(m => m.CategoryId == productParams.CategoryId);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsn" => query.OrderBy(m => m.NewPrice),
                    "PriceDsc" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.NewPrice),
                };
            }
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber) - 1).Take(productParams.PageSize);
            var result = _mapper.Map<List<ProductDTO>>(query);
            return result;


        }
        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = _mapper.Map<Product>(productDTO);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var imagePath = await _imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();
            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null) return false;
            var findProduct = await _context.Products.Include(m => m.Category)
                .Include(m => m.Photos).FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);
            if (findProduct is null) return false;

            _mapper.Map(updateProductDTO, findProduct);
            var findPhoto = await _context.Photos.Where(m => m.ProductId == updateProductDTO.Id).ToListAsync();
            foreach (var item in findPhoto)
            {
                _imageManagementService.DeleteImageAsync(item.ImageName);
            }
            _context.Photos.RemoveRange(findPhoto);
            var imagePath = await _imageManagementService.AddImageAsync(updateProductDTO.Photo, findProduct.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = findProduct.Id
            }).ToList();
            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(Product product)
        {
            var photo = await _context.Photos.Where(x => x.ProductId == product.Id).ToListAsync();
            foreach (var item in photo)
            {
                _imageManagementService.DeleteImageAsync(item.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
