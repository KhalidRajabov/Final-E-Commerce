﻿using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Helper
{
    public class Helper
    {

        
        public static void DeleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            };
        }
        
    }
}
