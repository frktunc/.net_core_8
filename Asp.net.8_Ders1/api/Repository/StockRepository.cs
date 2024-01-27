using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _contex;
        public StockRepository(ApplicationDBContext context)
        {
            _contex = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
           await _contex.Stocks.AddAsync(stockModel);
           await _contex.SaveChangesAsync();
           return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _contex.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null){
                return null;
            }
            _contex.Stocks.Remove(stockModel);
            await _contex.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync() 
        {
            return await _contex.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _contex.Stocks.FindAsync(id);
        }

        public Task<bool> StockExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _contex.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingStock == null){
                return null;
            }
        
            existingStock.Symbol = stockDto.Symbol;
            existingStock.MarketCup = stockDto.MarketCup;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Industry = stockDto.Industry;

            await _contex.SaveChangesAsync();
            return existingStock;
        }
    }
}