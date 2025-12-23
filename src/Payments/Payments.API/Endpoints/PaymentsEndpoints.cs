using Microsoft.AspNetCore.Mvc;
using Payments.Application.UseCases.AddBankAccount;
using Payments.Application.UseCases.CreditBankAccount;
using Payments.Application.UseCases.GetBalanceByUserId;

namespace Payments.API.Endpoints
{
    public static class PaymentsEndpoints
    {
        public static WebApplication MapPaymentsEndpoints(this WebApplication app)
        {
            app.MapGroup("/")
                .WithTags("BankAccounts")
                .MapAddBankAccount()
                .MapGetBankAccountBalanceByUserId()
                .MapCreditBankAccount()
                ;

            return app;
        }

        private static RouteGroupBuilder MapAddBankAccount(this RouteGroupBuilder group)
        {
            group.MapPost("/create", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    CancellationToken ct,
                    IAddBankAccountRequestHandler handler) =>
                {
                    AddBankAccountResponse response = await handler.HandleAsync(userId, ct);
                    return Results.Created("/payments/", response);
                })
                .WithName("AddBankAccount")
                .WithSummary("Add a new Bank Account")
                .WithDescription("Create a new Bank Account for current user")
                .WithOpenApi();

            return group;
        }


        private static RouteGroupBuilder MapGetBankAccountBalanceByUserId(this RouteGroupBuilder group)
        {
            group.MapGet("/balance", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    IGetBankAccountBalanceByUserIdRequestHandler handler) =>
                {
                    GetBankAccountBalanceByUserIdResponse? response =
                        await handler.HandleAsync(userId);

                    return response is not null
                        ? Results.Ok(response)
                        : Results.NotFound();
                })
                .WithName("GetBankAccountBalanceByUserId")
                .WithSummary("Get Bank Account Balance by UserID")
                .WithDescription("Get balance of the Bank Account for current user")
                .WithOpenApi();

            return group;
        }

        private static RouteGroupBuilder MapCreditBankAccount(this RouteGroupBuilder group)
        {
            group.MapPost("topup", async (
                    [FromHeader(Name = "X-User-Id")] Guid userId,
                    CreditBankAccountRequest request,
                    CancellationToken ct,
                    ICreditBankAccountRequestHandler handler) =>
                {
                    CreditBankAccountResponse response = await handler.HandleAsync(userId, request, ct);
                    return Results.Ok(response);
                })
                .WithName("CreditBankAccount")
                .WithSummary("Add amount to Bank Account")
                .WithDescription("Topping up a Bank Account for current user")
                .WithOpenApi();

            return group;
        }
    }
}