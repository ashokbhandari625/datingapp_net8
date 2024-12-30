using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace API.Inerfaces
{
    public interface IPhotoService
    {
        
                Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

                Task<DeletionResult> DeletePhotoAsyc(string publicId);
                
    }


}