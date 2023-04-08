namespace TopAutoSpot.Views.AdministratorViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// A page model for managing listings of vehicles, including boats, buses, cars, motorcycles, trailers, and trucks.
    /// </summary>
    [Authorize]
    public class ManageListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        /// <summary>
        /// A list of boats in the database.
        /// </summary>
        public List<Boat> Boats { get; private set; } = null!;

        /// <summary>
        /// A list of buses in the database.
        /// </summary>
        public List<Bus> Buses { get; private set; } = null!;

        /// <summary>
        /// A list of cars in the database.
        /// </summary>
        public List<Car> Cars { get; private set; } = null!;

        /// <summary>
        /// A list of motorcycles in the database.
        /// </summary>
        public List<Motorcycle> Motorcycles { get; private set; } = null!;

        /// <summary>
        /// A list of trailers in the database.
        /// </summary>
        public List<Trailer> Trailers { get; private set; } = null!;

        /// <summary>
        /// A list of trucks in the database.
        /// </summary>
        public List<Truck> Trucks { get; private set; } = null!;

        /// <summary>
        /// The overall count of all vehicles in the database.
        /// </summary>
        public int OverallCount { get; private set; }

        /// <summary>
        /// HTTP GET method for displaying the listings.
        /// </summary>
        /// <returns>The page to be displayed.</returns>
        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Boats = _context.Boats
                    .AsNoTracking()
                    .ToList();

                Buses = _context.Buses
                    .AsNoTracking()
                    .ToList();

                Cars = _context.Cars
                    .AsNoTracking()
                    .ToList();

                Motorcycles = _context.Motorcycles
                    .AsNoTracking()
                    .ToList();

                Trailers = _context.Trailers
                    .AsNoTracking()
                    .ToList();

                Trucks = _context.Trucks
                    .AsNoTracking()
                    .ToList();

                OverallCount = Boats.Count + Buses.Count + Cars.Count +
                        Motorcycles.Count + Trailers.Count + Trucks.Count;

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Gets the image data URL for a given vehicle.
        /// </summary>
        /// <param name="carId">The ID of the vehicle.</param>
        /// <returns>The image data URL for the vehicle.</returns>
        public string GetImage(string carId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines if a given vehicle has any images associated with it.
        /// </summary>
        /// <param name="carId">The ID of the vehicle.</param>
        /// <returns>True if the vehicle has at least one associated image, false otherwise.</returns>
        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
