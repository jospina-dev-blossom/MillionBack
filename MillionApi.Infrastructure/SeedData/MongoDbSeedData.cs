using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MillionApi.Domain.AggregateModel;
using MillionApi.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MillionApi.Infrastructure.SeedData
{
    public static class MongoDbSeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
    var context = serviceProvider.GetRequiredService<MongoContext>();

// Verificar si ya existen datos
            var existingProperties = await context.Properties.CountDocumentsAsync(FilterDefinition<Property>.Empty);
     if (existingProperties > 0)
    {
       return; // Ya hay datos, no hacer seed
  }

            // Crear Owners de ejemplo
            var owners = new List<Owner>
{
              new Owner
            {
          IdOwner = ObjectId.GenerateNewId().ToString(),
 Name = "Juan Pérez",
                    Address = "Calle Principal 123, Ciudad",
 Photo = "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=400",
         Birthday = new DateTime(1980, 5, 15)
  },
        new Owner
    {
         IdOwner = ObjectId.GenerateNewId().ToString(),
       Name = "María García",
                 Address = "Avenida Central 456, Ciudad",
  Photo = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=400",
        Birthday = new DateTime(1985, 8, 22)
 },
                new Owner
       {
      IdOwner = ObjectId.GenerateNewId().ToString(),
        Name = "Carlos Rodríguez",
       Address = "Boulevard Norte 789, Ciudad",
        Photo = "https://images.unsplash.com/photo-1556157382-97eda2d62296?w=400",
    Birthday = new DateTime(1975, 3, 10)
  },
          new Owner
     {
     IdOwner = ObjectId.GenerateNewId().ToString(),
Name = "Ana Martínez",
        Address = "Calle del Sol 234, Ciudad",
          Photo = "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=400",
        Birthday = new DateTime(1990, 11, 5)
             },
    new Owner
     {
  IdOwner = ObjectId.GenerateNewId().ToString(),
      Name = "Luis Fernández",
              Address = "Avenida Libertad 567, Ciudad",
      Photo = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=400",
           Birthday = new DateTime(1982, 7, 18)
  }
            };

            await context.Owners.InsertManyAsync(owners);

// Crear 20 Properties con imágenes reales de Unsplash
        var properties = new List<Property>
{
        new Property
             {
       IdOwner = owners[0].IdOwner,
        Name = "Casa Moderna en el Centro",
         AddressProperty = "Calle Principal 789, Centro",
              PriceProperty = 250000m,
         ImageUrl = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800",
   CodeInternal = 1001,
         Year = 2020
                },
  new Property
          {
     IdOwner = owners[1].IdOwner,
               Name = "Apartamento con Vista al Mar",
          AddressProperty = "Avenida Costera 321, Playa",
   PriceProperty = 180000m,
    ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800",
    CodeInternal = 1002,
     Year = 2019
},
        new Property
{
       IdOwner = owners[2].IdOwner,
      Name = "Villa de Lujo con Piscina",
       AddressProperty = "Colinas del Este 100, Residencial",
    PriceProperty = 450000m,
              ImageUrl = "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
           CodeInternal = 1003,
     Year = 2021
           },
 new Property
           {
         IdOwner = owners[0].IdOwner,
      Name = "Loft Industrial Renovado",
     AddressProperty = "Calle Industria 45, Zona Urbana",
       PriceProperty = 195000m,
 ImageUrl = "https://images.unsplash.com/photo-1502672260066-6bc35f0af07e?w=800",
    CodeInternal = 1004,
  Year = 2018
                },
    new Property
      {
        IdOwner = owners[3].IdOwner,
      Name = "Casa de Campo con Jardín",
         AddressProperty = "Camino Rural 200, Afueras",
        PriceProperty = 320000m,
               ImageUrl = "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800",
      CodeInternal = 1005,
 Year = 2017
  },
   new Property
        {
          IdOwner = owners[1].IdOwner,
            Name = "Penthouse de Diseño",
     AddressProperty = "Torre Central Piso 20, Centro",
      PriceProperty = 550000m,
         ImageUrl = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800",
      CodeInternal = 1006,
         Year = 2022
         },
        new Property
        {
   IdOwner = owners[4].IdOwner,
         Name = "Duplex Familiar Amplio",
               AddressProperty = "Residencial Las Flores 78, Suburbio",
  PriceProperty = 285000m,
    ImageUrl = "https://images.unsplash.com/photo-1572120360610-d971b9d7767c?w=800",
        CodeInternal = 1007,
          Year = 2019
},
       new Property
     {
      IdOwner = owners[2].IdOwner,
         Name = "Estudio Minimalista",
       AddressProperty = "Avenida Moderna 15, Centro",
        PriceProperty = 125000m,
       ImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800",
               CodeInternal = 1008,
   Year = 2020
           },
    new Property
    {
        IdOwner = owners[3].IdOwner,
      Name = "Casa Colonial Restaurada",
        AddressProperty = "Calle Histórica 300, Casco Antiguo",
         PriceProperty = 380000m,
     ImageUrl = "https://images.unsplash.com/photo-1605276374104-dee2a0ed3cd6?w=800",
             CodeInternal = 1009,
          Year = 2016
       },
     new Property
         {
                 IdOwner = owners[0].IdOwner,
          Name = "Apartamento Ejecutivo",
       AddressProperty = "Edificio Business Center 12B, Zona Financiera",
        PriceProperty = 210000m,
      ImageUrl = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800",
  CodeInternal = 1010,
               Year = 2021
      },
         new Property
        {
  IdOwner = owners[4].IdOwner,
     Name = "Casa de Playa Frente al Mar",
           AddressProperty = "Paseo Marítimo 500, Costa",
  PriceProperty = 620000m,
   ImageUrl = "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2?w=800",
     CodeInternal = 1011,
         Year = 2020
       },
    new Property
          {
  IdOwner = owners[1].IdOwner,
     Name = "Townhouse Contemporáneo",
     AddressProperty = "Complejo Urbano 88, Nueva Ciudad",
    PriceProperty = 275000m,
       ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=800",
   CodeInternal = 1012,
      Year = 2021
                },
       new Property
   {
       IdOwner = owners[2].IdOwner,
      Name = "Chalet de Montaña",
   AddressProperty = "Sierra Alta 25, Montaña",
       PriceProperty = 410000m,
       ImageUrl = "https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800",
     CodeInternal = 1013,
      Year = 2018
        },
             new Property
                {
          IdOwner = owners[3].IdOwner,
  Name = "Apartamento con Terraza",
            AddressProperty = "Residencial Primavera 45, Norte",
    PriceProperty = 165000m,
     ImageUrl = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
          CodeInternal = 1014,
               Year = 2019
         },
 new Property
     {
       IdOwner = owners[0].IdOwner,
  Name = "Casa Ecológica Sustentable",
         AddressProperty = "Eco Village 10, Zona Verde",
      PriceProperty = 340000m,
   ImageUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
           CodeInternal = 1015,
            Year = 2022
     },
       new Property
                {
            IdOwner = owners[4].IdOwner,
       Name = "Loft Artístico en SoHo",
   AddressProperty = "Barrio Artístico 67, SoHo",
       PriceProperty = 230000m,
          ImageUrl = "https://images.unsplash.com/photo-1600607687644-c7171b42498f?w=800",
        CodeInternal = 1016,
    Year = 2019
            },
      new Property
 {
    IdOwner = owners[1].IdOwner,
              Name = "Mansión con Jardines",
     AddressProperty = "Avenida Exclusiva 1, Zona Premium",
         PriceProperty = 890000m,
        ImageUrl = "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
             CodeInternal = 1017,
         Year = 2021
    },
   new Property
 {
    IdOwner = owners[2].IdOwner,
       Name = "Apartamento Tipo Loft",
AddressProperty = "Edificio Alto 5C, Centro Histórico",
         PriceProperty = 175000m,
    ImageUrl = "https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=800",
         CodeInternal = 1018,
       Year = 2020
                },
     new Property
     {
  IdOwner = owners[3].IdOwner,
       Name = "Casa Mediterránea con Patio",
     AddressProperty = "Calle del Mar 150, Costa Sur",
    PriceProperty = 395000m,
       ImageUrl = "https://images.unsplash.com/photo-1592595896551-12b371d546d5?w=800",
     CodeInternal = 1019,
            Year = 2018
           },
      new Property
  {
               IdOwner = owners[4].IdOwner,
  Name = "Condominio de Lujo",
     AddressProperty = "Torres del Parque 30A, Zona Alta",
        PriceProperty = 520000m,
                ImageUrl = "https://images.unsplash.com/photo-1567496898669-ee935f5f647a?w=800",
        CodeInternal = 1020,
         Year = 2022
       }
            };

  await context.Properties.InsertManyAsync(properties);

  // Crear PropertyImages de ejemplo para algunas propiedades
       var images = new List<PropertyImage>();
         
  for (int i = 0; i < Math.Min(10, properties.Count); i++)
            {
                images.Add(new PropertyImage
    {
         IdPropertyImage = ObjectId.GenerateNewId().ToString(),
      IdProperty = properties[i].Id,
    File = $"https://images.unsplash.com/photo-{1600596542815 + i}-ffad4c1539a9?w=800",
   Enabled = true
    });
     
      images.Add(new PropertyImage
           {
  IdPropertyImage = ObjectId.GenerateNewId().ToString(),
             IdProperty = properties[i].Id,
      File = $"https://images.unsplash.com/photo-{1512917774080 + i}-9991f1c4c750?w=800",
      Enabled = true
                });
   }

            await context.PropertyImages.InsertManyAsync(images);

      // Crear PropertyTraces de ejemplo
          var traces = new List<PropertyTrace>();
        
            for (int i = 0; i < properties.Count; i++)
 {
       traces.Add(new PropertyTrace
      {
     IdPropertyTrace = ObjectId.GenerateNewId().ToString(),
            IdProperty = properties[i].Id,
           DateSale = DateTime.Now.AddMonths(-12 + i),
      Name = "Venta Inicial",
               Value = properties[i].PriceProperty,
        Tax = properties[i].PriceProperty * 0.10m
                });
  }

            await context.PropertyTraces.InsertManyAsync(traces);
     }
    }
}
