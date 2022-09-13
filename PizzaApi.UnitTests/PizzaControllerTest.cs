using NUnit.Framework;
using PizzaApi.Domain.Models;
using PizzaApi.Infrastructure.Contexts;
using PizzaApi.Controllers;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace PizzaApi.UnitTests;

public class PizzagControllerTest
{
    private List<Pizza> pizzas;

    private PizzaController _controller;

    [SetUp]
    public void Setup()
    {
        pizzas = new List<Pizza>
        {
            new Pizza
            {
                Id = 0,
                Name = "mushroom1",
                IsVegan = false

            },
            new Pizza
            {
                Id = 0,
                Name = "mushroom2",
                IsVegan = false
            },
            new Pizza
            {
                Id = 0,
                Name = "mushroom3",
                IsVegan = false
            }
        };
    }

    [Test]
    public void TestGet()
    {
        var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase("TestGetPizza").Options;

        var context = new ApiContext(options);

        context.PizzaMenu.AddRange(pizzas);
        context.SaveChanges();

        _controller = Substitute.For<PizzaController>(context);

        var result = _controller.Get(1);
        result.Result.Value.Id.Should().Be(1);
        result.Result.Value.Name.Should().Be("mushroom1");
        result.Result.Value.IsVegan.Should().Be(false);
    }

    [Test]
    public void TestPost()
    {
        var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase("TestPost").Options;
        var context = new ApiContext(options);

        context.PizzaMenu.AddRange(pizzas);
        context.SaveChanges();
        _controller = Substitute.For<PizzaController>(context);

        Pizza pizzaToPost = new Pizza
        {
            Id = 0,
            Name = "mushroom4",
            IsVegan = false
        };

        _controller.Create(pizzaToPost);

        var result = _controller.Get(4);

        Pizza resultPizza = result.Result.Value;
        resultPizza.Should().BeEquivalentTo(pizzaToPost);
    }

    [Test]
    public void TestPut()
    {
        var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase("TestPut").Options;
        var context = new ApiContext(options);

        context.PizzaMenu.AddRange(pizzas);
        context.SaveChanges();

        _controller = Substitute.For<PizzaController>(context);

        Pizza pizzaToPut = new Pizza
        {
            Id = 1,
            Name = "Super Mushroom",
            IsVegan = true
        };

        _controller.Edit(pizzaToPut);

        var result = _controller.Get(1);
        result.Result.Value.Id.Should().Be(1);
        result.Result.Value.Name.Should().Be("Super Mushroom");
        result.Result.Value.IsVegan.Should().Be(true);
    }

    [Test]
    public void TestDelete()
    {
        var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase("TestDelete").Options;
        var context = new ApiContext(options);

        context.PizzaMenu.AddRange(pizzas);
        context.SaveChanges();

        _controller = Substitute.For<PizzaController>(context);

        _controller.Delete(1);

        List<Pizza> shouldBe = new List<Pizza>
        {
            new Pizza
            {
                Id = 2,
                Name = "mushroom2",
                IsVegan = false
            },
            new Pizza
            {
                Id = 3,
                Name = "mushroom3",
                IsVegan = false
            }
        };

        List<Pizza> newList = (List<Pizza>)_controller.GetAll().Result.Value;
        newList.Should().BeEquivalentTo(shouldBe);
    }

    [Test]
    public void TestGetAll()
    {
        var options = new DbContextOptionsBuilder<ApiContext>().UseInMemoryDatabase("TestGetAllHotelBookings").Options;
        var context = new ApiContext(options);

        context.PizzaMenu.AddRange(pizzas);
        context.SaveChanges();

        _controller = Substitute.For<PizzaController>(context);

        List<Pizza> allHotelBookings = (List<Pizza>)_controller.GetAll().Result.Value;
        allHotelBookings.Should().BeEquivalentTo(pizzas);
    }
}