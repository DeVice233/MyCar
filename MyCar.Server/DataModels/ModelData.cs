﻿using Microsoft.EntityFrameworkCore;
using ModelsApi;
using MyCar.Server.DB;

namespace MyCar.Server.DataModels
{
    public static class ModelData 
    {
        //Старый ModelData
        //public static DiscountApi GetDiscount(Discount discount, MyCar_DBContext dbContext)
        //{
        //    var result = (DiscountApi)discount;
        //    var saleCar = dbContext.SaleCars.FirstOrDefault(s => s.Id == discount.SaleCarId);
        //    var car = dbContext.Cars.FirstOrDefault(s => s.Id == saleCar.CarId);
        //    result.SaleCar = (SaleCarApi)saleCar;
        //    result.SaleCar.Car = GetCar(car, dbContext);
        //    if (result.SaleCar.FullPrice != 0)
        //        result.PercentValue = Math.Round((decimal)(result.DiscountValue / result.SaleCar.FullPrice) * 100, 2);
        //    else
        //        result.PercentValue = 0;
        //    return result;
        //}
        //public static CarApi GetCar(Car car, MyCar_DBContext dbContext)
        //{
        //    var result = (CarApi)car;
        //    var characteristicsCar = dbContext.CharacteristicCars.Where(t => t.CarId == car.Id).Select(t => (CharacteristicCarApi)t).ToList();
        //    foreach (var characeristicCar in characteristicsCar)
        //    {
        //        characeristicCar.Characteristic = (CharacteristicApi)dbContext.Characteristics.FirstOrDefault(s => s.Id == characeristicCar.CharacteristicId);
        //        characeristicCar.Characteristic.Unit = (UnitApi)dbContext.Units.FirstOrDefault(s => s.Id == characeristicCar.Characteristic.UnitId);

        //    }
        //    result.CharacteristicCars = characteristicsCar;
        //    var model = dbContext.Models.FirstOrDefault(t => t.Id == car.ModelId);
        //    result.Model = (ModelApi)model;
        //    var mark = dbContext.MarkCars.FirstOrDefault(i => i.Id == model.MarkId);
        //    result.CarMark = mark.MarkName;
        //    result.Model.MarkCar = (MarkCarApi)mark;
        //    var body = dbContext.BodyTypes.FirstOrDefault(b => b.Id == car.TypeId);
        //    result.BodyType = (BodyTypeApi)body;
        //    foreach (var characteristic in dbContext.CharacteristicCars.Where(s => s.CarId == car.Id).ToList())
        //    {
        //        characteristic.Characteristic = dbContext.Characteristics.FirstOrDefault(s => s.Id == characteristic.CharacteristicId);
        //        result.CarOptions += $"{characteristic.Characteristic.CharacteristicName} {characteristic.CharacteristicValue} \n";
        //    }
        //    return result;
        //}

        //public static SaleCarApi SaleGet(SaleCar saleCar, MyCar_DBContext dbContext)
        //{
        //    var result = (SaleCarApi)saleCar;
        //    var mainPhoto = dbContext.CarPhotos.FirstOrDefault(s => s.SaleCarId == saleCar.Id && s.IsMainPhoto == 1);
        //    if (mainPhoto != null && mainPhoto.PhotoName != null)
        //    {
        //        result.MainPhotoCar = mainPhoto.PhotoName;
        //    }
        //    var carPhotos = dbContext.CarPhotos.Where(s => s.SaleCarId == saleCar.Id).Select(t => (CarPhotoApi)t).ToList();
        //    result.CarPhotos = carPhotos;
        //    var equipment = dbContext.Equipment.FirstOrDefault(s => s.Id == saleCar.EquipmentId);
        //    result.Equipment = (EquipmentApi)equipment;
        //    var car = dbContext.Cars.FirstOrDefault(s => s.Id == saleCar.CarId);
        //    var wareHouses = new List<Warehouse>(dbContext.Warehouses.Where(s => s.SaleCarId == saleCar.Id));
        //    foreach (var ware in wareHouses)
        //    {
        //        //Проверека на то что заказ из которого берем количество не отменен
        //        if (dbContext.Statuses.First(s => s.Id == dbContext.Orders.First(s => s.Id == ware.OrderId).StatusId).StatusName == "Отменен")
        //            continue;
        //        result.Count += ware.CountChange;
        //    }
        //    result.Car = GetCar(car, dbContext);
        //    return result;
        //}

        //public static WareHouseApi WarehouseGet(Warehouse wareHouse, MyCar_DBContext dbContext)
        //{
        //    var result = (WareHouseApi)wareHouse;

        //    var order = (OrderApi)dbContext.Orders.FirstOrDefault(a => a.Warehouses.Any(s => s.OrderId == a.Id));
        //    order.ActionType = (ActionTypeApi)dbContext.ActionTypes.First(s => s.Id == order.ActionTypeId);
        //    order.Status = (StatusApi)dbContext.Statuses.First(s => s.Id == order.StatusId);

