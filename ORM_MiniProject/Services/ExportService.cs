using ClosedXML.Excel;
using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.Models;
using ORM_MiniProject.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

public class ExportService
{
    private readonly IOrdersService _ordersService;

    public ExportService(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    public async Task<bool> ExportUserOrdersToExcel(int userId, string filePath)
    {
        var oldOrders = await _ordersService.GetAllOrdersAsync();
        List<OrderExcelDto> orders = new List<OrderExcelDto>();
        foreach (var item in oldOrders) {
            OrderExcelDto orderExcelDto = new OrderExcelDto()
            {
                UserId = item.UserId,
                Status = item.Status,
                Id = item.Id,
                OrderDate = item.OrderDate,
                TotalAmount = item.TotalAmount,
                User = item.User
            };
            orders.Add(orderExcelDto);
        }



        if (orders == null || orders.Count == 0)
        {
            Console.WriteLine("No orders found for this user.");
            return false;
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Orders");

            var table = worksheet.FirstCell().InsertTable(orders);

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
        }

        return true;
    }

}

