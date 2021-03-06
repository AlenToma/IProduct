﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IProduct.Modules
{
    public enum Status { Hidden, Active }

    public enum ObjectStatus { Added, Removed }

    public enum Roles { Administrator, Customers }

    public enum PageType { Product, Content }

    public enum ProductShow { Slider, Table, ImageGrid }

    public enum InvoiceState { Pending, Handled }

    public enum SignInApplication { Cookie, Google, Facebook }

    public enum LogLevel { Low, All }

    public enum ImageFileType
    {
        Undefined, // Unknown, None, etc. whatever you like
        Jpeg,
        Jpg = Jpeg,
        Png,
        MemoryBmp,
        Bmp,
        Emf,
        Wmf,
        Gif,
        Tiff,
        Exif,
        Icon,
        TiF

    }


}
