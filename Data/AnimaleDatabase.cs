﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using PetAdoptM.Models;

namespace PetAdoptM.Data
{
    public class AnimaleDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public AnimaleDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Animale>().Wait();
            _database.CreateTableAsync<Tipuri>().Wait();
            _database.CreateTableAsync<ListTipuri>().Wait();
            _database.CreateTableAsync<Locatie>().Wait();
        }

        // Tipuri 
        public Task<int> SaveTipAsync(Tipuri tip)
        {
            if (tip.ID != 0)
                return _database.UpdateAsync(tip);
            else
                return _database.InsertAsync(tip);
        }

        public Task<int> DeleteTipAsync(int tipId)
        {
            return _database.DeleteAsync<Tipuri>(tipId);
        }


        public Task<List<Tipuri>> GetTipAsync()
        {
            return _database.Table<Tipuri>().ToListAsync();
        }

        public Task<List<Tipuri>> GetTipAsync(int animalId)
        {
            return _database.QueryAsync<Tipuri>(
                "SELECT * FROM Animale A " +
                "INNER JOIN ListTipuri LI ON A.ID = LI.AnimaleID " +
                "INNER JOIN Tipuri I ON LI.TipuriID = I.ID " +
                "WHERE LI.AnimaleID = ?", animalId);
        }

        // Animale 
        public async Task<int> SaveAnimaleAsync(Animale animale)
        {
            if (animale.ID != 0)
            {
                return await _database.UpdateAsync(animale);
            }
            else
            {
                
                if (animale.Locatie != null && animale.Locatie.ID == 0)
                {
                    await _database.InsertAsync(animale.Locatie);
                }

                return await _database.InsertAsync(animale);
            }
        }

        public Task<int> DeleteAnimaleAsync(Animale animale)
        {
            return _database.DeleteAsync(animale);
        }

        public Task<List<Animale>> GetAnimaleAsync(int id)
        {
            return _database.Table<Animale>().ToListAsync();
        }

        public Task<List<Animale>> GetRecipeAsync()
        {
            return _database.Table<Animale>().ToListAsync();
        }

        // ListTipuri 
        public Task<int> SaveListTipAsync(ListTipuri listTip)
        {
            if (listTip.ID != 0)
                return _database.UpdateAsync(listTip);
            else
                return _database.InsertAsync(listTip);
        }

        public Task<ListTipuri> GetTipDetailsAsync(int tipId)
{
    return _database.Table<ListTipuri>().FirstOrDefaultAsync(x => x.TipuriID == tipId);
}

        // Locatie 
        public Task<int> SaveLocatieAsync(Locatie locatie)
        {
            if (locatie.ID != 0)
                return _database.UpdateAsync(locatie);
            else
                return _database.InsertAsync(locatie);
        }

        public Task<int> DeleteLocatieAsync(Locatie locatie)
        {
            return _database.DeleteAsync(locatie);
        }

        public Task<List<Locatie>> GetLocatieAsync()
        {
            return _database.Table<Locatie>().ToListAsync();
        }

        public Task<Locatie> GetLocatieAsync(int locatieId)
        {
            return _database.Table<Locatie>().FirstOrDefaultAsync(l => l.ID == locatieId);
        }
    }
}