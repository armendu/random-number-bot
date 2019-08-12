﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Tarscord.Persistence.Interfaces;

namespace Tarscord.Persistence.Respositories
{
    public class BaseRepository<T>: IBaseRepository<T> where T : class
    {
        private readonly IDatabaseConnection _connection;

        public BaseRepository(IDatabaseConnection connection)
        {
            _connection = connection;
        }

        public async Task<IQueryable<T>> FindBy(Func<T, bool> predicate)
        {
            IEnumerable<T> results = await _connection.Connection.GetAllAsync<T>();
            results = results?.Where(predicate);

            return results?.AsQueryable();
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            IEnumerable<T> items = await _connection.Connection.GetAllAsync<T>();

            return items.AsQueryable();
        }

        public async Task<T> CreateAsync(T item)
        {
//            using (var transaction = _connection.Connection.BeginTransaction())
//            {
                await _connection.Connection.InsertAsync(item);
//                transaction.Commit();
//            }

            return item;
        }

        public async Task<T> UpdateItem(T item)
        {
            using (var transaction = _connection.Connection.BeginTransaction())
            {
                try
                {
                    await _connection.Connection.UpdateAsync(item);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return item;
        }

        public async Task DeleteItem(T item)
        {
            using (var transaction = _connection.Connection.BeginTransaction())
            {
                try
                {
                    await _connection.Connection.DeleteAsync(item);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
    }
}