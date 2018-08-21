using System;
using App.Models;
using App.Repository;

using AlmRest;
using AlmRest.Repository;
using AlmRest.Controller;

namespace App.Controllers
{
    public class {{ classname }}Controller : GenericController<{{ classname }}>
    {
        public {{ classname }}Controller()
        {
            this.repository = new {{ classname }}Repository();
        }
        
    }
}