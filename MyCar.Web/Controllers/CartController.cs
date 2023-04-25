﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using MyCar.Web.Core;
using System.Data;

namespace MyCar.Web.Controllers
{
    public class CartController : Controller
    {
        public List<OrderApi> Orders = new List<OrderApi>();

        public List<SaleCarApi> Cars { get; set; } = new List<SaleCarApi>();

        public List<UserApi> Users { get; set; } = new List<UserApi>();

        public List<ActionTypeApi> Types { get; set; }

        public List<StatusApi> Statuses { get; set; }

        public List<WareHouseApi> Warehouses { get; set; } = new List<WareHouseApi>();
        // GET: CartController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CartController/Details/5

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Администратор, Клиент")]
        public IActionResult CartPage(string name)
        {
            name = User.Identity.Name;
            return View("CartPage", GetPage(name).Result);
        }

        public async Task<List<OrderApi>> GetPage(string name)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            var order = Orders.Where(s => s.User.UserName == name).ToList();
            return order;
        }

        public async Task GetOrders()
        {
            Cars = await Api.GetListAsync<List<SaleCarApi>>("CarSales");
            Warehouses = await Api.GetListAsync<List<WareHouseApi>>("Warehouse");
            Users = await Api.GetListAsync<List<UserApi>>("User");
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");
            Types = await Api.GetListAsync<List<ActionTypeApi>>("ActionType");
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
        }

        public async Task<IActionResult> AddOrder(int id)
        {
            await GetOrders();

            var user = Users.FirstOrDefault(s => s.UserName == User.Identity.Name);
            
            var ord = Orders.LastOrDefault(s => s.UserId == user.ID);
            if (ord == null || ord.Status.StatusName == "Завершен")
            {
                var car = Cars.FirstOrDefault(s => s.ID == id);
                WareHouseApi wareHouse = new WareHouseApi { ID = Warehouses.Count() + 1, CountChange = -1, SaleCar = car, SaleCarId = car.ID, Price = DiscountCounter.GetDiscount(car), Discount = DiscountCounter.GetDiscountPrice(car)};

                OrderApi order = new OrderApi();
                order.WareHouses = new List<WareHouseApi>();
                order.WareHouses.Add(wareHouse);
                order.SumOrder = car.FullPrice - wareHouse.Discount;
                    
                order.User = user;
                order.UserId = user.ID;
                order.DateOfOrder = DateTime.Now;
                order.StatusId = 1;
                order.Status = Statuses.FirstOrDefault(s => s.ID == order.StatusId);
                order.ActionTypeId = 2;
                order.ActionType = Types.FirstOrDefault(s => s.ID == order.ActionTypeId);
                await CreateOrder(order);
                wareHouse.OrderId = order.ID;
                return View("DetailsCart", order);
            }

            else
            {
                var order = Orders.LastOrDefault(s => s.UserId == user.ID);
                var car = Cars.FirstOrDefault(s => s.ID == id);

                WareHouseApi wareHouse = new WareHouseApi
                {
                    ID = Warehouses.Count() + 1,
                    SaleCar = car,
                    SaleCarId = car.ID,
                    CountChange = -1,
                    Discount = DiscountCounter.GetDiscountPrice(car),
                    Price = DiscountCounter.GetDiscount(car),
                    OrderId = order.ID,
                };
                if (wareHouse.SaleCar.Car.CarMark.Contains("Toyota") || wareHouse.SaleCar.Car.CarMark.Contains("Lexus"))
                {
                    wareHouse.Discount = car.FullPrice * 10 / 100;
                }
                order.WareHouses.Add(wareHouse);
                order.SumOrder = car.FullPrice - wareHouse.Discount;
                await EditOrder(order);
                await CreateWareHouse(wareHouse);
                return View("DetailsCart", order);

            }
        }

        public async Task<IActionResult> AddCars(int id)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            var order = Orders.FirstOrDefault(s => s.ID == id);
            return View("DetailsCart", order);
        }

        public async Task<IActionResult> DetailsCart(string name)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            Cars = await Api.GetListAsync<List<SaleCarApi>>("CarSales");
            Warehouses = await Api.GetListAsync<List<WareHouseApi>>("Warehouse");
            Users = await Api.GetListAsync<List<UserApi>>("User");
            var user = Users.FirstOrDefault(s => s.UserName == name);
            var order = Orders.LastOrDefault(s => s.User.UserName == name && s.Status.StatusName == "Новый");
            if (order == null)
            {
                return View("Error");
            }
            else if (order.WareHouses == null || order.WareHouses.Count == 0)
            {
                DeleteOrder(order);
                return View("Error");
            }
            order.WareHouses = Warehouses.Where(s => s.OrderId == order.ID).ToList();
            return View("DetailsCart", order);
        }

        public async Task<IActionResult> DeleteCar(int id)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            Cars = await Api.GetListAsync<List<SaleCarApi>>("CarSales");
            Warehouses = await Api.GetListAsync<List<WareHouseApi>>("Warehouse");
            var deleteCar = Warehouses.FirstOrDefault(s => s.ID == id);
            var order = Orders.FirstOrDefault(s => s.ID == deleteCar.OrderId);
            order.WareHouses.Remove(deleteCar);
            await DeleteCar(deleteCar);
            return View("DetailsCart", order);
        }

        public async Task<IActionResult> DetailsOrder(int id)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            Cars = await Api.GetListAsync<List<SaleCarApi>>("CarSales");
            Warehouses = await Api.GetListAsync<List<WareHouseApi>>("Warehouse");
            Users = await Api.GetListAsync<List<UserApi>>("User");
            var order = Orders.LastOrDefault(s => s.ID == id);
            if (order == null)
            {
                return View("Error");
            }
            order.WareHouses = Warehouses.Where(s => s.OrderId == order.ID).ToList();
            order.SumOrder = order.WareHouses.Sum(s => s.Price);
            return View("DetailsOrder", order);
        }

        public async Task<IActionResult> ConfirmOrder(int id)
        {
            Orders = await Api.GetListAsync<List<OrderApi>>("Order");
            Cars = await Api.GetListAsync<List<SaleCarApi>>("CarSales");
            Warehouses = await Api.GetListAsync<List<WareHouseApi>>("Warehouse");
            Statuses = await Api.GetListAsync<List<StatusApi>>("Status");
            var order = Orders.FirstOrDefault(s => s.ID == id);
            if (order == null)
            {
                return View("Error");
            }
            var status = Statuses.FirstOrDefault(s => s.StatusName == "Завершен");
            order.Status = status;
            order.StatusId = status.ID;
            await EditOrder(order);
            return View("DetailsCart", order);
        }

        public async Task CreateOrder(OrderApi orderApi)
        {
            var order = await Api.PostAsync<OrderApi>(orderApi, "Order");
        }

        public async Task EditOrder(OrderApi orderApi)
        {
            var order = await Api.PutAsync<OrderApi>(orderApi, "Order");
        }

        private async Task DeleteCar(WareHouseApi wareHouse)
        {
            var warehouse = await Api.DeleteAsync<WareHouseApi>(wareHouse, "Warehouse");
        }

        private async Task DeleteOrder(OrderApi orderApi)
        {
            var order = await Api.DeleteAsync<OrderApi>(orderApi, "Order");
        }

        public async Task CreateWareHouse(WareHouseApi wareHouse)
        {
            var order = await Api.PostAsync<WareHouseApi>(wareHouse, "Warehouse");
        }

        public async Task EditWareHouse(WareHouseApi wareHouse)
        {
            var order = await Api.PutAsync<WareHouseApi>(wareHouse, "Warehouse");
        }
    }
}