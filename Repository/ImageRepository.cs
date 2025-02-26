using TaskIcosoftBackend.Models;
using Microsoft.EntityFrameworkCore;
using TaskIcosoftBackend.Data;


namespace TaskIcosoftBackend.Repository
{
    public class ImageRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(DataContext context, ILogger<ImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> InsertImageAsync(string base64Image)
        {
            try
            {
                
                if (string.IsNullOrEmpty(base64Image))
                {
                    _logger.LogError("La imagen en base64 no puede estar vacía.");
                    return -1; 
                }

                var image = new Image
                {
                    Base64Image = base64Image, 
                    IdImageType = 1,  
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                return image.IdImage; 
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al insertar la imagen: {Message}", ex.Message);
                return -1;
            }
        }

        public async Task<Image?> GetImageByIdAsync(int imageId)
        {
            return await _context.Images
                .Include(i => i.ImageType)
                .FirstOrDefaultAsync(i => i.IdImage == imageId);
        }
    }
}
