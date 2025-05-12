namespace DealerAdmin.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public int CountBeds { get; set; }
        public int CountBaths { get; set; }
        public int Space { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public byte[] Image { get; set; }

    } 
    public class Propertys
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public int CountBeds { get; set; }
        public int CountBaths { get; set; }
        public int Space { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public byte[] Image { get; set; }
        public IFormFile mainImage { get; set; }
        public List<IFormFile> images { get; set; }

    }
    public class PropertyDto
    {
        public Property Property { get; set; }
        public List<byte[]> Images { get; set; } = new List<byte[]>();
    }


    public class PropertyRequest
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Area { get; set; }
        public string? Location { get; set; }
        public int? Space { get; set; }

        // Land-specific
        public string? LandType { get; set; }

        // House-specific
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? FloorNumber { get; set; }

        // Villa-specific
        public int? NumberOfFloors { get; set; }
        public decimal? Price { get; set; }

        public bool? TakePicture { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public List<PropertyRequestImage>? Images { get; set; } = new();

    }

    public class PropertyRequestImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int PropertyRequestId { get; set; }
        public PropertyRequest? PropertyRequest { get; set; } = null!;

    }
}