        //    //если заказ на поступление, считаем сколько авто осталось в каждом WH
        //    if (order.ActionType.ActionTypeName == "Поступление")
        //    {
        //        result.CountRemains = (int)result.CountChange; //сначала назначаем сколько изначально пришло в поставке

        //        List<CountChangeHistory> thisCountChangeHistories =
        //            new List<CountChangeHistory>(dbContext.CountChangeHistories.Where(s => s.WarehouseInId == result.ID).ToList()); //затем находим изменения связанные с нашей поставкой

        //        if (thisCountChangeHistories.Count != 0) //если они есть, вычитаем из изначального количества которое было в поставке, значение которое было продано из этой поставки
        //        {
        //            foreach (CountChangeHistory countChangeHis in thisCountChangeHistories)
        //            {
        //                //снизу также длинная проверка на то что заказ на продажу который связан с нашей поставкой не отменен
        //                if (dbContext.Statuses.First(s => s.Id == dbContext.Orders.First(o => o.Id == dbContext.Warehouses.First(w => w.Id == countChangeHis.WarehouseOutId).OrderId).StatusId).StatusName == "Отменен")
        //                    continue;
        //                result.CountRemains -= (int)countChangeHis.Count;
        //            }
        //        }
        //    }
        //    var sale = dbContext.SaleCars.FirstOrDefault(s => s.Id == result.SaleCarId);
        //    result.SaleCar = SaleGet(sale, dbContext);
        //    return result;
        //}

        //public static CountChangeHistoryApi GetCount(CountChangeHistory countChange, MyCar_DBContext dbContext)
        //{
        //    var result = (CountChangeHistoryApi)countChange;
        //    var warehouseIn = dbContext.Warehouses.FirstOrDefault(s => s.Id == countChange.WarehouseInId);
        //    var warehouseOut = dbContext.Warehouses.FirstOrDefault(s => s.Id == countChange.WarehouseOutId);
        //    result.WarehouseIn = WarehouseGet(warehouseIn, dbContext);
        //    result.WarehouseOut = WarehouseGet(warehouseOut, dbContext);
        //    return result;
        //}

        //public static OrderApi OrderGet(Order orderIn, MyCar_DBContext dbContext)
        //{
        //    var result = (OrderApi)orderIn;
        //    var user = dbContext.Users.FirstOrDefault(x => x.Id == orderIn.UserId);
        //    result.User = (UserApi)user;
        //    var status = dbContext.Statuses.FirstOrDefault(x => x.Id == orderIn.StatusId);
        //    result.Status = (StatusApi)status;
        //    var actionType = dbContext.ActionTypes.FirstOrDefault(x => x.Id == orderIn.ActionTypeId);
        //    result.ActionType = (ActionTypeApi)actionType;
        //    var thisWarehouses = dbContext.Warehouses.Where(s => s.OrderId == orderIn.Id).ToList();
        //    result.WareHouses = thisWarehouses.Select(s => (WarehouseGet(s, dbContext))).ToList(); //result.WareHouses = thisWarehouses.Select(s=> (WareHouseApi)s).ToList();
        //    return result;
        //}

        //Новый ModelData (Типа оптимизированный)
        public static DiscountApi GetDiscount(Discount discount, MyCar_DBContext dbContext)
        {
            var result = (DiscountApi)discount;

            var saleCar = dbContext.SaleCars
                .Include(s => s.Car)
                    .ThenInclude(c => c.Model)
                        .ThenInclude(m => m.Mark)
                .FirstOrDefault(s => s.Id == discount.SaleCarId);

            result.SaleCar = (SaleCarApi)saleCar;
            result.SaleCar.Car = GetCar(saleCar.Car, dbContext);

            if (result.SaleCar.FullPrice != 0)
                result.PercentValue = Math.Round((decimal)(result.DiscountValue / result.SaleCar.FullPrice) * 100, 2);
            else
                result.PercentValue = 0;

            return result;
        }
        public static CarApi GetCar(Car car, MyCar_DBContext dbContext)
        {
            var result = (CarApi)car;

            var characteristicCars = dbContext.CharacteristicCars
                .Include(cc => cc.Characteristic)
                    .ThenInclude(c => c.Unit)
                .Where(cc => cc.CarId == car.Id)
                .Select(cc => (CharacteristicCarApi)cc)
                .ToList();

            result.CharacteristicCars = characteristicCars;

            var carOptions = dbContext.CharacteristicCars
                .Include(cc => cc.Characteristic)
                .Where(cc => cc.CarId == car.Id)
                .Select(cc => $"{cc.Characteristic.CharacteristicName} {cc.CharacteristicValue}")
                .ToList();

            result.CarOptions = string.Join("\n", carOptions);

            var modelId = car.ModelId;
            var model = dbContext.Models
                .Include(m => m.Cars)
                .FirstOrDefault(m => m.Id == modelId);

            result.Model = (ModelApi)model;
            result.CarMark = model.Mark?.MarkName;

            var bodyTypeId = car.TypeId;
            var bodyType = dbContext.BodyTypes.FirstOrDefault(bt => bt.Id == bodyTypeId);
            result.BodyType = (BodyTypeApi)bodyType;

            return result;
        }

