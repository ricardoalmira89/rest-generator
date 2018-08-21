using System;
using AlmRest.Repository;
using App.Models;

namespace App.Repository
{
    public class {{ classname }}Repository : BaseRepository<{{ classname }}>, IRepository<{{ classname }}>
    {
        public {{ classname }}Repository() {}
    }
}