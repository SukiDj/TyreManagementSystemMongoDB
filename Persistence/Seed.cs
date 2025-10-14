using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (!context.Tyres.Any())
            {
                var tyres = new List<Tyre>
                {
                    new Tyre
                    {
                        Code = Guid.NewGuid(),
                        Name = "Continental EcoContact",
                        Type = "Summer",
                        Size = "205/55 R16",
                        Brand = "Continental",
                        Price = 90.5
                    },
                    new Tyre
                    {
                        Code = Guid.NewGuid(),
                        Name = "Michelin Pilot Sport",
                        Type = "All-Season",
                        Size = "225/45 R17",
                        Brand = "Michelin",
                        Price = 120.0
                    },
                    new Tyre
                    {
                        Code = Guid.NewGuid(),
                        Name = "Pirelli Cinturato",
                        Type = "Winter",
                        Size = "195/65 R15",
                        Brand = "Pirelli",
                        Price = 85.0
                    },
                    new Tyre
                    {
                        Code = Guid.NewGuid(),
                        Name = "Goodyear Eagle F1",
                        Type = "Performance",
                        Size = "235/40 R18",
                        Brand = "Goodyear",
                        Price = 150.0
                    },
                    new Tyre
                    {
                        Code = Guid.NewGuid(),
                        Name = "Bridgestone Turanza",
                        Type = "Touring",
                        Size = "215/60 R16",
                        Brand = "Bridgestone",
                        Price = 110.0
                    }
                };

                await context.Tyres.AddRangeAsync(tyres);
            }

            if (!context.Clients.Any())
            {
                var clients = new List<Client>
                {
                    new Client
                    {
                        Id = Guid.NewGuid(),
                        Name = "AutoShop",
                        Contact = "autosales@autoshop.com",
                        Address = "123 Auto Road, New York, NY"
                    },
                    new Client
                    {
                        Id = Guid.NewGuid(),
                        Name = "SpeedTires Inc.",
                        Contact = "info@speedtires.com",
                        Address = "456 Fast Lane, Los Angeles, CA"
                    },
                    new Client
                    {
                        Id = Guid.NewGuid(),
                        Name = "Wheels & Co.",
                        Contact = "contact@wheelsco.com",
                        Address = "789 Wheel St, Houston, TX"
                    }
                };

                await context.Clients.AddRangeAsync(clients);
            }

            if (!context.Machines.Any())
            {
                var machines = new List<Machine>
                {
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Banbury Mixer",
                        Type = "Mixing"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Calendering Machine",
                        Type = "Forming"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tire Building Machine (TBM)",
                        Type = "Assembly"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Curing Press",
                        Type = "Vulcanization"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Extruder Machine",
                        Type = "Extrusion"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tread Cutting Machine",
                        Type = "Cutting"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tire Balancing Machine",
                        Type = "Balancing"
                    },
                    new Machine
                    {
                        Id = Guid.NewGuid(),
                        Name = "Rubber Strainer Machine",
                        Type = "Filtering"
                    }
                };

                await context.Machines.AddRangeAsync(machines);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var korisnici = new List<User> {
                    new ProductionOperator{Ime = "Vukan", Prezime = "Taskov", UserName = "tasko_", Email = "taskov.vukan@elfak.rs", LockoutEnabled = false, Telefon = "063492989", DatumRodjenja = new DateTime(2002, 7, 11)},
                    new BusinessUnitLeader{Ime = "Dimitrije", Prezime = "Najdanovic", UserName = "dika", Email = "dikadika@elfak.rs", LockoutEnabled = false, Telefon = "064492989", DatumRodjenja = new DateTime(2002, 11, 7)},
                    new QualitySupervisor{Ime = "Aleksandar", Prezime = "Djordjevic", UserName = "suki", Email = "sukisuki@elfak.rs", LockoutEnabled = false, Telefon = "065492989", DatumRodjenja = new DateTime(2002, 4, 26)}
                };

                var roles = new List<AppRole> {
                    new AppRole{Name = "ProductionOperator"},
                    new AppRole{Name = "BusinessUnitLeader"},
                    new AppRole{Name = "QualitySupervisor"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);  // Kreiraj uloge u bazi
                }

                foreach (var korisnik in korisnici)
                {
                    await userManager.CreateAsync(korisnik, "PrejaK@s1fra");  // Kreiraj korisnike sa šifrom u bazi
                    if (korisnik.Ime == "Vukan")
                        await userManager.AddToRoleAsync(korisnik, "ProductionOperator");
                    if (korisnik.Ime == "Dimitrije")
                        await userManager.AddToRoleAsync(korisnik, "BusinessUnitLeader");
                    if (korisnik.Ime == "Aleksandar")
                        await userManager.AddToRoleAsync(korisnik, "QualitySupervisor");
                }
            }
        }
    }
}
