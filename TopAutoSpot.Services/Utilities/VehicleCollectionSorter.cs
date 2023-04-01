using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Services.Utilities
{
    public static class VehicleCollectionSorter
    {
        public static List<Car> SortBy(List<Car> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }

        public static List<Truck> SortBy(List<Truck> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }

        public static List<Motorcycle> SortBy(List<Motorcycle> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }

        public static List<Trailer> SortBy(List<Trailer> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }

        public static List<Boat> SortBy(List<Boat> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }

        public static List<Bus> SortBy(List<Bus> collection, string orderSetting)
        {
            orderSetting = orderSetting.Trim();

            if (orderSetting == "Oldest")
            {
                return collection
                    .OrderBy(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Newest")
            {
                return collection
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
            else if (orderSetting == "Cheapest")
            {
                return collection
                    .OrderBy(c => c.Price)
                    .ToList();
            }
            else if (orderSetting == "Expensive")
            {
                return collection
                    .OrderByDescending(c => c.Price)
                    .ToList();
            }
            else
            {
                return collection;
            }
        }
    }
}
