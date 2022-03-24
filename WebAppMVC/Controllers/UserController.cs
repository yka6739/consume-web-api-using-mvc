﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44371/api");
       
        HttpClient client;
        public UserController()
        {
            client = new HttpClient();
            client.BaseAddress= baseAddress;    
        }

        public ActionResult Index()
        {
            List<UserViewModel> modelList = new List<UserViewModel>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress+"/user").Result;
            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<UserViewModel>>(data);
                     
            }
            return View(modelList);
        }



        // Create User Method--------
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel model )
        {
            string data= JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8,"application/json");
            HttpResponseMessage response= client.PostAsync(client.BaseAddress+"/user",content).Result;  
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }




        // Edit User Details--------
        public ActionResult Edit(int id)
        {
            UserViewModel model = new UserViewModel();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/user/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<UserViewModel>(data);

            }
            return View("Create",model);
           
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/user/"+model.UserId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        public ActionResult Delete(int id)
        {
          
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/user/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
   
}