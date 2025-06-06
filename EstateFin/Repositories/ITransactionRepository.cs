﻿using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        Transaction GetById(int id);
        IEnumerable<Transaction> GetAll(int id);
        void Update(Transaction transaction);
        void Delete(int id);
    }
}
