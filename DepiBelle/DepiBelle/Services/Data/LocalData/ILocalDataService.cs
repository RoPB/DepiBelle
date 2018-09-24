﻿using System;
using System.Threading.Tasks;
using DepiBelle.Models;

namespace DepiBelle.Services.Data.LocalData
{
    public interface ILocalDataService
    {
        Task<bool> Contains(string key);

        Task<T> Get<T>(string key);

        Task<bool> AddOrReplace<T>(string key, T item);

        Task<bool> Remove(string key);
    }
}