        public static SaleCarApi SaleGet(SaleCar saleCar, MyCar_DBContext dbContext)
        {
            var result = (SaleCarApi)saleCar;

            var mainPhoto = dbContext.CarPhotos.FirstOrDefault(s => s.SaleCarId == saleCar.Id && s.IsMainPhoto == 1);
            if (mainPhoto != null && mainPhoto.PhotoName != null)
            {
                result.MainPhotoCar = mainPhoto.PhotoName;
            }

            var carPhotos = dbContext.CarPhotos
                .Where(s => s.SaleCarId == saleCar.Id)
                .Select(t => (CarPhotoApi)t)
                .ToList();

            result.CarPhotos = carPhotos;

            var equipment = dbContext.Equipment.FirstOrDefault(s => s.Id == saleCar.EquipmentId);
            result.Equipment = (EquipmentApi)equipment;

            var car = dbContext.Cars
                .Include(c => c.Model)
                    .ThenInclude(m => m.Mark)
                .FirstOrDefault(s => s.Id == saleCar.CarId);

            var warehouseCount = dbContext.Warehouses
                .Where(w => w.SaleCarId == saleCar.Id && dbContext.Orders.Any(o => o.Id == w.OrderId && o.Status.StatusName != "Отменен"))
                .Sum(w => w.CountChange);

            result.Count = warehouseCount;

            result.Car = GetCar(car, dbContext);

            return result;
        }
        public static WareHouseApi WarehouseGet(Warehouse wareHouse, MyCar_DBContext dbContext)
        {
            var result = (WareHouseApi)wareHouse;

            var order = dbContext.Orders
                .Include(o => o.ActionType)
                .Include(o => o.Status)
                .FirstOrDefault(a => a.Warehouses.Any(s => s.OrderId == a.Id));


            //если заказ на поступление, считаем сколько авто осталось в каждом WH
            if (order.ActionType.ActionTypeName == "Поступление")
            {
                result.CountRemains = (int)result.CountChange; //сначала назначаем сколько изначально пришло в поставке

                List<CountChangeHistory> thisCountChangeHistories =
                    new List<CountChangeHistory>(dbContext.CountChangeHistories.Where(s => s.WarehouseInId == result.ID).ToList()); //затем находим изменения связанные с нашей поставкой

                if (thisCountChangeHistories.Count != 0) //если они есть, вычитаем из изначального количества которое было в поставке, значение которое было продано из этой поставки
                {
                    foreach (CountChangeHistory countChangeHis in thisCountChangeHistories)
                    {
                        //снизу также длинная проверка на то что заказ на продажу который связан с нашей поставкой не отменен
                        if (dbContext.Statuses.First(s => s.Id == dbContext.Orders.First(o => o.Id == dbContext.Warehouses.First(w => w.Id == countChangeHis.WarehouseOutId).OrderId).StatusId).StatusName == "Отменен")
                            continue;
                        result.CountRemains -= (int)countChangeHis.Count;
                    }
                }
            }

            var sale = dbContext.SaleCars.FirstOrDefault(s => s.Id == result.SaleCarId);
            result.SaleCar = SaleGet(sale, dbContext);

            return result;
        }

        public static CountChangeHistoryApi GetCount(CountChangeHistory countChange, MyCar_DBContext dbContext)
        {
            var result = (CountChangeHistoryApi)countChange;

            var warehouseInId = countChange.WarehouseInId;
            var warehouseOutId = countChange.WarehouseOutId;

            var warehouseIn = dbContext.Warehouses
                .Include(w => w.Order)
                    .ThenInclude(o => o.Status)
                .Include(w => w.Order)
                    .ThenInclude(o => o.ActionType)
                .FirstOrDefault(w => w.Id == warehouseInId);

            var warehouseOut = dbContext.Warehouses
                .Include(w => w.Order)
                    .ThenInclude(o => o.Status)
                .Include(w => w.Order)
                    .ThenInclude(o => o.ActionType)
                .FirstOrDefault(w => w.Id == warehouseOutId);

            result.WarehouseIn = WarehouseGet(warehouseIn, dbContext);
            result.WarehouseOut = WarehouseGet(warehouseOut, dbContext);

            return result;
        }

        public static OrderApi OrderGet(Order orderIn, MyCar_DBContext dbContext)
        {
            var result = (OrderApi)orderIn;

            var user = dbContext.Users.FirstOrDefault(x => x.Id == orderIn.UserId);
            result.User = (UserApi)user;

            var status = dbContext.Statuses.FirstOrDefault(x => x.Id == orderIn.StatusId);
            result.Status = (StatusApi)status;

            var actionType = dbContext.ActionTypes.FirstOrDefault(x => x.Id == orderIn.ActionTypeId);
            result.ActionType = (ActionTypeApi)actionType;

            var orderWarehouses = dbContext.Warehouses
                .Include(w => w.Order)
                .Where(w => w.Order.Id == orderIn.Id)
                .ToList();

            result.WareHouses = orderWarehouses.Select(w => WarehouseGet(w, dbContext)).ToList();

            return result;
        }
    }
}
