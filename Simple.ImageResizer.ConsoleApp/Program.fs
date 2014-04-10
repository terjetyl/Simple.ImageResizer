// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open Simple.ImageResizer

[<EntryPoint>]
let main argv = 
    printfn "%A" argv

    let imageResizer = new ImageResizer(@"c:\test\image1.jpg")
    let resizedImage = imageResizer.Resize(400, ImageEncoding.Jpg)
    imageResizer.SaveToFile(@"c:\test\image1_w400.jpg");
    imageResizer.Dispose()

    let imageResizer2 = new ImageResizer(@"c:\test\image1.jpg")
    let resizedImage = imageResizer2.Resize(400, 400, true, ImageEncoding.Jpg)
    imageResizer2.SaveToFile(@"c:\test\image1_400x400_scaleToFill.jpg");

    let imageResizer3 = new ImageResizer(@"c:\test\image1.jpg")
    let resizedImage = imageResizer3.Resize(1400, 200, false, ImageEncoding.Jpg)
    imageResizer3.SaveToFile(@"c:\test\image1_1400x200_scaleToFit.jpg");

    let imageResizer4 = new ImageResizer(@"c:\test\image1.jpg")
    let resizedImage = imageResizer4.Resize(1400, ImageEncoding.Jpg)
    imageResizer4.SaveToFile(@"c:\test\image1_1400.jpg")

    0 // return an integer exit code
