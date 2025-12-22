using Microsoft.AspNetCore.Mvc;
using Orders.Application.UseCases.AddOrder;
using Orders.Application.UseCases.GetOrderStatusById;
using Orders.Application.UseCases.ListOrders;

namespace Orders.API.Endpoints
{
    public static class OrdersEndpoints
    {
        public static WebApplication MapOrdersEndpoints(this WebApplication app)
        {
            app.MapGroup("/")
                .WithTags("Orders")
                .MapGetOrders()
                .MapGetOrderStatusById()
                .MapAddOrder()
                ;

            return app;
        }

        private static RouteGroupBuilder MapGetOrders(this RouteGroupBuilder group)
        {
            group.MapGet("", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    IListOrdersRequestHandler handler) =>
                {
                    ListOrdersResponse response = await handler.HandleAsync(userId);
                    return Results.Ok(response);
                })
                .WithName("GetOrders")
                .WithSummary("Get all orders")
                .WithDescription("Get all orders for current user")
                .WithOpenApi();

            return group;
        }
        
        private static RouteGroupBuilder MapAddOrder(this RouteGroupBuilder group)
        {
            group.MapPost("", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    AddOrderRequest request,
                    IAddOrderRequestHandler handler) =>
                {
                    AddOrderResponse response = await handler.HandleAsync(userId, request);
                    return Results.Created($"/orders/{response.OrderId}", response);
                })
                .WithName("AddOrder")
                .WithSummary("Add a new order")
                .WithDescription("Create a new order for current user and send async message to Payments")
                .WithOpenApi();

            return group;
        }

        
        private static RouteGroupBuilder MapGetOrderStatusById(this RouteGroupBuilder group)
        {
            group.MapGet("{orderId:guid}", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    Guid orderId,
                    IGetOrderStatusByIdRequestHandler handler) =>
                {
                    GetOrderStatusByIdResponse? response =
                        await handler.HandleAsync(orderId, userId);

                    return response is not null
                        ? Results.Ok(response)
                        : Results.NotFound();
                })
                .WithName("GetOrderStatusById")
                .WithSummary("Get Order Status by ID")
                .WithDescription("Get status of the specific order for current user")
                .WithOpenApi();

            return group;
        }
    }
}
