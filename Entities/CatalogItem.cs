using System;
using Play.Common;

namespace Play.Inventory.Service.Entities
{
  public class CatalogItem : IEntity
  {
    public Guid Id { get; set; }

    public String Name { get; set; }

    public String Description { get; set; }
  }
}