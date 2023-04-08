namespace TopAutoSpot.Views.AdministratorViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// PageModel class for managing vehicle listings approval, accessible to administrators only.
    /// </summary>
    [Authorize]
    public class ListingsApprovalModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListingsApprovalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OverallCount { get; private set; }

        public List<Car> Cars { get; set; } = null!;

        public List<Motorcycle> Motorcycles { get; set; } = null!;

        public List<Truck> Trucks { get; set; } = null!;

        public List<Trailer> Trailers { get; set; } = null!;

        public List<Boat> Boats { get; set; } = null!;

        public List<Bus> Buses { get; set; } = null!;

        /// <summary>
        /// Handles HTTP GET requests to display listings that are waiting approval or closed.
        /// </summary>
        /// <returns>The PageResult object.</returns>
        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                InitializeVehicles();
                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        private void InitializeVehicles()
        {
            Boats = _context.Boats
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Buses = _context.Buses
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Cars = _context.Cars
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Trucks = _context.Trucks
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                        Motorcycles.Count + Trailers.Count + Trucks.Count;
        }

        /// <summary>
        /// Gets the image data URL for a vehicle with the specified ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>The image data URL as a string.</returns>
        public string GetImage(string vehicleId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == vehicleId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines whether a vehicle with the specified ID has any images.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle.</param>
        /// <returns>True if the vehicle has at least one image; otherwise, false.</returns>
        public bool HasAnyImages(string vehicleId)
        {
            return _context.VehicleImages
                .Where(img => img.VehicleId == vehicleId)
                .ToList().Count > 0;
        }
    }
}
