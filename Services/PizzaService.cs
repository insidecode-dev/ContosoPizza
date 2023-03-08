using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
namespace ContosoPizza.Services;

public class PizzaService
{
    private PizzaContext _dataContext;
    public PizzaService(PizzaContext pizza)
    {
        _dataContext = pizza;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _dataContext.Pizzas
                .Include(p=>p.Toppings)
                .Include(p=>p.Sauce)
                .AsNoTracking()
                .ToList();
    }

    public Pizza? GetById(int id)
    {
        return _dataContext.Pizzas
               .Include(p=>p.Toppings)
               .Include(p=>p.Sauce)
               .AsNoTracking()
               .SingleOrDefault(p=>p.Id==id);
                    
    }

    public Pizza Create(Pizza newPizza)
    {
        _dataContext.Pizzas.Add(newPizza);
        _dataContext.SaveChanges();
        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizzaToUpdate = _dataContext.Pizzas.Find(PizzaId);
    var toppingToAdd = _dataContext.Toppings.Find(ToppingId);

    if (pizzaToUpdate is null || toppingToAdd is null)
    {
        throw new InvalidOperationException("Pizza or topping does not exist");
    }

    if(pizzaToUpdate.Toppings is null)
    {
        pizzaToUpdate.Toppings = new List<Topping>();
    }

    pizzaToUpdate.Toppings.Add(toppingToAdd);

    _dataContext.SaveChanges();
    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizza = _dataContext.Pizzas.FirstOrDefault(pz=>pz.Id==PizzaId);
        var sauce = _dataContext.Sauces.FirstOrDefault(sc=>sc.Id==SauceId);
        if(pizza is null || sauce is null)  throw new InvalidOperationException("Pizza or Sauce does not exist...");

        pizza.Sauce = sauce;
        _dataContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _dataContext.Pizzas.Find(id);
    if (pizzaToDelete is not null)
    {
        _dataContext.Pizzas.Remove(pizzaToDelete);
        _dataContext.SaveChanges();
    }
    }
}