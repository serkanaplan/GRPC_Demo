using System.Collections.Concurrent;
using GRPC_Server.Models;
using Grpc.Core;

namespace GRPC_Server.Services;

public class ProductService : Product.ProductBase
{
    private static ConcurrentDictionary<int, ProductModel> _products = new();
    private static int _nextId = 1;


    public override Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        if (_products.TryGetValue(request.Id, out var product))
            return Task.FromResult(new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.Id} is not found."));
    }


    public override Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var product = new ProductModel
        {
            Id = Interlocked.Increment(ref _nextId),
            Name = request.Name,
            Price = request.Price
        };

        if (_products.TryAdd(product.Id, product))
            return Task.FromResult(new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        throw new RpcException(new Status(StatusCode.Internal, "Failed to create product."));
    }


    public override Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        if (_products.TryGetValue(request.Id, out var existingProduct))
        {
            var updatedProduct = new ProductModel
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price
            };

            if (_products.TryUpdate(request.Id, updatedProduct, existingProduct))
                return Task.FromResult(new ProductResponse
                {
                    Id = updatedProduct.Id,
                    Name = updatedProduct.Name,
                    Price = updatedProduct.Price
                });
        }
        throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.Id} is not found."));
    }


    public override Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        if (_products.TryRemove(request.Id, out _)) return Task.FromResult(new DeleteProductResponse { Success = true });
        return Task.FromResult(new DeleteProductResponse { Success = false });
    }


    public override Task<ListProductsResponse> ListProducts(ListProductsRequest request, ServerCallContext context)
    {
        var response = new ListProductsResponse();
        response.Products.AddRange(_products.Values.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }));
        return Task.FromResult(response);
    }
}