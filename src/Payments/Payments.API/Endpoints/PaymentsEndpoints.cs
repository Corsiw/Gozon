using Microsoft.AspNetCore.Mvc;
using Payments.Application.UseCases.AddBankAccount;

namespace Payments.API.Endpoints
{
    public static class PaymentsEndpoints
    {
        public static WebApplication MapPaymentsEndpoints(this WebApplication app)
        {
            app.MapGroup("/")
                .WithTags("BankAccounts")
                .MapAddOrder()
                // .MapGetOrders()
                // .MapGetOrderStatusById()
                ;

            return app;
        }

        // private static RouteGroupBuilder MapGetOrders(this RouteGroupBuilder group)
        // {
        //     group.MapGet("", async (
        //             [FromHeader(Name = "X-User-Id")] Guid userId,
        //             IListOrdersRequestHandler handler) =>
        //         {
        //             ListOrdersResponse response = await handler.HandleAsync(userId);
        //             return Results.Ok(response);
        //         })
        //         .WithName("GetOrders")
        //         .WithSummary("Get all orders")
        //         .WithDescription("Get all orders for current user")
        //         .WithOpenApi();
        //
        //     return group;
        // }
        
        private static RouteGroupBuilder MapAddOrder(this RouteGroupBuilder group)
        {
            group.MapPost("", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    IAddBankAccountRequestHandler handler) =>
                {
                    AddBankAccountResponse response = await handler.HandleAsync(userId);
                    return Results.Created($"/bankAccounts/{response.UserId}", response);
                })
                .WithName("AddBankAccount")
                .WithSummary("Add a new Bank Account")
                .WithDescription("Create a new Bank Account for current user")
                .WithOpenApi();

            return group;
        }

        
        // private static RouteGroupBuilder MapGetOrderStatusById(this RouteGroupBuilder group)
        // {
        //     group.MapGet("{orderId:guid}", async (
        //             [FromHeader(Name = "X-User-Id")] Guid userId,
        //             Guid orderId,
        //             IGetOrderStatusByIdRequestHandler handler) =>
        //         {
        //             GetOrderStatusByIdResponse? response =
        //                 await handler.HandleAsync(orderId, userId);
        //
        //             return response is not null
        //                 ? Results.Ok(response)
        //                 : Results.NotFound();
        //         })
        //         .WithName("GetOrderStatusById")
        //         .WithSummary("Get Order Status by ID")
        //         .WithDescription("Get status of the specific order for current user")
        //         .WithOpenApi();
        //
        //     return group;
        // }
    }
}
