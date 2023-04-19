using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiSample.Interfaces;
using RestApiSample.Middleware;
using RestApiSample.Models;
using RestApiSample.Services;

namespace RestApiSample.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : BaseController
{

    private readonly ILogger<UserController> _logger;
    private readonly ProductService _productService;

    public ProductController(ILogger<UserController> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromForm] ProductDto productDto)
    {

        var email = getJwtPayload("email");

        await _productService.createProduct(email, productDto);

        return Created("", productDto);
    }

    [HttpGet]
    [CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    public IActionResult Get()
    {
        var products = _productService.getProducts();
        return products.GetActionResult();
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public IActionResult GetById(int id)
    {
        var product = _productService.getProduct(id);
        return product.GetActionResult();
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, [FromForm] UpdateProductDto updateProductDto)
    {
        var email = getJwtPayload("email");
        var result = await _productService.updateProduct(id, email, updateProductDto);
        return result.GetActionResult();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> delete(int id)
    {
        var deleteUser = await _productService.deleteProduct(id);
        return deleteUser.GetActionResult();
    }
}
