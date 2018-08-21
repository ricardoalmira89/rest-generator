using System;

using AlmRest;
using AlmRest.Repository;

namespace App.Repository
{
    public class {{ classname }}Repository : BaseRepository<{{ classname }}>, IRepository<{{ classname }}>
    {
        public {{ classname }}Repository() {}
    }
}