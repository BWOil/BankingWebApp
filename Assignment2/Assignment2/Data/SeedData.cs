﻿using System;
using McbaExample.Models;
using Newtonsoft.Json;

namespace Assignment2.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            
            try
            {
                var context = serviceProvider.GetRequiredService<McbaContext>();

                // Look for customers.

                if (context.Customers.Any())
                    return; // DB has already been seeded.

                // load json
                var customers = LoadJSON();

                // Insert into database.
                foreach (var customer in customers)
                {

                    foreach (var account in customer.Accounts)
                    {
                        var transactions = account.Transactions;
                        foreach (var transaction in transactions)
                        {
                            account.Balance += transaction.Amount;
                            transaction.TransactionType = TransactionType.Deposit;
                        }
                    }

                    context.Customers.Add(customer);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }
            

        }

        private static List<Customer> LoadJSON()
        {

            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

            // Contact webservice.
            using var client = new HttpClient();
            var json = client.GetStringAsync(Url).Result;

            // Convert JSON into objects.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            });
            return customers;
        }


    }

    
}

