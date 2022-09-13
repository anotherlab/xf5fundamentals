# Powershell Script to create iOS & Android images from source image
# srcImage is the image to resize
# despath is the folder to place the images. 
# the sub folders for each density will be added as needed
# basesize is the size to render to in pixels.
#
# Note:
# Loading a bitmap will need the full path to the image, relative paths will fail to load
#
# Sample usage:
# Make-PlatformSizes.ps1 c:\images\mylogo.png c:\images\output 100
#
# This will take mylogo.png and write scaled copies from 100px to folders in c:\images\output

param (
    [Parameter(Mandatory = $true)][string]$srcImage,
    [Parameter(Mandatory = $true)][string]$destPath,
    [Parameter(Mandatory = $true)][double]$baseSize
    )

# load the .NET assembly for working with bitmaps    
Add-Type -Assembly 'System.Drawing'

function ResizePosh()
{
    param (
        [String]$ImagePath,
        [String]$OutputPath,
        [double]$BaseSize,
        [Parameter(Mandatory=$False, ParameterSetName="Absolute")][double]$Density
    )

    # load the image
    $srcImage = [System.Drawing.Bitmap]::FromFile($ImagePath, $False)

    # Figure out the scaling factor
    if ($srcImage.Width -gt $srcImage.Height)
    {
        $xf = $srcImage.Width / $BaseSize
    }
    else {
        $xf = $srcImage.Height / $BaseSize
    }

    # Get the new width/height based on the scaling factor and the density
    $Width = [int]($srcImage.Width * $Density / $xf)
    $Height = [int]($srcImage.Height * $Density / $xf)

    # Create a new bitmap
    $Bitmap = New-Object System.Drawing.Bitmap $Width, $Height

    # Create a new image from the bitmap
    $NewImage = [System.Drawing.Graphics]::FromImage($Bitmap)

    #Retrieving the best quality possible
    $SmoothingMode = "HighQuality"
    $InterpolationMode = "HighQualityBicubic"
    $PixelOffsetMode = "HighQuality"

    $NewImage.SmoothingMode = $SmoothingMode
    $NewImage.InterpolationMode = $InterpolationMode
    $NewImage.PixelOffsetMode = $PixelOffsetMode

    # Draw the source image to the new image and scale it
    $NewImage.DrawImage($srcImage, $(New-Object -TypeName System.Drawing.Rectangle -ArgumentList 0, 0, $Width, $Height))

    # Save the new image
    $Bitmap.Save($OutputPath)

    # Cleanup
    $Bitmap.Dispose()
    $NewImage.Dispose()
    $srcImage.Dispose()
}

# initialize the arrays with the Android and iOS densities
[PsObject[]]$iOSSizes = @()
$iOSSizes = @(
    @{Size=1; Density=""},
    @{Size=2; Density="@2x"},
    @{Size=3; Density="@3x"}
)

[PsObject[]]$androidSizes = @()
$androidSizes = @(
    @{Size=1;   Density="mdpi"},
    @{Size=1.5; Density="hdpi"},
    @{Size=2;   Density="xhdpi"},
    @{Size=3;   Density="xxhdpi"},
    @{Size=4;   Density="xxxhdpi"}
)

$rootName = [System.IO.Path]::GetFileNameWithoutExtension($srcImage)
$ext = [System.IO.Path]::GetExtension($srcImage)

foreach ($size in $iOSSizes)
{
    # iOS will use different names for each density, but all in one folder
    $newPath = Join-Path $destPath "ios"

    # If the destination folder doesn't exist, create it
    if (!(Test-Path -Path $newPath))
    {
        write-host "Creating new folder" $newPath
        $null = New-Item -Path $newPath -ItemType Container
    }

    # Create the new file name with the destination folder
    $newPath = Join-Path $destPath "ios" ($rootName + $size.Density + $ext)
    write-host "ios convert" $srcImage $newPath
    try {
        ResizePosh -ImagePath $srcImage -OutputPath $newPath -BaseSize $baseSize -Density $size.Size
    }
    catch {
        $error
        throw
    }
}

foreach ($size in $androidSizes)
{
    # Android will use different folder names for each density
    $newPath = Join-Path $destPath $size.Density

    # If the destination folder doesn't exist, create it
    if (!(Test-Path -Path $newPath))
    {
        write-host "Creating new folder" $newPath
        $null = New-Item -Path $newPath -ItemType Container
    }

    # Create the file name with the destination folder
    $newPath = Join-Path $destPath $size.Density  ($rootName + $ext)
    write-host "Android convert" $srcImage $newPath
    try {
        ResizePosh -ImagePath $srcImage -OutputPath $newPath -BaseSize $baseSize -Density $size.Size
    }
    catch {
        $error
        break
    }
}
