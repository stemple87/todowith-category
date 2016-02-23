using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace CollectorNS
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
      };
      Get["/collectors"] = _ => {
        List<Collector> AllCollectors = Collector.GetAll();
        return View["collectors.cshtml", AllCollectors];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };
      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/collectors/new"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["collectors_form.cshtml", AllCategories];
      };
      Post["/collectors/new"] = _ => {
        Collector newCollector = new Collector(Request.Form["collector-description"], Request.Form["category-id"]);
        newCollector.Save();
        return View["success.cshtml"];
      };
      Post["/collectors/delete"] = _ => {
        Collector.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCategory = Category.Find(parameters.id);
        var CategoryCollectors = SelectedCategory.GetCollectors();
        model.Add("category", SelectedCategory);
        model.Add("collectors", CategoryCollectors);
        return View["category.cshtml", model];
      };

    }
  }
}
