﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TestController : Controller
    {

        // GET: Test
        public string Index()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string users = "";
                return users += db.Users.FirstOrDefault().ToString();
                //return "users";

                //IdentityRole role = new IdentityRole("newrole");
                //RoleManager man = new RoleManager();

            }
        }
    }
}