using ORM_MiniProject.DTOs.OrderDetailDTOs;
using ORM_MiniProject.DTOs.OrderDTOs;
using ORM_MiniProject.DTOs.PaymentDTOs;
using ORM_MiniProject.DTOs.ProductDTOs;
using ORM_MiniProject.DTOs.UserDTOs;
using ORM_MiniProject.Exceptions;
using ORM_MiniProject.Models;
using ORM_MiniProject.Services.Implementations;
using ORM_MiniProject.Services.Interfaces;

namespace ORM_MiniProject;

internal class Program
{
    static async Task Main(string[] args)
    {

        IUsersService usersService = new UserService();
        IOrdersService ordersService = new OrdersService();
        IProductService productService = new ProductService();
        IPaymentService paymentService = new PaymentService();
        IOrderDetailsService orderDetailsService = new OrderDetailsService();
        ExportService exportService = new ExportService(ordersService);
        Users loginnedUser = new();
        UserMenu:
        Console.WriteLine("1.Register\n" +
            "2.Login\n"+
            "3.Quit");
        Console.WriteLine("choose option");
        string userChoice =  Console.ReadLine();
        switch (userChoice) {
            case "1":
                try
                {
                    Console.WriteLine("Enter fullname:");
                    string registerFullName = Console.ReadLine();
                    Console.WriteLine("Enter email:");
                    string registerEmail = Console.ReadLine();
                    Console.WriteLine("Enter Password");
                    string registerPassword = Console.ReadLine();
                    Console.WriteLine("Enter Address:");
                    string registerAddress = Console.ReadLine();
                    UserPostDto userPostDto = new UserPostDto()
                    {
                        Address = registerAddress,
                        Email = registerEmail,
                        FullName = registerFullName,
                        Password = registerPassword
                    };
                    await usersService.CreateUserAsync(userPostDto);
                    Console.WriteLine("press enter");
                    Console.ReadKey();
                    goto UserMenu;
                }

                catch (InvalidUserInformationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("press enter");
                    Console.ReadKey();
                    goto UserMenu;
                }
                catch(SameEmailException e) {  Console.WriteLine(e.Message);
                    Console.WriteLine("press enter");
                    Console.ReadKey();
                    goto UserMenu;
                }
            case "0":
                return;
            case "2":
                try
                {
                    Console.WriteLine("Enter Email");
                    string loginMail = Console.ReadLine();
                    Console.WriteLine("Enter Password");
                    string loginPassword = Console.ReadLine();
                    UserLoginDto userLoginDto = new UserLoginDto()
                    {
                        Email = loginMail,
                        Password = loginPassword
                    };
                    await usersService.LoginAsync(userLoginDto);
                    foreach(var item in await usersService.GetAllUsersAsync())
                    {
                        if (item.Email == loginMail)
                            loginnedUser.Id = item.Id;
                    }
                }
                catch (UserAuthenticationException e)
                {

                    Console.WriteLine(e.Message);
                    Console.WriteLine("press enter");
                    Console.ReadKey();
                    goto UserMenu;
                }
                break;
            default:
                Console.WriteLine("wrong input");
                Console.WriteLine("press enter");
                Console.ReadKey();
                goto UserMenu;
        }
    ServicesMenu:

        Console.WriteLine("-------------------------------------------------------------");
        Console.WriteLine("1.Product Service");
        Console.WriteLine("2.Order Service");
        Console.WriteLine("3.Payment Service");
        Console.WriteLine("0.Logout Account");

        string serviceCommand = Console.ReadLine();
        Console.WriteLine("choose one");
        switch (serviceCommand) { 
        default:
                Console.WriteLine("wrong input");
                Console.WriteLine("press enter");
                Console.ReadKey();
                goto ServicesMenu;
            case "0":
                goto UserMenu;
            case "1":
                ProductMenu:
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Welcome to the Product Service");
                Console.WriteLine("1.Add Product");
                Console.WriteLine("2.Update Product");
                Console.WriteLine("3.Get all products");
                Console.WriteLine("4.Get Product By Id");
                Console.WriteLine("5.Delete Product");
                Console.WriteLine("6.Search Products By Name");
                Console.WriteLine("7.Export orders to excel");
                Console.WriteLine("0.Exit Porduct Service");
                Console.WriteLine("choose one");
                string productServiceOption = Console.ReadLine();
                switch (productServiceOption)
                {
                    case "0":
                        goto ServicesMenu;
                    default:
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto ProductMenu;
                    case "1":
                        try
                        {
                            Console.Write("Enter the product name:");
                            string productName = Console.ReadLine();
                            Console.Write("Enter the product price:");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal productPrice) || productPrice <= 0) { Console.WriteLine("Invalid Price"); goto ProductMenu; }
                            Console.Write("Enter the product stock:");
                            if (!int.TryParse(Console.ReadLine(), out int productStock) || productStock <= 0) { Console.WriteLine("Invalid stock"); goto ProductMenu; }
                            Console.Write("Enter the description:");
                            string productDescription = Console.ReadLine();
                            ProductPostDto product = new ProductPostDto()
                            {
                                Name = productName,
                                Description = productDescription,
                                Price = productPrice,
                                Stock = productStock
                            };
                            await productService.CreateProductAsync(product);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (InvalidProductException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                    case "2":
                        try
                        {
                            Console.Write("Enter the product ID to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId) || updateId <= 0) { Console.WriteLine("Invalid Id"); goto ProductMenu; }

                            Console.Write("Enter the new product name: ");
                            string newName = Console.ReadLine();

                            Console.Write("Enter the new product price: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice) || newPrice <= 0) { Console.WriteLine("Invalid Price"); goto ProductMenu; }

                            Console.Write("Enter the new product stock: ");
                            if (!int.TryParse(Console.ReadLine(), out int newStock) || newStock <= 0) { Console.WriteLine("Invalid stock"); goto ProductMenu; }

                            Console.Write("Enter the new description: ");
                            string newDescription = Console.ReadLine();

                            await productService.UpdateProductAsync(new ProductPutDto
                            {
                                Name = newName,
                                Price = newPrice,
                                Stock = newStock,
                                Description = newDescription,
                                Id = updateId
                            });

                            Console.WriteLine("Product updated successfully! Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (InvalidProductException e)
                        {
                            Console.WriteLine($"Error: {e.Message}");
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine($"Error: {e.Message}");
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                    case "3":
                        List<ProductGetDto> products = await productService.GetAllProductsAsync();
                        foreach (var item in products)
                        {
                            Console.WriteLine($"Id:{item.Id} Name:{item.Name} Price:{item.Price} " +
                                $"Stock:{item.Stock} Description:{item.Description} Create time: {item.CreatedTime} Update time:{item.UpdatedTime}" );
                        }
                        Console.WriteLine("Press enter to continue.");
                        Console.ReadKey();
                        goto ProductMenu;
                    case "4":
                        try
                        {
                            Console.WriteLine("Enter the Product Id:");
                            if (!int.TryParse(Console.ReadLine(), out int prId) || prId <= 0) { Console.WriteLine("Invalid Id"); goto ProductMenu; }
                            ProductGetDto productGetById = await productService.GetProductAsync(prId);
                            Console.WriteLine($"Id:{productGetById.Id} Name:{productGetById.Name} Price:{productGetById.Price} " +
                                    $"Stock:{productGetById.Stock} Description:{productGetById.Description} Create time: {productGetById.CreatedTime} Update time:{productGetById.UpdatedTime}");
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                    case "5":
                        try
                        {
                            Console.WriteLine("Enter the Product Id:");
                            if (!int.TryParse(Console.ReadLine(), out int deleteId) || deleteId <= 0) { Console.WriteLine("Invalid Id"); goto ProductMenu; }
                            await productService.DeleteProductAsync(deleteId);
                            Console.WriteLine("Product Removed!");
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                    case "6":
                        try
                        {
                            Console.WriteLine("Enter the name which do you search:");
                            string name = Console.ReadLine();
                            var productsSearch = await productService.SearchProductByNameAsync(name);
                            foreach (var item in productsSearch)
                            {
                                Console.WriteLine($"Id:{item.Id} Name:{item.Name} Price:{item.Price} " +
                                    $"Stock:{item.Stock} Description:{item.Description} Create time: {item.CreatedTime} Update time:{item.UpdatedTime}");
                            }
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                        catch (InvalidProductNameException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Press enter to continue.");
                            Console.ReadKey();
                            goto ProductMenu;
                        }
                }
            case "2":
            OrderMenu:
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Welcome to the Order Service");
                Console.WriteLine("1.Create Order");
                Console.WriteLine("2.Cancel Order");
                Console.WriteLine("3.Complete Order");
                Console.WriteLine("4.Get Orders");
                Console.WriteLine("5.Add OrderDetail");
                Console.WriteLine("6.Get OrderDetails");
                Console.WriteLine("0.Exit Order Service");
                Console.WriteLine("choose one");
                string orderOption = Console.ReadLine();
                switch (orderOption)
                {
                    case "0":
                        goto ServicesMenu;
                    default:
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto OrderMenu;
                    case "1":
                        try
                        {
                            OrderPostDto order = new OrderPostDto()
                            {
                                UserId = loginnedUser.Id
                            };                               
                            await ordersService.CreateOrderAsync(order);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (InvalidOrderException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                    case "2":
                        try
                        {
                            Console.WriteLine("enter order id for cancel");
                            if (!int.TryParse(Console.ReadLine(), out int cancelId) || cancelId <= 0) { Console.WriteLine("Invalid Id"); goto OrderMenu; }
                            await ordersService.CancelOrderAsync(cancelId,loginnedUser.Id);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (OrderAlreadyCancelledException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                    case "3":
                        try
                        {
                            Console.WriteLine("Enter Order Id for Complete:");
                            if (!int.TryParse(Console.ReadLine(), out int completeId) || completeId <= 0) { Console.WriteLine("Invalid Id"); goto OrderMenu; }
                            await ordersService.CompleteOrderAsync(completeId, loginnedUser.Id);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (OrderAlreadyCompletedException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                    case "4":
                        var orders = await ordersService.GetAllOrdersAsync();
                        foreach (var order in orders)
                        {
                            if(order.UserId==loginnedUser.Id)
                            Console.WriteLine($"Order id:{order.Id}   Total Amount:{order.TotalAmount} Status:{order.Status} Order date:{order.OrderDate} user id: {order.UserId}");
                        }
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto OrderMenu;
                    case "5":
                        try
                        {
                            Console.WriteLine("Enter the order Id:");
                            if (!int.TryParse(Console.ReadLine(), out int odOrderId) || odOrderId <= 0) { Console.WriteLine("Invalid Id"); goto OrderMenu; }
                            Console.WriteLine("Enter the product Id:");
                            if (!int.TryParse(Console.ReadLine(), out int odProductId) || odProductId <= 0) { Console.WriteLine("Invalid Id"); goto OrderMenu; }
                            Console.WriteLine("Enter the quantity");
                            if (!int.TryParse(Console.ReadLine(), out int odQuantity) || odQuantity <= 0) { Console.WriteLine("Invalid Id"); goto OrderMenu; }

                            OrderDetailsPostDto Od = new OrderDetailsPostDto()
                            {
                                OrderId = odOrderId,
                                ProductId = odProductId,
                                Quantity = odQuantity
                            };

                            await ordersService.AddOrderDetailAsync(Od);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (InvalidOrderDetailException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto OrderMenu;
                        }
                    case "6":
                        var ordersForDetails = await ordersService.GetAllOrdersAsync();
                        foreach (var order in ordersForDetails)
                        {
                            if (order.UserId == loginnedUser.Id)
                                foreach(var item in order.OrderDetails)
                                {
                                    Console.WriteLine($"OrderDetail id:{item.Id}  orderid:{item.OrderId} quantity:{item.Quantity} Price per item:{item.PricePerItem} product id: {item.ProductId}");

                                }
                        }
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto OrderMenu;
                    case "7":
                        string filePath = @"C:\Users\Ibrahim\Desktop\UserOrders\UserOrders.xlsx"; 
                        bool success =await exportService.ExportUserOrdersToExcel(loginnedUser.Id, filePath);

                        if (success)
                        {
                            Console.WriteLine("Excel file created successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to create Excel file.");
                        }
                        goto OrderMenu;


                }
            case "3":
                PaymentMenu:
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Welcome to the Payment Service");
                Console.WriteLine("1.Make Payment");
                Console.WriteLine("2.Get All Payments");
                Console.WriteLine("0.Exit Order Service");
                Console.WriteLine("choose one");
                string paymentOption = Console.ReadLine();
                switch (paymentOption)
                {
                    case "0":
                        goto ServicesMenu;
                    default:
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto PaymentMenu;
                    case "1":
                        try
                        {
                            Console.WriteLine("Enter the Order Id:");
                            if (!int.TryParse(Console.ReadLine(), out int paymentOrderID) || paymentOrderID <= 0) { Console.WriteLine("Invalid Id"); goto PaymentMenu; }
                            //Console.WriteLine("Enter the Total Amount:");
                            //int paymentAmount = int.Parse(Console.ReadLine());
                            PaymentPostDto paymentPostDto = new PaymentPostDto()
                            {
                                OrderId = paymentOrderID,
                            };
                            await paymentService.MakePaymentAsync(paymentPostDto,loginnedUser.Id);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto PaymentMenu;
                        }
                        catch (NotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto PaymentMenu;
                        }
                        catch(InvalidPaymentException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("press enter");
                            Console.ReadKey();
                            goto PaymentMenu;
                        }
                    case"2":
                        var payments = await paymentService.GetAllPaymentsAsync(loginnedUser.Id);
                        foreach (var p in payments)
                        {
                            Console.WriteLine($"Id:{p.Id} Date:{p.PaymentDate} Payment Amouunt{p.Amount} Order Id:{p.OrderId} ");
                        }
                        Console.WriteLine("press enter");
                        Console.ReadKey();
                        goto PaymentMenu;
                }
            

        }
        
    }
    void ValidateInput(string input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new InvalidOrderException($"{fieldName} cannot be empty or whitespace.");
        }
    }

    int ValidateIntegerInput(string input, string fieldName)
    {
        if (!int.TryParse(input, out int value))
        {
            throw new InvalidOrderException($"{fieldName} must be a valid integer.");
        }
        return value;
    }

    decimal ValidateDecimalInput(string input, string fieldName)
    {
        if (!decimal.TryParse(input, out decimal value))
        {
            throw new InvalidOrderException($"{fieldName} must be a valid decimal number.");
        }
        return value;
    }

    DateTime ValidateDateInput(string input, string fieldName)
    {
        if (!DateTime.TryParse(input, out DateTime date))
        {
            throw new InvalidOrderException($"{fieldName} must be a valid date in the format yyyy-MM-dd.");
        }
        return date;
    }
}
