using Nancy;
using CollectorNS.Objects;
using System.Collections.Generic;
using System;

namespace CollectorNS
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml", Collector.GetAll()];
      };
      Get["/{id}"] = parameters => {
        Console.WriteLine("ID: " + parameters.id);
        return View["viewThing.cshtml", Collector.Find(parameters.id)];
      };
      Post["/"] = _ => {
        Console.WriteLine("Form data: " + Request.Form["thing"]);
        Console.WriteLine("Description: " + new Collector(Request.Form["thing"]).GetDescription());
        return View["index.cshtml", Collector.GetAll()];
      };
      Get["/delete"] = _ => {
        return View["sure.cshtml", "/delete"];
      };
      Get["/delete/delete"] = _ => {
        return View["sure.cshtml", "/delete/delete"];
      };
      Get["/delete/delete/delete"] = _ => {
        return View["sure.cshtml", "/delete/delete/delete"];
      };
      Get["/delete/delete/delete/delete"] = _ => {
        Collector.DeleteAll();
        return View["index.cshtml", Collector.GetAll()];
      };
    }
  }
}
