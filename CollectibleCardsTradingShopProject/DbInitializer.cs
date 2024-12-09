using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.Models;

namespace CollectibleCardsTradingShopProject
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "ADMIN" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "user", NormalizedName = "USER" }).Wait();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>();
                for (int i = 0; i < 100; i++)
                {
                    var user = new User
                    {
                        UserName = $"user{i + 1}",
                        Email = $"user{i + 1}@example.com",
                        NormalizedUserName = $"USER{i + 1}",
                        NormalizedEmail = $"USER{i + 1}@EXAMPLE.COM",
                        EmailConfirmed = true,
                        PasswordHash = userManager.PasswordHasher.HashPassword(null, "Password123!")
                    };
                    users.Add(user);
                }
                context.Users.AddRange(users);
                context.SaveChanges();

                var userIds = context.Users.Select(u => u.Id).ToList();
                foreach (var userId in userIds)
                {
                    userManager.AddToRoleAsync(context.Users.Find(userId), "user").Wait();
                }
            }

            if (!context.Set<Franchise>().Any())
            {
                var franchises = new List<Franchise>
            {
                new Franchise { Name = "Pokémon" },
                new Franchise { Name = "Yu-Gi-Oh!" },
                new Franchise { Name = "Magic: The Gathering" },
                new Franchise { Name = "Digimon" },
                new Franchise { Name = "Hearthstone" }
            };
                context.Set<Franchise>().AddRange(franchises);
                context.SaveChanges();
            }

            if (!context.Set<Rarity>().Any())
            {
                var rarities = new List<Rarity>
            {
                new Rarity { Name = "Common" },
                new Rarity { Name = "Uncommon" },
                new Rarity { Name = "Rare" },
                new Rarity { Name = "Ultra Rare" },
                new Rarity { Name = "Legendary" }
            };
                context.Set<Rarity>().AddRange(rarities);
                context.SaveChanges();
            }

            if (!context.Set<LotCardStatus>().Any())
            {
                var statuses = new List<LotCardStatus>
            {
                new LotCardStatus { Status = "Offered" },
                new LotCardStatus { Status = "Wanted" },
                new LotCardStatus { Status = "Required" }
            };
                context.Set<LotCardStatus>().AddRange(statuses);
                context.SaveChanges();
            }

            if (!context.Set<Card>().Any())
            {
                var franchises = context.Set<Franchise>().ToList();
                var rarities = context.Set<Rarity>().ToList();
                var cards = new List<Card>();
                for (int i = 0; i < 2000; i++)
                {
                    var card = new Card
                    {
                        Name = $"Card_{i + 1}",
                        Image = $@"\images\Image_{i + 1}.png",
                        FranchiseId = franchises[new Random().Next(franchises.Count)].Id,
                        RarityId = rarities[new Random().Next(rarities.Count)].Id
                    };
                    cards.Add(card);
                }
                context.Set<Card>().AddRange(cards);
                context.SaveChanges();
            }

            if (!context.Set<Lot>().Any())
            {
                var lots = Enumerable.Range(1, 500).Select(i => new Lot()).ToList();
                context.Set<Lot>().AddRange(lots);
                context.SaveChanges();
            }

            if (!context.Set<CardInLot>().Any())
            {
                var cards = context.Set<Card>().ToList();
                var lots = context.Set<Lot>().ToList();
                var statuses = context.Set<LotCardStatus>().ToList();
                var cardInLots = new List<CardInLot>();
                for (int i = 0; i < 2000; i++)
                {
                    var cardInLot = new CardInLot
                    {
                        CardId = cards[new Random().Next(cards.Count)].Id,
                        LotId = lots[new Random().Next(lots.Count)].Id,
                        LotCardStatusId = statuses[new Random().Next(statuses.Count)].Id
                    };
                    cardInLots.Add(cardInLot);
                }
                context.Set<CardInLot>().AddRange(cardInLots);
                context.SaveChanges();
            }
            if (!context.Set<UserLot>().Any())
            {
                var users = context.Users.ToList();
                var lots = context.Set<Lot>().ToList();
                var userLots = new List<UserLot>();
                var random = new Random();

                foreach (var lot in lots)
                {
                    var openingUser = users[random.Next(users.Count)];
                    userLots.Add(new UserLot
                    {
                        LotId = lot.Id,
                        UserId = openingUser.Id, 
                        DidCloseTheLot = false 
                    });

                    if (random.Next(0, 2) == 1)
                    {
                        var closingUser = users[random.Next(users.Count)];
                        userLots.Add(new UserLot
                        {
                            LotId = lot.Id,
                            UserId = closingUser.Id,
                            DidCloseTheLot = true
                        });
                    }
                }

                context.Set<UserLot>().AddRange(userLots);
                context.SaveChanges();
            }
        }
    }
}
