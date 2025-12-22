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
                // .MapAttachFile()
                // .MapAnalyzeWork()
                // .MapGetReport()
                // .MapGetWordCloud()
                ;

            return app;
        }

        private static RouteGroupBuilder MapGetOrders(this RouteGroupBuilder group)
        {
            group.MapGet("", async (IListOrdersRequestHandler handler) =>
                {
                    ListOrdersResponse response = await handler.HandleAsync();
                    return Results.Ok(response);
                })
                .WithName("GetOrders")
                .WithSummary("Get all orders")
                .WithDescription("Get all orders from the database")
                .WithOpenApi();
            return group;
        }
        
        private static RouteGroupBuilder MapAddOrder(this RouteGroupBuilder group)
        {
            group.MapPost("", async (AddOrderRequest request, IAddOrderRequestHandler handler) =>
                {
                    AddOrderResponse response = await handler.HandleAsync(request);
                    return Results.Created($"/orders/{response.OrderId}", response);
                })
                .WithName("AddOrder")
                .WithSummary("Add a new order")
                .WithDescription("Create a new order and async message to Payments")
                .WithOpenApi();
            return group;
        }
        
        private static RouteGroupBuilder MapGetOrderStatusById(this RouteGroupBuilder group)
        {
            group.MapGet("{orderId:guid}", async (Guid orderId, IGetOrderStatusByIdRequestHandler handler) =>
                {
                    GetOrderStatusByIdResponse? response = await handler.HandleAsync(orderId);
                    return response is not null ? Results.Ok(response) : Results.NotFound();
                })
                .WithName("GetOrderStatusById")
                .WithSummary("Get Order Status by ID")
                .WithDescription("Get status of the specific order")
                .WithOpenApi();
            return group;
        }
        //
        //
        // private static RouteGroupBuilder MapAttachFile(this RouteGroupBuilder group)
        // {
        //     group.MapPatch("{workId:guid}/file", async (Guid workId, [FromForm] AttachFileForm? form, IAttachFileRequestHandler handler) =>
        //         {
        //             if (form?.File == null || form.File.Length == 0)
        //             {
        //                 return Results.BadRequest(new { error = "File is required." });
        //             }
        //             
        //             IFormFile file = form.File;
        //
        //             AttachFileRequest request = new(
        //                 file.OpenReadStream(),
        //                 file.FileName,
        //                 file.ContentType
        //             );
        //             
        //             AttachFileResponse response = await handler.HandleAsync(workId, request);
        //             return Results.Ok(response);
        //         })
        //         .WithName("AttachFile")
        //         .WithSummary("Attach file to work")
        //         .WithDescription("Upload a file and attach it to an existing work")
        //         .WithOpenApi()
        //         .DisableAntiforgery();
        //     return group;
        // }
        //
        // private static RouteGroupBuilder MapAnalyzeWork(this RouteGroupBuilder group)
        // {
        //     group.MapPatch("{workId:guid}/analyze", async (Guid workId, IAnalyzeWorkRequestHandler handler) =>
        //         {
        //             AnalyzeWorkResponse response = await  handler.HandleAsync(workId);
        //             return Results.Ok(response);
        //         })
        //         .WithName("AnalyzeWork")
        //         .WithSummary("Analyze work for plagiarism")
        //         .WithDescription("Trigger analysis for the specified work")
        //         .WithOpenApi();
        //     return group;
        // }
        //
        // private static RouteGroupBuilder MapGetReport(this RouteGroupBuilder group)
        // {
        //     group.MapGet("{workId:guid}/report", async (Guid workId, IGetReportRequestHandler handler) =>
        //         {
        //             GetReportResponse? response = await handler.HandleAsync(workId);
        //             return response is not null ? Results.Ok(response) : Results.NotFound();
        //         })
        //         .WithName("GetReport")
        //         .WithSummary("Get analysis report for work")
        //         .WithDescription("Get the plagiarism report for a specific work")
        //         .WithOpenApi();
        //     return group;
        // }
        //
        // private static RouteGroupBuilder MapGetWordCloud(this RouteGroupBuilder group)
        // {
        //     group.MapGet("{workId:guid}/wordcloud", async (Guid workId, [FromServices] IGetWordCloudRequestHandler handler) =>
        //         {
        //             GetWordCloudResponse response = await handler.HandleAsync(workId);
        //             return Results.File(response.FileStream, response.ContentType, response.FileName);
        //         })
        //         .WithName("GetWordCloud")
        //         .WithSummary("Get Word Cloud")
        //         .WithDescription("Get Word Cloud from external API")
        //         .WithOpenApi()
        //         .DisableAntiforgery();
        //     return group;
        // }
    }
}
